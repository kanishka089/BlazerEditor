# BlazerEditor Installation Guide

## Installation

```bash
dotnet add package BlazerEditor
```

## Setup

### 1. Register Services

In your `Program.cs` or `Startup.cs`:

```csharp
using BlazerEditor;

// In ConfigureServices or builder.Services
services.AddBlazerEditor();
```

### 2. Add CSS Reference

Choose ONE of the following options:

#### Option A: Scoped CSS Bundle (Recommended)
Add to your `_Host.cshtml` (Blazor Server) or `index.html` (Blazor WebAssembly):

```html
<link href="_content/BlazerEditor/BlazerEditor.bundle.scp.css" rel="stylesheet" />
```

#### Option B: Direct CSS File
Alternatively, use the direct CSS file:

```html
<link href="_content/BlazerEditor/css/blazereditor.css" rel="stylesheet" />
```

### 3. Add Using Statements

In your `_Imports.razor`:

```razor
@using BlazerEditor.Components
@using BlazerEditor.Models
```

### 4. Use the Component

```razor
@page "/editor"

<EmailEditor @ref="editor" Options="options" />

@code {
    private EmailEditor? editor;
    private EditorOptions options = new()
    {
        Appearance = new AppearanceConfig { Theme = "modern_light" },
        MinHeight = 600
    };
}
```

## Complete Example

See the `BlazerEditorTest` project for a complete working example.

## API Reference

### Export HTML
```csharp
var result = await editor.ExportHtmlAsync();
string html = result.Html;
```

### Save Design
```csharp
var design = await editor.SaveDesignAsync();
string json = JsonSerializer.Serialize(design);
```

### Load Design
```csharp
await editor.LoadDesignAsync(design);
// or
await editor.LoadDesignAsync(jsonString);
```

## Troubleshooting

### Styles not loading?
Make sure you've added the CSS reference to your `_Host.cshtml` or `index.html` file.

### Components not found?
Add the using statements to your `_Imports.razor` file.
