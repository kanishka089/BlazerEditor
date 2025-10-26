using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using BlazerEditor.Models;

namespace BlazerEditor.Components;

public partial class EmailEditor : ComponentBase
{
    [Parameter] public EditorOptions Options { get; set; } = new();
    [Parameter] public EventCallback OnReady { get; set; }
    [Parameter] public EventCallback<ExportResult> OnExport { get; set; }
    [Parameter] public EventCallback<EmailDesign> OnSave { get; set; }
    [Parameter] public bool ShowExportButton { get; set; } = true;
    [Parameter] public bool ShowSaveButton { get; set; } = true;
    
    [Inject] public IJSRuntime JSRuntime { get; set; } = default!;

    private EmailDesign _currentDesign = new();
    private List<EmailDesign> _undoStack = new();
    private List<EmailDesign> _redoStack = new();
    private bool _canUndo => _undoStack.Count > 0;
    private bool _canRedo => _redoStack.Count > 0;
    
    private PreviewMode _previewMode = PreviewMode.Desktop;
    private int _selectedRowIndex = -1;
    private int _selectedColumnIndex = -1;
    private int _selectedContentIndex = -1;
    private Content? _selectedContent = null;
    
    private ComponentDefinition? _draggedComponent = null;
    private Content? _draggedContent = null;
    private int _draggedContentRowIndex = -1;
    private int _draggedContentColumnIndex = -1;
    private int _draggedContentIndex = -1;
    private int _dropTargetRowIndex = -1;
    private int _dropTargetColumnIndex = -1;
    private int _dropTargetIndex = -1;
    private int _rowDropTargetIndex = -1;
    private int _hoveredRowIndex = -1;
    private int _hoveredColumnRowIndex = -1;
    private int _hoveredColumnIndex = -1;
    private int _splitClickCounter = 0;

    private readonly List<ComponentDefinition> _availableComponents = new()
    {
        new() { Type = "text", Label = "Text", Icon = "📝" },
        new() { Type = "heading", Label = "Heading", Icon = "📰" },
        new() { Type = "image", Label = "Image", Icon = "🖼️" },
    };

    private readonly List<LayoutDefinition> _availableLayouts = new()
    {
        // Removed Row - users will add rows via + buttons
    };

    private LayoutDefinition? _draggedLayout = null;

    protected override async Task OnInitializedAsync()
    {
        _currentDesign = DesignService.CreateEmptyDesign();
        
        // Add a default row on load
        if (!_currentDesign.Body.Rows.Any())
        {
            var defaultRow = new Row
            {
                Cells = new List<int> { 100 },
                Columns = new List<Column>
                {
                    new Column
                    {
                        Id = Guid.NewGuid().ToString("N")[..10],
                        Contents = new List<Content>()
                    }
                }
            };
            _currentDesign.Body.Rows.Add(defaultRow);
        }
        
        await OnReady.InvokeAsync();
    }

    private string ThemeClass => Options.Appearance?.Theme switch
    {
        "modern_dark" => "theme-dark",
        "classic" => "theme-classic",
        _ => "theme-light"
    };

    private string GetCanvasClass() => _previewMode switch
    {
        PreviewMode.Mobile => "canvas-mobile",
        _ => "canvas-desktop"
    };

    private string GetRowClass(int index) => _selectedRowIndex == index ? "selected" : "";
    
    private string GetContentClass(int rowIndex, int colIndex, int contentIndex)
    {
        return IsContentSelected(rowIndex, colIndex, contentIndex) ? "selected" : "";
    }

    private string GetRowDropZoneClass(int index)
    {
        return _rowDropTargetIndex == index ? "active" : "";
    }

    private bool IsContentSelected(int rowIndex, int colIndex, int contentIndex)
    {
        return _selectedRowIndex == rowIndex && 
               _selectedColumnIndex == colIndex && 
               _selectedContentIndex == contentIndex;
    }

