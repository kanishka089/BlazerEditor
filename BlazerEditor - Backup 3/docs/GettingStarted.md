# Getting Started with BlazerEditor

## Installation

### NuGet Package

```bash
dotnet add package BlazerEditor
```

### From Source

```bash
git clone https://github.com/yourusername/blazereditor.git
cd blazereditor
dotnet build
```

## Setup

### 1. Register Services

In your `Program.cs`:

```csharp
using BlazerEditor;

builder.Services.AddBlazerEditor();
```

### 2. Add Imports

In your `_Imports.razor`:

```razor
@using BlazerEditor
@using BlazerEditor.Components
@using BlazerEditor.Models
```

### 3. Use the Component

```razor
@page "/email-editor"

<EmailEditor @ref="editorRef" 
             Options="editorOptions"
             OnReady="OnEditorReady" />

@code {
    private EmailEditor? editorRef;
    private EditorOptions editorOptions = new()
    {
        Appearance = new AppearanceConfig { Theme = "modern_light" },
        MinHeight = 600
    };

    private void OnEditorReady()
    {
        Console.WriteLine("Editor ready!");
    }
}
```

## Basic Operations

### Export HTML

```csharp
private async Task ExportHtml()
{
    var result = await editorRef.ExportHtmlAsync();
    string html = result.Html;
    EmailDesign design = result.Design;
    
    // Save to database, send email, etc.
}
```

### Save Design

```csharp
private async Task SaveDesign()
{
    var design = await editorRef.SaveDesignAsync();
    var json = JsonSerializer.Serialize(design);
    
    // Save JSON to database
    await SaveToDatabase(json);
}
```

### Load Design

```csharp
private async Task LoadDesign()
{
    var json = await LoadFromDatabase();
    await editorRef.LoadDesignAsync(json);
}
```

## Configuration Options

```csharp
var options = new EditorOptions
{
    // Appearance
    Appearance = new AppearanceConfig 
    { 
        Theme = "modern_light" // modern_light, modern_dark, classic
    },
    
    // Display mode
    DisplayMode = DisplayMode.Email, // Email or Web
    
    // Localization
    Locale = "en-US",
    
    // Editor height
    MinHeight = 600,
    
    // Features
    Features = new FeaturesConfig
    {
        TextEditor = true,
        ImageEditor = true,
        UndoRedo = true,
        Preview = true,
        Export = true,
        Import = true
    },
    
    // Custom merge tags
    MergeTags = new List<MergeTag>
    {
        new() { Name = "First Name", Value = "{{firstName}}", Sample = "John" },
        new() { Name = "Last Name", Value = "{{lastName}}", Sample = "Doe" }
    }
};
```

## Next Steps

- [Component Reference](./ComponentReference.md)
- [API Documentation](./API.md)
- [Examples](./Examples.md)
- [Customization](./Customization.md)
