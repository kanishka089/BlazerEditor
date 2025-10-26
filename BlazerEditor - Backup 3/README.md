# BlazerEditor - Email Template Editor for Blazor

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/)
[![Blazor](https://img.shields.io/badge/Blazor-WebAssembly-blueviolet.svg)](https://blazor.net/)
[![NuGet](https://img.shields.io/badge/NuGet-1.0.0-blue.svg)](https://www.nuget.org/)

A powerful, free, and open-source drag-and-drop email template editor for Blazor applications. Inspired by Unlayer, but completely self-contained with no CDN dependencies.

## Features

- 🎨 **Drag & Drop Interface** - Intuitive visual email builder
- 📱 **Responsive Design** - Mobile and desktop preview modes
- 🎯 **Rich Components** - Text, images, buttons, dividers, columns, and more
- 💾 **Export/Import** - Save designs as JSON, export as HTML
- 🎨 **Customizable Themes** - Light and dark mode support
- 🔧 **Extensible** - Add custom components and tools
- 📦 **Self-Contained** - No external CDN dependencies
- 🆓 **Free & Open Source** - MIT Licensed

## Installation

```bash
dotnet add package BlazerEditor
```

## Quick Start

```razor
@using BlazerEditor

<EmailEditor @ref="editorRef" 
             OnReady="OnEditorReady"
             Options="editorOptions" />

@code {
    private EmailEditor editorRef;
    private EditorOptions editorOptions = new()
    {
        Appearance = new AppearanceConfig { Theme = "modern_light" },
        DisplayMode = DisplayMode.Email
    };

    private void OnEditorReady()
    {
        Console.WriteLine("Editor is ready!");
    }

    private async Task ExportHtml()
    {
        var result = await editorRef.ExportHtmlAsync();
        Console.WriteLine(result.Html);
    }

    private async Task SaveDesign()
    {
        var design = await editorRef.SaveDesignAsync();
        // Save design JSON to database
    }

    private async Task LoadDesign(string designJson)
    {
        await editorRef.LoadDesignAsync(designJson);
    }
}
```

## Documentation

- 📚 [Getting Started](docs/GettingStarted.md)
- 📖 [API Reference](docs/API.md)
- 💡 [Examples](docs/Examples.md)
- ✨ [Features](docs/Features.md)
- 🔨 [Build Instructions](BUILD.md)
- 🤝 [Contributing](CONTRIBUTING.md)
- 📊 [Unlayer Comparison](docs/UnlayerComparison.md)

## Why BlazerEditor?

✅ **100% Free** - No pricing tiers, no hidden costs
✅ **Open Source** - MIT License, modify as you need
✅ **Self-Contained** - No CDN dependencies, works offline
✅ **Privacy First** - Your data stays on your infrastructure
✅ **Blazor Native** - Built specifically for .NET developers
✅ **Fully Customizable** - Complete source code access
✅ **No Vendor Lock-in** - Own your email editor

## Comparison with Unlayer

| Feature | Unlayer | BlazerEditor |
|---------|---------|--------------|
| Platform | React | Blazor |
| Pricing | Freemium | Always Free |
| Source | Closed | Open Source |
| CDN Required | Yes | No |
| Offline Support | No | Yes |
| Customization | Limited | Unlimited |

See [detailed comparison](docs/UnlayerComparison.md) for more information.

## Contributing

We welcome contributions! Whether it's:
- 🐛 Bug reports
- 💡 Feature requests
- 📝 Documentation improvements
- 🔧 Code contributions

See [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

## Roadmap

- [x] Core editor functionality
- [x] Basic components (text, image, button, etc.)
- [x] HTML export
- [x] Undo/redo
- [ ] More components (social, video, spacer)
- [ ] Image upload integration
- [ ] Template marketplace
- [ ] Custom fonts
- [ ] Collaboration features

## Support

- 💬 [GitHub Discussions](https://github.com/yourusername/blazereditor/discussions)
- 🐛 [Issue Tracker](https://github.com/yourusername/blazereditor/issues)
- 📧 Email: support@blazereditor.dev

## License

MIT License - Free for personal and commercial use.

Copyright (c) 2024 BlazerEditor Contributors

## Acknowledgments

Inspired by [Unlayer's React Email Editor](https://github.com/unlayer/react-email-editor). Built with ❤️ for the Blazor community.

---

**Star ⭐ this repo if you find it useful!**