    private string GetColumnClass(Column column)
    {
        // Column has content if it has regular contents OR sub-columns with content
        var hasRegularContent = column.Contents.Any();
        var hasSubColumnContent = column.HasSubColumns && column.SubColumns!.Any(sc => sc.Contents.Any());
        
        return (hasRegularContent || hasSubColumnContent) ? "has-content" : "";
    }

    // Public API Methods
    public async Task<ExportResult> ExportHtmlAsync()
    {
        var html = HtmlExportService.ExportToHtml(_currentDesign);
        var result = new ExportResult
        {
            Html = html,
            Design = _currentDesign
        };
        return result;
    }

    public async Task<EmailDesign> SaveDesignAsync()
    {
        return _currentDesign;
    }

    public async Task LoadDesignAsync(string designJson)
    {
        var design = DesignService.DeserializeDesign(designJson);
        if (design != null)
        {
            SaveToUndoStack();
            _currentDesign = design;
            StateHasChanged();
        }
    }

    public async Task LoadDesignAsync(EmailDesign design)
    {
        SaveToUndoStack();
        _currentDesign = design;
        StateHasChanged();
    }

    // Event Handlers
    private async Task HandleExport()
    {
        var result = await ExportHtmlAsync();
        await OnExport.InvokeAsync(result);
    }

    private async Task HandleSave()
    {
        await OnSave.InvokeAsync(_currentDesign);
    }

    private void SetPreviewMode(PreviewMode mode)
    {
        _previewMode = mode;
    }

    private void SelectRow(int index)
    {
        _selectedRowIndex = index;
        _selectedColumnIndex = -1;
        _selectedContentIndex = -1;
        _selectedContent = null;
    }

    private void DeselectAll()
    {
        _selectedRowIndex = -1;
        _selectedColumnIndex = -1;
        _selectedContentIndex = -1;
        _selectedContent = null;
    }

    private void SelectContent(int rowIndex, int colIndex, int contentIndex)
    {
        _selectedRowIndex = rowIndex;
        _selectedColumnIndex = colIndex;
        _selectedContentIndex = contentIndex;
        _selectedContent = _currentDesign.Body.Rows[rowIndex].Columns[colIndex].Contents[contentIndex];
    }

    private void SelectNestedContent(Content content)
    {
        _selectedRowIndex = -1;
        _selectedColumnIndex = -1;
        _selectedContentIndex = -1;
        _selectedContent = content;
        Console.WriteLine($"Selected nested content: {content.Type}");
    }

    private void DeleteRow(int index)
    {
        // Prevent deleting the last row
        if (_currentDesign.Body.Rows.Count <= 1)
        {
            return;
        }
        
        SaveToUndoStack();
        _currentDesign.Body.Rows.RemoveAt(index);
        _selectedRowIndex = -1;
    }

    private void DuplicateRow(int index)
    {
        SaveToUndoStack();
        var row = _currentDesign.Body.Rows[index];
        var newRow = System.Text.Json.JsonSerializer.Deserialize<Row>(
            System.Text.Json.JsonSerializer.Serialize(row)
        );
        if (newRow != null)
        {
            newRow.Id = Guid.NewGuid().ToString("N")[..10];
            _currentDesign.Body.Rows.Insert(index + 1, newRow);
        }
    }

    private void AddRowAbove(int index)
    {
        SaveToUndoStack();
        var newRow = new Row
        {
            Cells = new List<int> { 100 },
            Columns = new List<Column>
            {
                new Column
                {
                    Id = Guid.NewGuid().ToString("N")[..10],
                    Contents = new List<Content>()
                }
            }
        };
        _currentDesign.Body.Rows.Insert(index, newRow);
    }

    private void AddRowBelow(int index)
    {
        SaveToUndoStack();
        var newRow = new Row
        {
            Cells = new List<int> { 100 },
            Columns = new List<Column>
            {
                new Column
                {
                    Id = Guid.NewGuid().ToString("N")[..10],
                    Contents = new List<Content>()
                }
            }
        };
        _currentDesign.Body.Rows.Insert(index + 1, newRow);
    }

