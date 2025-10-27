# BlazerEditor - Email Template Editor for Blazor

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/)
[![Blazor](https://img.shields.io/badge/Blazor-WebAssembly-blueviolet.svg)](https://blazor.net/)
[![NuGet](https://img.shields.io/nuget/v/BlazerEditor.svg)](https://www.nuget.org/packages/BlazerEditor/)

**A powerful, free, and open-source drag-and-drop email template editor for Blazor.**

Build beautiful responsive email templates with an intuitive visual editor. No CDN dependencies, 100% free, MIT licensed.

## ⚡ Quick Start (3 Steps)

### 1️⃣ Install
```bash
dotnet add package BlazerEditor
```

### 2️⃣ Setup (Program.cs)
```csharp
builder.Services.AddBlazerEditor();
builder.Services.AddRadzenComponents();
```

Add to `_Host.cshtml` or `index.html`:
```html
<link rel="stylesheet" href="_content/Radzen.Blazor/css/material-base.css" />
<link href="_content/BlazerEditor/css/blazereditor.css" rel="stylesheet" />
<script src="_content/Radzen.Blazor/Radzen.Blazor.js"></script>
<script src="_content/BlazerEditor/js/mergeTagAutocomplete.js"></script>
```

### 3️⃣ Use
```razor
@page "/editor"
@using BlazerEditor
@inject MergeTagService MergeTagService

<EmailEditor @ref="editor" MergeTags="mergeTags" />
<button @onclick="Export">Export HTML</button>

@code {
    EmailEditor? editor;
    List<MergeTag> mergeTags = new();

    protected override void OnInitialized() => 
        mergeTags = MergeTagService.GetDefaultMergeTags();

    async Task Export() {
        var result = await editor!.ExportHtmlAsync();
        Console.WriteLine(result.Html); // Use the HTML!
    }
}
```

**That's it!** 🎉 You now have a fully functional email editor with merge tags.

---

## 🏷️ Using Merge Tags (Super Simple!)

Merge tags let you insert personalized data into emails (like `{{first_name}}`, `{{company}}`).

### Two Ways to Insert Merge Tags:

**1. Click the "Merge Tags 🏷️" button** → Browse categories → Click a tag
```
Personal: {{first_name}}, {{last_name}}, {{email}}
Company: {{company}}, {{job_title}}
Links: {{unsubscribe_url}}, {{view_online}}
```

**2. Type `{{` anywhere** → Autocomplete menu appears → Select tag
```
Type: {{fir  →  Shows "First Name"
Type: {{com  →  Shows "Company Name"
```

### Custom Merge Tags:
```csharp
mergeTags = new List<MergeTag> {
    new() { 
        Name = "Customer Name",
        Value = "{{customer_name}}",
        Category = "Personal",
        Sample = "John Doe"
    }
};
```

### Replace Tags with Real Data:
```csharp
var html = result.Html
    .Replace("{{first_name}}", "John")
    .Replace("{{company}}", "Acme Corp");
// Send personalized email!
```

That's all you need to know about merge tags! Simple and powerful. 🎉

---

## Features

- 🎨 **Drag & Drop Interface** - Intuitive visual email builder with real-time preview
- 📱 **Responsive Design** - Mobile and desktop preview modes
- 🎯 **Rich Components** - Text, images, buttons, headings, dividers, columns, and more
- 🏷️ **Merge Tags System** - Powerful merge tag support with:
  - Dropdown picker with categorized tags
  - Autocomplete (type `{{` to trigger)
  - Custom tag categories
  - Sample value preview
  - Search and filter functionality
- 💾 **Export/Import** - Save designs as JSON, export as production-ready HTML
- 🎨 **Customizable Themes** - Light and dark mode support
- ↩️ **Undo/Redo** - Full history management
- 🔧 **Extensible** - Add custom components and tools
- 📦 **Self-Contained** - No external CDN dependencies, works offline
- 🆓 **Free & Open Source** - MIT Licensed, no restrictions

## 📚 Complete Example

```razor
@page "/email-builder"
@using BlazerEditor
@inject MergeTagService MergeTagService

<h3>Email Template Builder</h3>

<EmailEditor @ref="editor" 
             MergeTags="mergeTags"
             Options="options" />

<div class="actions">
    <button @onclick="ExportHtml">📤 Export HTML</button>
    <button @onclick="SaveDesign">💾 Save Design</button>
    <button @onclick="LoadDesign">📂 Load Design</button>
</div>

@code {
    EmailEditor? editor;
    List<MergeTag> mergeTags = new();
    EditorOptions options = new() {
        Appearance = new() { Theme = "modern_light" },
        DisplayMode = DisplayMode.Email
    };

    protected override void OnInitialized() {
        mergeTags = MergeTagService.GetDefaultMergeTags();
    }

    async Task ExportHtml() {
        var result = await editor!.ExportHtmlAsync();
        
        // result.Html - Ready-to-send HTML email
        // result.DesignJson - Save this to database for later editing
        
        await SendEmail(result.Html);
        await SaveToDatabase(result.DesignJson);
    }

    async Task SaveDesign() {
        // Returns JSON string - save to database
        var designJson = await editor!.SaveDesignAsync();
        await SaveToDatabase(designJson);
    }

    async Task LoadDesign() {
        // Load JSON from database
        var designJson = await LoadFromDatabase();
        await editor!.LoadDesignAsync(designJson);
    }

    async Task SendEmail(string html) {
        // Your email sending logic
    }

    async Task SaveToDatabase(string json) {
        // Your database save logic
    }

    async Task<string> LoadFromDatabase() {
        // Your database load logic
        return "{}";
    }
}
```

### 💾 Save & Load Example

```csharp
// Save design to database
var designJson = await editor.SaveDesignAsync();
await _dbContext.EmailTemplates.AddAsync(new EmailTemplate {
    Name = "Welcome Email",
    DesignJson = designJson,
    CreatedAt = DateTime.UtcNow
});
await _dbContext.SaveChangesAsync();

// Load design from database
var template = await _dbContext.EmailTemplates.FindAsync(templateId);
await editor.LoadDesignAsync(template.DesignJson);

// Export HTML for sending
var result = await editor.ExportHtmlAsync();
await _emailService.SendAsync(recipient, subject, result.Html);
```

**That's it!** You're ready to build beautiful email templates. 🚀

## Need Help?

- 💬 [GitHub Discussions](https://github.com/kanishka089/BlazerEditor/discussions) - Ask questions
- 🐛 [Issue Tracker](https://github.com/kanishka089/BlazerEditor/issues) - Report bugs
- ⭐ [Star on GitHub](https://github.com/kanishka089/BlazerEditor) - Show your support

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



## Contributing

We welcome contributions! Whether it's:
- 🐛 Bug reports
- 💡 Feature requests
- 📝 Documentation improvements
- 🔧 Code contributions

Open an issue or pull request on [GitHub](https://github.com/kanishka089/BlazerEditor)!

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



## License

MIT License - Free for personal and commercial use.

Copyright (c) 2024 BlazerEditor Contributors

## Acknowledgments

Inspired by [Unlayer's React Email Editor](https://github.com/unlayer/react-email-editor). Built with ❤️ for the Blazor community.

---

**Star ⭐ this repo if you find it useful!**
