# BlazerEditor - Project Summary

## Overview

**BlazerEditor** is a free, open-source, drag-and-drop email template editor for Blazor applications. Inspired by Unlayer's React Email Editor, BlazerEditor provides similar functionality but is completely self-contained with no CDN dependencies, making it perfect for offline use and full control over your email template creation.

## Key Differences from Unlayer

| Feature | Unlayer (React) | BlazerEditor (Blazor) |
|---------|----------------|----------------------|
| **Platform** | React.js | Blazor (C#/.NET) |
| **Dependencies** | CDN-hosted editor | Self-contained |
| **Pricing** | Freemium (paid features) | 100% Free & Open Source |
| **Offline Support** | No (requires CDN) | Yes (fully offline) |
| **Customization** | Limited | Full source code access |
| **License** | Proprietary | MIT License |
| **Bundle Size** | Small wrapper (~10KB) | ~100KB (includes editor) |
| **Data Control** | Hosted service | Your infrastructure |

## Architecture

### Component Structure

```
EmailEditor (Main Component)
├── Toolbar (Undo/Redo, Preview, Actions)
├── Left Panel (Component Library)
├── Canvas (Email Preview)
└── Right Panel (Properties Editor)
```

### Data Flow

```
User Action → Component State → EmailDesign Model → JSON/HTML Export
```

### Key Design Patterns

1. **Component-Based Architecture** - Modular, reusable components
2. **State Management** - Centralized design state with undo/redo
3. **Service Layer** - Separation of concerns (export, design management)
4. **Event-Driven** - Callbacks for key operations
5. **Imperative API** - Ref-based API for programmatic control

## Core Components

### EmailEditor.razor
Main editor component with drag-and-drop interface, toolbar, and canvas.

**Key Features:**
- Drag-and-drop component placement
- Multi-column layout support
- Real-time preview
- Undo/redo with 50-level history
- Desktop/mobile preview modes

### Models
- **EmailDesign** - Complete email structure
- **EditorOptions** - Configuration options
- **ExportResult** - HTML export result

### Services
- **HtmlExportService** - Converts design to HTML
- **DesignService** - Design management and serialization
- **TemplateLibraryService** - Pre-built templates

## Features Implemented

✅ **Core Editor**
- Drag-and-drop interface
- Component library (Text, Heading, Image, Button, Divider)
- Multi-column layouts
- Row and column management
- Real-time preview

✅ **Editing**
- Property editor
- Undo/redo
- Copy/duplicate
- Delete operations

✅ **Export**
- HTML export with email-compatible markup
- JSON design export
- Inline CSS styles
- Mobile-responsive output

✅ **Theming**
- Modern Light theme
- Modern Dark theme
- Customizable appearance

✅ **Developer Experience**
- Comprehensive API
- TypeScript-like type safety (C#)
- Event callbacks
- Extensive documentation

## Technical Stack

- **Framework**: Blazor (.NET 8.0)
- **Language**: C# 12
- **UI**: Razor Components + CSS
- **Serialization**: System.Text.Json
- **Package**: NuGet

## File Structure

```
BlazerEditor/
├── Components/
│   ├── EmailEditor.razor          # Main editor component
│   ├── EmailEditor.razor.cs       # Component logic
│   ├── EmailEditor.razor.css      # Component styles
│   └── PreviewModal.razor         # Preview modal
├── Models/
│   ├── EditorOptions.cs           # Configuration
│   ├── EmailDesign.cs             # Design structure
│   └── ExportResult.cs            # Export result
├── Services/
│   ├── HtmlExportService.cs       # HTML generation
│   ├── DesignService.cs           # Design management
│   └── TemplateLibraryService.cs  # Template library
├── Demo/                          # Demo application
│   ├── Pages/
│   │   └── Index.razor            # Demo page
│   └── wwwroot/
│       └── index.html             # HTML shell
├── docs/                          # Documentation
│   ├── GettingStarted.md
│   ├── API.md
│   ├── Examples.md
│   └── Features.md
├── BlazerEditor.csproj            # Project file
├── README.md                      # Main readme
├── LICENSE                        # MIT License
└── CHANGELOG.md                   # Version history
```

## Usage Example

```razor
@page "/editor"
@inject IJSRuntime JS

<EmailEditor @ref="editor" 
             Options="options"
             OnReady="OnReady" />

<button @onclick="Export">Export HTML</button>

@code {
    private EmailEditor? editor;
    private EditorOptions options = new()
    {
        Appearance = new AppearanceConfig { Theme = "modern_light" },
        MinHeight = 600
    };

    private void OnReady()
    {
        Console.WriteLine("Editor ready!");
    }

    private async Task Export()
    {
        var result = await editor!.ExportHtmlAsync();
        await JS.InvokeVoidAsync("console.log", result.Html);
    }
}
```

## API Surface

### Public Methods
- `ExportHtmlAsync()` - Export to HTML
- `SaveDesignAsync()` - Save design as JSON
- `LoadDesignAsync(string json)` - Load design from JSON
- `LoadDesignAsync(EmailDesign design)` - Load design object

### Events
- `OnReady` - Editor initialized
- `OnExport` - Export completed
- `OnSave` - Save completed

### Parameters
- `Options` - Editor configuration
- `ShowExportButton` - Show/hide export button
- `ShowSaveButton` - Show/hide save button

## Comparison with Unlayer Implementation

### Similarities
✅ Component-based architecture
✅ Drag-and-drop interface
✅ JSON-based design storage
✅ HTML export functionality
✅ Undo/redo support
✅ Preview modes
✅ Property editor

### Differences
🔄 **Platform**: Blazor instead of React
🔄 **Language**: C# instead of JavaScript
🔄 **Hosting**: Self-contained instead of CDN
🔄 **Licensing**: MIT instead of proprietary
🔄 **Customization**: Full source access
🔄 **Integration**: .NET ecosystem

## Performance Characteristics

- **Initial Load**: ~100KB (self-contained)
- **Runtime**: Blazor WebAssembly or Server
- **Memory**: Efficient state management
- **Rendering**: Fast component updates
- **Export**: Instant HTML generation

## Browser Support

- ✅ Chrome/Edge (Chromium)
- ✅ Firefox
- ✅ Safari
- ✅ Mobile browsers

## Future Roadmap

### Phase 1 (Current)
- ✅ Core editor functionality
- ✅ Basic components
- ✅ HTML export
- ✅ Undo/redo

### Phase 2 (Next)
- 🔲 More components (social, video, spacer)
- 🔲 Custom fonts
- 🔲 Image upload
- 🔲 Template marketplace

### Phase 3 (Future)
- 🔲 Collaboration features
- 🔲 Version history
- 🔲 A/B testing
- 🔲 Analytics integration

## Installation

```bash
dotnet add package BlazerEditor
```

## Quick Start

1. Install package
2. Register services: `builder.Services.AddBlazerEditor()`
3. Add component: `<EmailEditor @ref="editor" />`
4. Use API: `await editor.ExportHtmlAsync()`

## Documentation

- [Getting Started](docs/GettingStarted.md)
- [API Reference](docs/API.md)
- [Examples](docs/Examples.md)
- [Features](docs/Features.md)
- [Build Instructions](BUILD.md)
- [Contributing](CONTRIBUTING.md)

## License

MIT License - Free for personal and commercial use.

## Contributing

Contributions welcome! See [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

## Support

- GitHub Issues: Report bugs and request features
- Discussions: Ask questions and share ideas
- Documentation: Comprehensive guides and examples

## Acknowledgments

Inspired by [Unlayer's React Email Editor](https://github.com/unlayer/react-email-editor), BlazerEditor brings similar functionality to the Blazor ecosystem with a focus on being free, open-source, and self-contained.

---

**Built with ❤️ for the Blazor community**