    private void MoveRowUp(int index)
    {
        if (index == 0) return;
        
        SaveToUndoStack();
        var row = _currentDesign.Body.Rows[index];
        _currentDesign.Body.Rows.RemoveAt(index);
        _currentDesign.Body.Rows.Insert(index - 1, row);
        _selectedRowIndex = index - 1;
    }

    private void MoveRowDown(int index)
    {
        if (index >= _currentDesign.Body.Rows.Count - 1) return;
        
        SaveToUndoStack();
        var row = _currentDesign.Body.Rows[index];
        _currentDesign.Body.Rows.RemoveAt(index);
        _currentDesign.Body.Rows.Insert(index + 1, row);
        _selectedRowIndex = index + 1;
    }

    private void SetColumnCount(int rowIndex, int columnCount)
    {
        SaveToUndoStack();
        var row = _currentDesign.Body.Rows[rowIndex];
        
        // Save all existing content
        var allContent = row.Columns.SelectMany(c => c.Contents).ToList();
        
        // Clear and recreate columns
        row.Columns.Clear();
        row.Cells.Clear();
        
        var widthPerColumn = 100 / columnCount;
        for (int i = 0; i < columnCount; i++)
        {
            row.Columns.Add(new Column
            {
                Id = Guid.NewGuid().ToString("N")[..10],
                Contents = new List<Content>()
            });
            row.Cells.Add(widthPerColumn);
        }
        
        // Put all content in first column
        if (allContent.Any())
        {
            row.Columns[0].Contents = allContent;
        }
        
        StateHasChanged();
    }

    private void SplitColumn(int rowIndex, int colIndex)
    {
        SaveToUndoStack();
        var row = _currentDesign.Body.Rows[rowIndex];
        
        if (colIndex >= row.Columns.Count) return;
        
        var columnToSplit = row.Columns[colIndex];
        var currentWidth = row.Cells.Count > colIndex ? row.Cells[colIndex] : (100 / row.Columns.Count);
        
        Console.WriteLine($"SPLITTING ROW-LEVEL COLUMN: Row {rowIndex}, Column {colIndex}, Current width: {currentWidth}%");
        Console.WriteLine($"BEFORE SPLIT - Row.Cells: [{string.Join(", ", row.Cells)}]");
        
        // Calculate new widths (split the current column in half)
        var newWidth = currentWidth / 2;
        Console.WriteLine($"New width for each split column: {newWidth}%");
        
        // Create a new column with the same content as the original
        var newColumn = new Column
        {
            Id = Guid.NewGuid().ToString("N")[..10],
            Contents = new List<Content>(), // Start empty
            Values = new ColumnValues()
        };
        
        // Update the width of the original column
        row.Cells[colIndex] = newWidth;
        
        // Insert the new column right after the current one
        row.Columns.Insert(colIndex + 1, newColumn);
        row.Cells.Insert(colIndex + 1, newWidth);
        
        Console.WriteLine($"AFTER ROW SPLIT: Row now has {row.Columns.Count} columns with widths: {string.Join(", ", row.Cells)}");
        
        StateHasChanged();
    }

    private void OnColumnMouseEnter(int rowIndex, int colIndex)
    {
        _hoveredColumnRowIndex = rowIndex;
        _hoveredColumnIndex = colIndex;
        Console.WriteLine($"Hovering column {colIndex} in row {rowIndex}");
        StateHasChanged();
    }

    private void OnColumnMouseLeave()
    {
        _hoveredColumnRowIndex = -1;
        _hoveredColumnIndex = -1;
        Console.WriteLine("Left column hover");
        StateHasChanged();
    }

    private async Task OnSplitColumnClick(int rowIndex, int colIndex)
    {
        Console.WriteLine($"Splitting column {colIndex} in row {rowIndex}");
        SplitColumn(rowIndex, colIndex);
    }

    private void TestClick()
    {
        Console.WriteLine("TEST CLICK WORKED!");
        // Also try JavaScript alert to make sure we see something
        _ = Task.Run(async () => {
            await Task.Delay(100);
            Console.WriteLine("Delayed test message");
        });
    }

