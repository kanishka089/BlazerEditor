# Merge Tags Guide

BlazerEditor supports Unlayer-style merge tags with both dropdown selection and autocomplete functionality.

## Features

- **Dropdown Selection**: Browse and select merge tags from an organized dropdown menu
- **Autocomplete**: Type `{{` to trigger an autocomplete menu with filtering
- **Keyboard Navigation**: Navigate autocomplete menu with arrow keys
- **Multiple Formats**: Supports 3 different JSON formats for compatibility
- **Grouped Tags**: Organize tags into logical groups (e.g., Contract, Person, Realestate)

## Basic Usage

### 1. Define Merge Tags

```csharp
var mergeTags = new List<MergeTag>
{
    new MergeTag
    {
        Name = "First Name",
        Value = "{{first_name}}",
        Sample = "John",
        Category = "Personal"
    },
    new MergeTag
    {
        Name = "Last Name",
        Value = "{{last_name}}",
        Sample = "Doe",
        Category = "Personal"
    },
    new MergeTag
    {
        Name = "Company",
        Value = "{{company}}",
        Sample = "Acme Corp",
        Category = "Company"
    }
};
```

### 2. Configure Editor Options

```csharp
var options = new EditorOptions
{
    EnableMergeTags = true,
    ShowMergeTagPreview = true,
    MergeTags = mergeTags
};
```

### 3. Use the Editor

```razor
<EmailEditor @ref="editor" Options="options" />

@code {
    private EmailEditor? editor;
}
```

## Dropdown Selection

Click the "Merge Tags" button in the toolbar to open the dropdown menu. You can:

- **Search**: Type to filter tags by name, value, or category
- **Browse Categories**: Expand/collapse category groups
- **Select Tag**: Click a tag to insert it at the cursor position

## Autocomplete (Type `{{`)

### Triggering Autocomplete

1. Position your cursor in a text field
2. Type `{{` (two curly braces)
3. An autocomplete menu appears

### Navigating Autocomplete

- **Arrow Down**: Move to next item
- **Arrow Up**: Move to previous item
- **Enter/Tab**: Insert selected tag
- **Escape**: Close menu
- **Type Letters**: Filter results in real-time

### Filtering

Type after `{{` to filter results:

- `{{fir` → Shows "First Name"
- `{{comp` → Shows "Company"
- `{{cont` → Shows contract-related tags

## Supported JSON Formats

BlazerEditor supports three different merge tag formats for maximum compatibility:

### Format 1: Grouped Dictionary (C# Style)

```json
{
  "Contract": [
    {
      "Name": "Contract From",
      "Value": "{{Contract_ContractFrom}}",
      "Sample": "2025-12-31"
    },
    {
      "Name": "Contract To",
      "Value": "{{Contract_ContractTo}}",
      "Sample": "2026-12-31"
    }
  ],
  "Person": [
    {
      "Name": "First Name",
      "Value": "{{Person_FirstName}}",
      "Sample": "John"
    }
  ]
}
```

### Format 2: Grouped Array (JavaScript Style)

```json
[
  {
    "name": "Contract",
    "mergeTags": [
      {
        "name": "Contract From",
        "value": "{{Contract_ContractFrom}}",
        "sample": "2025-12-31"
      }
    ]
  },
  {
    "name": "Person",
    "mergeTags": [
      {
        "name": "First Name",
        "value": "{{Person_FirstName}}",
        "sample": "John"
      }
    ]
  }
]
```

### Format 3: Flat Dictionary

```json
{
  "first_name": {
    "Name": "First Name",
    "Value": "{{first_name}}",
    "Sample": "John"
  },
  "last_name": {
    "Name": "Last Name",
    "Value": "{{last_name}}",
    "Sample": "Doe"
  }
}
```

## Advanced: Grouped Merge Tags

For complex scenarios with many tags, use the `MergeTagGroup` class:

