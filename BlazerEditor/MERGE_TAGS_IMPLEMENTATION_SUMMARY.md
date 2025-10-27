# Merge Tags Implementation Summary

## Overview

BlazerEditor now supports Unlayer-style merge tags with full autocomplete functionality. Users can either select merge tags from a dropdown menu or trigger an autocomplete menu by typing `{{` in text fields.

## What Was Implemented

### 1. JavaScript Autocomplete System (`wwwroot/js/mergeTagAutocomplete.js`)

**Features:**
- Detects `{{` trigger and shows autocomplete menu
- Real-time filtering as user types
- Keyboard navigation (Arrow Up/Down, Enter, Escape)
- Mouse selection support
- Positions menu dynamically based on cursor position
- Supports all 3 JSON formats (grouped dictionary, grouped array, flat dictionary)

**Key Functions:**
- `buildFormattedMergeTags()`: Transforms merge tags to Unlayer format
- `flattenMergeTags()`: Converts grouped tags to flat array for autocomplete
- `installAutocompleteTrigger()`: Sets up event listeners
- `blazerEditorAutocompleteInitialize()`: Public API to initialize autocomplete

### 2. Updated RichTextEditor Component

**Changes:**
- Added `MergeTags` parameter to accept list of merge tags
- Added `EditorId` parameter for unique identification
- Added JavaScript interop via `IJSRuntime`
- Initializes autocomplete on first render
- Passes merge tags to JavaScript layer

**Integration:**
```razor
<RichTextEditor MergeTags="_mergeTags" EditorId="@uniqueId" />
```

### 3. Updated Models

**MergeTag.cs:**
- Added `MergeTagGroup` class for grouping tags
- Supports both individual tags and grouped tags
- Backward compatible with existing code

**EditorOptions.cs:**
- Already had merge tag support, no changes needed

### 4. Updated EmailEditor Component

**Changes:**
- Passes `_mergeTags` to RichTextEditor instances
- Maintains existing dropdown functionality
- Both dropdown and autocomplete work together

### 5. Documentation

**Created Files:**
- `MERGE_TAGS_GUIDE.md`: Comprehensive usage guide
- Updated `INSTALLATION.md`: Added JavaScript reference

**Documentation Includes:**
- Basic usage examples
- Advanced grouped tags
- All 3 supported JSON formats
- Keyboard shortcuts
- Troubleshooting guide
- Best practices

## How It Works

### Architecture

```
┌─────────────────────────────────────────────────┐
│  EmailEditor Component                          │
│  ┌───────────────────────────────────────────┐ │
│  │ MergeTagPicker (Dropdown)                 │ │
│  │ - Categories                               │ │
│  │ - Search                                   │ │
│  │ - Selection                                │ │
│  └───────────────────────────────────────────┘ │
│                                                  │
│  ┌───────────────────────────────────────────┐ │
│  │ RichTextEditor                             │ │
│  │ ┌───────────────────────────────────────┐ │ │
│  │ │ RadzenHtmlEditor                     │ │ │
│  │ │ - ContentEditable                    │ │ │
│  │ │ - Keyboard Events                    │ │ │
│  │ └───────────────────────────────────────┘ │ │
│  │                                            │ │
│  │ ┌───────────────────────────────────────┐ │ │
│  │ │ JS: mergeTagAutocomplete.js           │ │ │
│  │ │ - Detects {{                          │ │ │
│  │ │ - Shows menu                          │ │ │
│  │ │ - Filters results                     │ │ │
│  │ │ - Inserts tag                         │ │ │
│  │ └───────────────────────────────────────┘ │ │
│  └───────────────────────────────────────────┘ │
└─────────────────────────────────────────────────┘
```

### Data Flow

1. **C# → JSON**: Merge tags serialized to JSON
2. **JSON → JS**: Passed to JavaScript via interop
3. **JS Transform**: Converted to Unlayer format
4. **Flatten**: Converted to flat array for autocomplete
5. **Render**: Menu shows filtered results
6. **Insert**: Selected tag inserted into editor

### Event Flow

**Trigger Detection:**
```
User types '{' → keydown event → prevKey = '{'
User types '{' → keydown event → prevKey === '{' → Open menu
```

**Menu Navigation:**
```
User types 'c' → Append to query → Filter results → Update menu
User presses ArrowDown → Increment index → Highlight item
User presses Enter → Insert selected → Close menu
```

## Supported Formats