    private void DeleteContent(int rowIndex, int colIndex, int contentIndex)
    {
        SaveToUndoStack();
        _currentDesign.Body.Rows[rowIndex].Columns[colIndex].Contents.RemoveAt(contentIndex);
        _selectedContent = null;
        _selectedContentIndex = -1;
    }

    private void MoveContentUp(int rowIndex, int colIndex, int contentIndex)
    {
        if (contentIndex == 0) return;
        
        SaveToUndoStack();
        var column = _currentDesign.Body.Rows[rowIndex].Columns[colIndex];
        var content = column.Contents[contentIndex];
        column.Contents.RemoveAt(contentIndex);
        column.Contents.Insert(contentIndex - 1, content);
        
        // Update selection
        _selectedContentIndex = contentIndex - 1;
    }

    private void MoveContentDown(int rowIndex, int colIndex, int contentIndex)
    {
        var column = _currentDesign.Body.Rows[rowIndex].Columns[colIndex];
        if (contentIndex >= column.Contents.Count - 1) return;
        
        SaveToUndoStack();
        var content = column.Contents[contentIndex];
        column.Contents.RemoveAt(contentIndex);
        column.Contents.Insert(contentIndex + 1, content);
        
        // Update selection
        _selectedContentIndex = contentIndex + 1;
    }

    private void DuplicateContent(int rowIndex, int colIndex, int contentIndex)
    {
        SaveToUndoStack();
        var column = _currentDesign.Body.Rows[rowIndex].Columns[colIndex];
        var content = column.Contents[contentIndex];
        
        var json = System.Text.Json.JsonSerializer.Serialize(content);
        var duplicate = System.Text.Json.JsonSerializer.Deserialize<Content>(json);
        
        if (duplicate != null)
        {
            duplicate.Id = Guid.NewGuid().ToString("N")[..10];
            column.Contents.Insert(contentIndex + 1, duplicate);
        }
    }

    // Drag and Drop - Layouts
    private void OnLayoutDragStart(LayoutDefinition layout)
    {
        _draggedLayout = layout;
        Console.WriteLine($"Layout drag started: {layout.Label}");
    }

    private void OnLayoutDragEnd()
    {
        Console.WriteLine("Layout drag ended");
        _draggedLayout = null;
        _rowDropTargetIndex = -1;
    }

    private void OnRowDragOver(int index)
    {
        if (_draggedLayout != null)
        {
            _rowDropTargetIndex = index;
            StateHasChanged();
        }
    }

    private void OnRowDrop(int index)
    {
        Console.WriteLine($"OnRowDrop called at index {index}, draggedLayout: {_draggedLayout?.Label}");
        
        if (_draggedLayout == null) return;

        SaveToUndoStack();
        
        var newRow = new Row
        {
            Cells = new List<int>(),
            Columns = new List<Column>()
        };
        
        // Create columns based on layout
        var widths = _draggedLayout.ColumnWidths ?? Enumerable.Repeat(100 / _draggedLayout.Columns, _draggedLayout.Columns).ToList();
        for (int i = 0; i < _draggedLayout.Columns; i++)
        {
            newRow.Cells.Add(widths[i]);
            newRow.Columns.Add(new Column
            {
                Id = Guid.NewGuid().ToString("N")[..10],
                Contents = new List<Content>()
            });
        }
        
        _currentDesign.Body.Rows.Insert(index, newRow);
        
        Console.WriteLine($"Row inserted at index {index}: {_draggedLayout.Columns} columns");
        _draggedLayout = null;
        _rowDropTargetIndex = -1;
        StateHasChanged();
    }

    // Drag and Drop - Components from library
    private void OnDragStart(ComponentDefinition component)
    {
        _draggedComponent = component;
    }

    private void OnDragEnd()
    {
        _draggedComponent = null;
    }