```csharp
var groups = new List<MergeTagGroup>
{
    new MergeTagGroup
    {
        Name = "Contract",
        MergeTags = new List<MergeTag>
        {
            new MergeTag { Name = "Contract From", Value = "{{Contract_ContractFrom}}", Sample = "2025-12-31" },
            new MergeTag { Name = "Contract To", Value = "{{Contract_ContractTo}}", Sample = "2026-12-31" },
            new MergeTag { Name = "Move In Date", Value = "{{Contract_MoveInDate}}", Sample = "2025-01-15" }
        }
    },
    new MergeTagGroup
    {
        Name = "Person",
        MergeTags = new List<MergeTag>
        {
            new MergeTag { Name = "First Name", Value = "{{Person_FirstName}}", Sample = "John" },
            new MergeTag { Name = "Last Name", Value = "{{Person_LastName}}", Sample = "Doe" },
            new MergeTag { Name = "Email", Value = "{{Person_Email}}", Sample = "john.doe@example.com" }
        }
    }
};

// Flatten groups for editor
var mergeTags = groups.SelectMany(g => g.MergeTags).ToList();
```

## Preview Mode

Toggle preview mode to see sample values instead of placeholder text:

```csharp
var options = new EditorOptions
{
    EnableMergeTags = true,
    ShowMergeTagPreview = true  // Shows toggle in dropdown
};
```

## Example: Complete Setup

```csharp
using BlazerEditor.Components;
using BlazerEditor.Models;

// Create merge tags
var mergeTags = MergeTagDefaults.GetDefaultTags();

// Or create custom tags
var customTags = new List<MergeTag>
{
    new MergeTag { Name = "Full Name", Value = "{{full_name}}", Sample = "John Doe", Category = "Personal" },
    new MergeTag { Name = "Unsubscribe Link", Value = "{{unsubscribe_url}}", Sample = "https://example.com/unsub", Category = "Links" }
};

var options = new EditorOptions
{
    MinHeight = 600,
    EnableMergeTags = true,
    ShowMergeTagPreview = true,
    MergeTags = customTags
};
```

```razor
@page "/email-builder"

<EmailEditor @ref="editor" Options="options" />

@code {
    private EmailEditor? editor;
    
    // Initialize options in component
    protected override void OnInitialized()
    {
        base.OnInitialized();
    }
}
```

## Default Merge Tags

BlazerEditor includes default merge tags via `MergeTagDefaults.GetDefaultTags()`:

### Personal
- First Name
- Last Name
- Full Name
- Email Address

### Company
- Company Name
- Job Title

### Links
- Unsubscribe Link
- View Online Link
- Preferences Link

### System
- Current Date
- Current Year

## Troubleshooting

### Autocomplete Not Working?

1. Ensure JavaScript file is loaded:
   ```html
   <script src="_content/BlazerEditor/js/mergeTagAutocomplete.js"></script>
   ```

2. Check browser console for errors

3. Verify merge tags are passed to RichTextEditor:
   ```razor
   <RichTextEditor MergeTags="_mergeTags" />
   ```

### Tags Not Showing in Dropdown?

1. Ensure `EnableMergeTags = true` in EditorOptions
2. Verify merge tags list is not empty
3. Check that tags have proper Name and Value properties

### Autocomplete Menu Position Incorrect?

This is expected in some scenarios. The menu positions relative to the cursor.

## Best Practices

1. **Use Categories**: Organize tags by category for better UX
2. **Descriptive Names**: Use clear, human-readable names
3. **Consistent Value Format**: Use consistent placeholder syntax (e.g., `{{variable_name}}`)
4. **Sample Values**: Provide realistic sample values for preview mode
5. **Group Related Tags**: Use MergeTagGroup for related tags

## API Reference

### MergeTag Properties

- `Name` (string): Display name shown in UI
- `Value` (string): Placeholder text inserted into template
- `Sample` (string): Preview/example value
- `Category` (string): Grouping category
- `Description` (string, optional): Help text

### MergeTagGroup Properties

- `Name` (string): Group name
- `MergeTags` (List<MergeTag>): Array of tags in group

### EditorOptions Merge Tag Properties

- `EnableMergeTags` (bool): Enable merge tags feature
- `ShowMergeTagPreview` (bool): Show preview toggle
- `MergeTags` (List<MergeTag>?): List of available tags


