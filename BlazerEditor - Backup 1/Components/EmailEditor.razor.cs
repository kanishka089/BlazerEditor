using Microsoft.AspNetCore.Components;
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

    private readonly List<ComponentDefinition> _availableComponents = new()
    {
        new() { Type = "text", Label = "Text", Icon = "📝" },
        new() { Type = "heading", Label = "Heading", Icon = "📰" },
        new() { Type = "image", Label = "Image", Icon = "🖼️" },
        new() { Type = "button", Label = "Button", Icon = "🔘" },
        new() { Type = "divider", Label = "Divider", Icon = "➖" },
        new() { Type = "column-divider", Label = "Column Divider", Icon = "│" },
    };

    private readonly List<LayoutDefinition> _availableLayouts = new()
    {
        new() { Columns = 1, Label = "1 Column", Icon = "▭" },
        new() { Columns = 2, Label = "2 Columns", Icon = "▭▭" },
        new() { Columns = 3, Label = "3 Columns", Icon = "▭▭▭" },
        new() { Columns = 4, Label = "4 Columns", Icon = "▭▭▭▭" },
    };

    private LayoutDefinition? _draggedLayout = null;

    protected override async Task OnInitializedAsync()
    {
        _currentDesign = DesignService.CreateEmptyDesign();
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

    private bool IsContentSelected(int rowIndex, int colIndex, int contentIndex)
    {
        return _selectedRowIndex == rowIndex && 
               _selectedColumnIndex == colIndex && 
               _selectedContentIndex == contentIndex;
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

    private void AddColumn(int rowIndex)
    {
        SaveToUndoStack();
        var row = _currentDesign.Body.Rows[rowIndex];
        
        // Add a new column
        var newColumn = new Column
        {
            Id = Guid.NewGuid().ToString("N")[..10],
            Contents = new List<Content>()
        };
        
        row.Columns.Add(newColumn);
        row.Cells.Add(1); // Add cell count
    }

    private void RemoveColumn(int rowIndex)
    {
        var row = _currentDesign.Body.Rows[rowIndex];
        if (row.Columns.Count <= 1) return;
        
        SaveToUndoStack();
        row.Columns.RemoveAt(row.Columns.Count - 1);
        row.Cells.RemoveAt(row.Cells.Count - 1);
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
            StateHasChanged();
            return;
        }
    }

    private void OnDragOver(Microsoft.AspNetCore.Components.Web.DragEventArgs e)
    {
        // Allow drop - preventDefault is handled by Blazor attribute
    }

    private void OnNestedColumnDrop(Content layoutContent, int nestedColIndex)
    {
        Console.WriteLine($"OnNestedColumnDrop called at nestedCol: {nestedColIndex}, draggedLayout: {_draggedLayout?.Label}, draggedComponent: {_draggedComponent?.Label}");
        
        SaveToUndoStack();

        // Handle nested layout drop (deeper nesting)
        if (_draggedLayout != null)
        {
            var nestedLayoutContent = new Content
            {
                Type = "layout",
                Columns = new List<Column>()
            };

            // Create sub-columns based on layout
            for (int i = 0; i < _draggedLayout.Columns; i++)
            {
                nestedLayoutContent.Columns.Add(new Column
                {
                    Id = Guid.NewGuid().ToString("N")[..10],
                    Contents = new List<Content>()
                });
            }

            layoutContent.Columns![nestedColIndex].Contents.Add(nestedLayoutContent);
            Console.WriteLine($"Deeper nested layout added: {_draggedLayout.Columns} columns");
            _draggedLayout = null;
            StateHasChanged();
            return;
        }

        // Handle component drops in nested columns
        if (_draggedComponent == null) return;

        var content = CreateContent(_draggedComponent.Type);
        layoutContent.Columns![nestedColIndex].Contents.Add(content);

        DesignService.IncrementCounter(_currentDesign, $"u_content_{_draggedComponent.Type}");
        _draggedComponent = null;
        StateHasChanged();
    }

    private void OnColumnDrop(int rowIndex, int colIndex)
    {
        Console.WriteLine($"OnColumnDrop called at row: {rowIndex}, col: {colIndex}, draggedLayout: {_draggedLayout?.Label}, draggedComponent: {_draggedComponent?.Label}");
        
        SaveToUndoStack();

        // Handle nested layout drop
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

            _currentDesign.Body.Rows[rowIndex].Columns[colIndex].Contents.Add(layoutContent);
            Console.WriteLine($"Nested layout added: {_draggedLayout.Columns} columns");
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
            
            // Create columns based on layout
            for (int i = 0; i < _draggedLayout.Columns; i++)
            {
                newRow.Cells.Add(1);
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
                    Url = "https://via.placeholder.com/600x400",
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
}