    // Drag and Drop - Content reordering
    private void OnContentDragStart(int rowIndex, int colIndex, int contentIndex)
    {
        _draggedContent = _currentDesign.Body.Rows[rowIndex].Columns[colIndex].Contents[contentIndex];
        _draggedContentRowIndex = rowIndex;
        _draggedContentColumnIndex = colIndex;
        _draggedContentIndex = contentIndex;
    }

    private void OnContentDragEnd()
    {
        _draggedContent = null;
        _draggedContentRowIndex = -1;
        _draggedContentColumnIndex = -1;
        _draggedContentIndex = -1;
        _dropTargetRowIndex = -1;
        _dropTargetColumnIndex = -1;
        _dropTargetIndex = -1;
    }

    private void OnContentDragOver(int rowIndex, int colIndex, int contentIndex)
    {
        _dropTargetRowIndex = rowIndex;
        _dropTargetColumnIndex = colIndex;
        _dropTargetIndex = contentIndex;
    }

    private void OnContentDragLeave()
    {
        _dropTargetRowIndex = -1;
        _dropTargetColumnIndex = -1;
        _dropTargetIndex = -1;
    }

    private void OnContentDrop(int targetRowIndex, int targetColIndex, int targetIndex)
    {
        SaveToUndoStack();

        // Handle reordering existing content
        if (_draggedContent != null)
        {
            if (_draggedContentRowIndex < 0 || _draggedContentColumnIndex < 0 || _draggedContentIndex < 0) return;

            // Remove from original position
            var sourceColumn = _currentDesign.Body.Rows[_draggedContentRowIndex].Columns[_draggedContentColumnIndex];
            sourceColumn.Contents.RemoveAt(_draggedContentIndex);

            // Add to new position
            var targetColumn = _currentDesign.Body.Rows[targetRowIndex].Columns[targetColIndex];
            
            // Adjust target index if moving within same column
            if (_draggedContentRowIndex == targetRowIndex && 
                _draggedContentColumnIndex == targetColIndex && 
                _draggedContentIndex < targetIndex)
            {
                targetIndex--;
            }

            targetColumn.Contents.Insert(targetIndex, _draggedContent);
            OnContentDragEnd();
            StateHasChanged();
            return;
        }

        // Handle dropping new layout from library at specific position
        if (_draggedLayout != null)
        {
            var layoutContent = new Content
            {
                Type = "layout",
                Columns = new List<Column>()
            };

            // Create sub-columns based on layout
            for (int i = 0; i < _draggedLayout.Columns; i++)
            {
                layoutContent.Columns.Add(new Column
                {
                    Id = Guid.NewGuid().ToString("N")[..10],
                    Contents = new List<Content>()
                });
            }

            var targetColumn = _currentDesign.Body.Rows[targetRowIndex].Columns[targetColIndex];
            targetColumn.Contents.Insert(targetIndex, layoutContent);
            
            Console.WriteLine($"Layout inserted at position {targetIndex}: {_draggedLayout.Columns} columns");
            _draggedLayout = null;
            
            // Clear drop zone indicators
            _dropTargetRowIndex = -1;
            _dropTargetColumnIndex = -1;
            _dropTargetIndex = -1;
            
            StateHasChanged();
            return;
        }

        // Handle dropping new component from library
        if (_draggedComponent != null)
        {
            var content = CreateContent(_draggedComponent.Type);
            var targetColumn = _currentDesign.Body.Rows[targetRowIndex].Columns[targetColIndex];
            targetColumn.Contents.Insert(targetIndex, content);
            
            DesignService.IncrementCounter(_currentDesign, $"u_content_{_draggedComponent.Type}");
            _draggedComponent = null;
            
            // Clear drop zone indicators
            _dropTargetRowIndex = -1;
            _dropTargetColumnIndex = -1;
            _dropTargetIndex = -1;
            
            StateHasChanged();
            return;
        }
    }

    private void OnDragOver(Microsoft.AspNetCore.Components.Web.DragEventArgs e)
    {
        // Allow drop - preventDefault is handled by Blazor attribute
    }