### Format 1: Grouped Dictionary (C#)
```json
{
  "Contract": [
    { "Name": "...", "Value": "...", "Sample": "..." }
  ]
}
```

### Format 2: Grouped Array (JavaScript)
```json
[
  {
    "name": "Contract",
    "mergeTags": [
      { "name": "...", "value": "...", "sample": "..." }
    ]
  }
]
```

### Format 3: Flat Dictionary
```json
{
  "first_name": {
    "Name": "...",
    "Value": "...",
    "Sample": "..."
  }
}
```

## Usage Example

```csharp
// Define merge tags
var mergeTags = new List<MergeTag>
{
    new MergeTag { Name = "First Name", Value = "{{first_name}}", Sample = "John" },
    new MergeTag { Name = "Company", Value = "{{company}}", Sample = "Acme Corp" }
};

// Configure editor
var options = new EditorOptions
{
    EnableMergeTags = true,
    ShowMergeTagPreview = true,
    MergeTags = mergeTags
};

// Use editor
<EmailEditor @ref="editor" Options="options" />
```

## Keyboard Shortcuts

| Key | Action |
|-----|--------|
| `{{` | Open autocomplete menu |
| Arrow Down | Next item |
| Arrow Up | Previous item |
| Enter/Tab | Insert selected tag |
| Escape | Close menu |
| Backspace | Delete character / Close if empty |
| Letters | Filter results |

## Integration Points

### For NuGet Package Users

1. **Add JavaScript Reference**:
   ```html
   <script src="_content/BlazerEditor/js/mergeTagAutocomplete.js"></script>
   ```

2. **Pass Merge Tags**:
   ```csharp
   var options = new EditorOptions
   {
       MergeTags = yourMergeTags
   };
   ```

3. **Done!** Autocomplete works automatically

### For Developers

**JavaScript API:**
- `blazerEditorAutocompleteInitialize(editorId, mergeTagsJson)`: Initialize autocomplete
- `blazerEditorInsertMergeTag(editorId, mergeTagValue)`: Manual insertion

**C# API:**
- `MergeTag`: Individual tag model
- `MergeTagGroup`: Group model
- `EditorOptions.MergeTags`: Property to set tags

## Benefits

1. **Familiar UX**: Matches Unlayer's merge tag system
2. **Flexible**: Supports multiple JSON formats
3. **Fast**: Real-time filtering and navigation
4. **Accessible**: Keyboard navigation support
5. **Backward Compatible**: Works with existing merge tag dropdown
6. **Easy Integration**: Minimal setup required

## Testing

### Manual Test Cases

1. **Autocomplete Trigger**
   - Type `{{` in text field
   - Verify menu appears

2. **Filtering**
   - Type `{{fir`
   - Verify only "First Name" appears

3. **Keyboard Navigation**
   - Press Arrow Down
   - Verify highlight moves

4. **Insertion**
   - Press Enter
   - Verify tag inserted

5. **Dropdown**
   - Click "Merge Tags" button
   - Verify dropdown opens
   - Click a tag
   - Verify tag inserted

## Future Enhancements

Potential improvements:
- Add recent tags list
- Add favorites
- Add tag descriptions tooltips
- Add custom icons for categories
- Add multi-language support
- Add accessibility improvements (ARIA labels)

## Files Modified

1. `wwwroot/js/mergeTagAutocomplete.js` - NEW
2. `Components/RichTextEditor.razor` - MODIFIED
3. `Components/EmailEditor.razor` - MODIFIED
4. `Models/MergeTag.cs` - MODIFIED
5. `INSTALLATION.md` - MODIFIED
6. `MERGE_TAGS_GUIDE.md` - NEW
7. `BlazerEditor.csproj` - MODIFIED
8. `BlazerEditorTest/Pages/_Host.cshtml` - MODIFIED

## Compatibility

- **Blazor Server**: ✅ Fully supported
- **Blazor WebAssembly**: ✅ Should work (untested)
- **Radzen HTML Editor**: ✅ Integrated
- **All Modern Browsers**: ✅ Supported

## Performance

- **Menu Rendering**: O(n) where n = number of tags
- **Filtering**: O(n) per keystroke
- **Memory**: Minimal per editor instance
- **Network**: Only loads JavaScript once

## Security

- No external dependencies
- No data sent to external servers
- All processing client-side
- Safe for sensitive data

## Summary

The merge tag autocomplete system is now fully functional and ready for use. It provides a familiar, fast, and flexible way to insert merge tags into email templates, matching the user experience of Unlayer while being fully integrated with BlazerEditor.