    private void OnColumnDrop(int rowIndex, int colIndex)
    {
        Console.WriteLine($"OnColumnDrop called at row: {rowIndex}, col: {colIndex}, draggedLayout: {_draggedLayout?.Label}, draggedComponent: {_draggedComponent?.Label}");
        
        SaveToUndoStack();

        // Handle layout drop - split the column instead of nesting
        if (_draggedLayout != null)
        {
            var row = _currentDesign.Body.Rows[rowIndex];
            
            // Ensure Cells list is synchronized with Columns
            while (row.Cells.Count < row.Columns.Count)
            {
                row.Cells.Add(100 / row.Columns.Count);
            }
            
            var targetColumn = row.Columns[colIndex];
            
            // Save the existing content from the column
            var existingContent = new List<Content>(targetColumn.Contents);
            
            // Remove the target column
            row.Columns.RemoveAt(colIndex);
            if (colIndex < row.Cells.Count)
            {
                row.Cells.RemoveAt(colIndex);
            }
            
            // Insert new columns at the same position with custom widths
            var widths = _draggedLayout.ColumnWidths ?? Enumerable.Repeat(100 / _draggedLayout.Columns, _draggedLayout.Columns).ToList();
            
            // Ensure we have enough widths
            while (widths.Count < _draggedLayout.Columns)
            {
                widths.Add(100 / _draggedLayout.Columns);
            }
            
            for (int i = 0; i < _draggedLayout.Columns; i++)
            {
                var newColumn = new Column
                {
                    Id = Guid.NewGuid().ToString("N")[..10],
                    Contents = new List<Content>()
                };
                
                // Put existing content in the first new column
                if (i == 0)
                {
                    newColumn.Contents = existingContent;
                }
                
                row.Columns.Insert(colIndex + i, newColumn);
                row.Cells.Insert(colIndex + i, widths[i]); // Store width percentage
            }
            
            Console.WriteLine($"Column split into {_draggedLayout.Columns} columns");
            _draggedLayout = null;
            StateHasChanged();
            return;
        }

        // Handle component drops in columns
        if (_draggedComponent == null) return;

        var content = CreateContent(_draggedComponent.Type);
        _currentDesign.Body.Rows[rowIndex].Columns[colIndex].Contents.Add(content);

        DesignService.IncrementCounter(_currentDesign, $"u_content_{_draggedComponent.Type}");
        _draggedComponent = null;
        StateHasChanged();
    }

    private void OnDrop(int rowIndex)
    {
        Console.WriteLine($"OnDrop called at rowIndex: {rowIndex}, draggedLayout: {_draggedLayout?.Label}, draggedComponent: {_draggedComponent?.Label}");
        
        // Handle layout drop
        if (_draggedLayout != null)
        {
            SaveToUndoStack();
            
            var newRow = new Row
            {
                Cells = new List<int>(),
                Columns = new List<Column>()
            };
            
            // Create columns based on layout with custom widths
            var widths = _draggedLayout.ColumnWidths ?? Enumerable.Repeat(100 / _draggedLayout.Columns, _draggedLayout.Columns).ToList();
            for (int i = 0; i < _draggedLayout.Columns; i++)
            {
                newRow.Cells.Add(widths[i]); // Store width percentage
                newRow.Columns.Add(new Column
                {
                    Id = Guid.NewGuid().ToString("N")[..10],
                    Contents = new List<Content>()
                });
            }
            
            if (rowIndex < 0 || !_currentDesign.Body.Rows.Any())
            {
                _currentDesign.Body.Rows.Add(newRow);
            }
            else
            {
                _currentDesign.Body.Rows.Insert(rowIndex + 1, newRow);
            }
            
            Console.WriteLine($"Layout added: {_draggedLayout.Columns} columns");
            _draggedLayout = null;
            StateHasChanged();
            return;
        }

        // Handle component drop on row (add to first column)
        if (_draggedComponent == null) return;

        SaveToUndoStack();

        var content = CreateContent(_draggedComponent.Type);
        
        if (rowIndex < 0 || !_currentDesign.Body.Rows.Any())
        {
            // Create new row
            var newRow = new Row
            {
                Cells = new List<int> { 1 },
                Columns = new List<Column>
                {
                    new Column
                    {
                        Contents = new List<Content> { content }
                    }
                }
            };
            _currentDesign.Body.Rows.Add(newRow);
        }
        else
        {
            // Add to existing row's first column
            _currentDesign.Body.Rows[rowIndex].Columns[0].Contents.Add(content);
        }

        DesignService.IncrementCounter(_currentDesign, $"u_content_{_draggedComponent.Type}");
        _draggedComponent = null;
    }

    private Content CreateContent(string type)
    {
        var content = new Content { Type = type };
        
        switch (type)
        {
            case "text":
                content.Values.Text = "<p>Enter your text here...</p>";
                break;
            case "heading":
                content.Values.Text = "Your Heading";
                content.Values.FontSize = "24px";
                break;
            case "image":
                content.Values.Src = new ImageSource
                {
                    Url = "",
                    Width = 600,
                    Height = 400
                };
                break;
            case "button":
                content.Values.Text = "Click Here";
                content.Values.Href = new LinkAction();
                content.Values.ButtonColors = new ButtonColors();
                break;
            case "divider":
                content.Values.Color = "#CCCCCC";
                content.Values.TextAlign = "horizontal"; // Store orientation in TextAlign
                content.Values.FontSize = "1px"; // Default thickness
                break;
            case "column-divider":
                content.Values.Color = "#CCCCCC";
                content.Values.TextAlign = "vertical"; // Vertical orientation
                content.Values.FontSize = "2px"; // Default thickness
                break;
        }
        
        return content;
    }

    // Undo/Redo
    private void SaveToUndoStack()
    {
        var json = System.Text.Json.JsonSerializer.Serialize(_currentDesign);
        var copy = System.Text.Json.JsonSerializer.Deserialize<EmailDesign>(json);
        if (copy != null)
        {
            _undoStack.Add(copy);
            _redoStack.Clear();
            
            // Limit undo stack size
            if (_undoStack.Count > 50)
            {
                _undoStack.RemoveAt(0);
            }
        }
    }

    private void Undo()
    {
        if (!_canUndo) return;
        
        var current = System.Text.Json.JsonSerializer.Serialize(_currentDesign);
        var currentCopy = System.Text.Json.JsonSerializer.Deserialize<EmailDesign>(current);
        if (currentCopy != null)
        {
            _redoStack.Add(currentCopy);
        }
        
        _currentDesign = _undoStack[^1];
        _undoStack.RemoveAt(_undoStack.Count - 1);
    }

    private void Redo()
    {
        if (!_canRedo) return;
        
        SaveToUndoStack();
        _currentDesign = _redoStack[^1];
        _redoStack.RemoveAt(_redoStack.Count - 1);
    }

    // Sub-column helper methods
    private string GetSubDropZoneClass(int rowIndex, int colIndex, int subIndex, int contentIndex)
    {
        // For now, return empty - can be enhanced later
        return "";
    }

    private string GetSubContentClass(int rowIndex, int colIndex, int subIndex, int contentIndex)
    {
        // For now, return empty - can be enhanced later  
        return "";
    }

    private bool IsSubContentSelected(int rowIndex, int colIndex, int subIndex, int contentIndex)
    {
        // For now, return false - can be enhanced later
        return false;
    }

    private void OnSubContentDrop(int rowIndex, int colIndex, int subIndex, int contentIndex)
    {
        // Handle dropping content into sub-columns
        SaveToUndoStack();

        if (_draggedComponent != null)
        {
            var content = CreateContent(_draggedComponent.Type);
            var subColumn = _currentDesign.Body.Rows[rowIndex].Columns[colIndex].SubColumns![subIndex];
            subColumn.Contents.Insert(contentIndex, content);
            
            DesignService.IncrementCounter(_currentDesign, $"u_content_{_draggedComponent.Type}");
            _draggedComponent = null;
            StateHasChanged();
        }
    }

    private void OnSubContentDragOver(int rowIndex, int colIndex, int subIndex, int contentIndex)
    {
        // Handle drag over for sub-columns
    }

    private void OnSubContentDragStart(int rowIndex, int colIndex, int subIndex, int contentIndex)
    {
        // Handle drag start for sub-column content
        var subColumn = _currentDesign.Body.Rows[rowIndex].Columns[colIndex].SubColumns![subIndex];
        _draggedContent = subColumn.Contents[contentIndex];
        _draggedContentRowIndex = rowIndex;
        _draggedContentColumnIndex = colIndex;
        _draggedContentIndex = contentIndex;
    }

    private void SelectSubContent(int rowIndex, int colIndex, int subIndex, int contentIndex)
    {
        // Handle selecting sub-column content
        var subColumn = _currentDesign.Body.Rows[rowIndex].Columns[colIndex].SubColumns![subIndex];
        _selectedContent = subColumn.Contents[contentIndex];
        _selectedRowIndex = rowIndex;
        _selectedColumnIndex = colIndex;
        _selectedContentIndex = contentIndex;
    }

    private void MoveSubContentUp(int rowIndex, int colIndex, int subIndex, int contentIndex)
    {
        if (contentIndex == 0) return;
        
        SaveToUndoStack();
        var subColumn = _currentDesign.Body.Rows[rowIndex].Columns[colIndex].SubColumns![subIndex];
        var content = subColumn.Contents[contentIndex];
        subColumn.Contents.RemoveAt(contentIndex);
        subColumn.Contents.Insert(contentIndex - 1, content);
        StateHasChanged();
    }

    private void MoveSubContentDown(int rowIndex, int colIndex, int subIndex, int contentIndex)
    {
        var subColumn = _currentDesign.Body.Rows[rowIndex].Columns[colIndex].SubColumns![subIndex];
        if (contentIndex >= subColumn.Contents.Count - 1) return;
        
        SaveToUndoStack();
        var content = subColumn.Contents[contentIndex];
        subColumn.Contents.RemoveAt(contentIndex);
        subColumn.Contents.Insert(contentIndex + 1, content);
        StateHasChanged();
    }

    private void DuplicateSubContent(int rowIndex, int colIndex, int subIndex, int contentIndex)
    {
        SaveToUndoStack();
        var subColumn = _currentDesign.Body.Rows[rowIndex].Columns[colIndex].SubColumns![subIndex];
        var content = subColumn.Contents[contentIndex];
        var duplicatedContent = System.Text.Json.JsonSerializer.Deserialize<Content>(
            System.Text.Json.JsonSerializer.Serialize(content)
        );
        if (duplicatedContent != null)
        {
            subColumn.Contents.Insert(contentIndex + 1, duplicatedContent);
            StateHasChanged();
        }
    }

    private void DeleteSubContent(int rowIndex, int colIndex, int subIndex, int contentIndex)
    {
        SaveToUndoStack();
        var subColumn = _currentDesign.Body.Rows[rowIndex].Columns[colIndex].SubColumns![subIndex];
        subColumn.Contents.RemoveAt(contentIndex);
        _selectedContent = null;
        _selectedContentIndex = -1;
        StateHasChanged();
    }
    private async Task HandleImageUpload(Microsoft.AspNetCore.Components.Forms.InputFileChangeEventArgs e, Content content)
    {
        var file = e.File;
        if (file != null)
        {
            // Read file as base64
            var buffer = new byte[file.Size];
            await file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024).ReadAsync(buffer); // 10MB max
            var base64 = Convert.ToBase64String(buffer);
            var dataUrl = $"data:{file.ContentType};base64,{base64}";
            
            // Update the image source
            content.Values.Src!.Url = dataUrl;
            
            StateHasChanged();
        }
    }
}

public enum PreviewMode
{
    Desktop,
    Mobile
}

public class ComponentDefinition
{
    public string Type { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
}

public class LayoutDefinition
{
    public int Columns { get; set; }
    public string Label { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public List<int>? ColumnWidths { get; set; } // Width percentages for each column
}