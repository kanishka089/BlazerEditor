# Build Instructions

## Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022, VS Code, or JetBrains Rider (optional)
- Git

## Building from Source

### 1. Clone the Repository

```bash
git clone https://github.com/yourusername/blazereditor.git
cd blazereditor
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Build the Library

```bash
dotnet build BlazerEditor.csproj
```

### 4. Run Tests (when available)

```bash
dotnet test
```

### 5. Create NuGet Package

```bash
dotnet pack BlazerEditor.csproj -c Release
```

The package will be created in `bin/Release/BlazerEditor.1.0.0.nupkg`

## Building the Demo

### 1. Navigate to Demo Directory

```bash
cd Demo
```

### 2. Restore and Build

```bash
dotnet restore
dotnet build
```

### 3. Run the Demo

```bash
dotnet run
```

Or for hot reload:

```bash
dotnet watch run
```

The demo will be available at `https://localhost:5001`

## Development Workflow

### Watch Mode

For development with automatic rebuilds:

```bash
dotnet watch build
```

### Clean Build

To clean and rebuild:

```bash
dotnet clean
dotnet build
```

## Publishing

### Publish to NuGet

1. Update version in `BlazerEditor.csproj`
2. Build and pack:
   ```bash
   dotnet pack -c Release
   ```
3. Push to NuGet:
   ```bash
   dotnet nuget push bin/Release/BlazerEditor.1.0.0.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json
   ```

### Publish Demo to Azure/Netlify

#### Azure Static Web Apps

```bash
cd Demo
dotnet publish -c Release
```

Then deploy the `bin/Release/net8.0/publish/wwwroot` folder.

#### Netlify

```bash
cd Demo
dotnet publish -c Release -o publish
```

Deploy the `publish/wwwroot` folder to Netlify.

## Project Structure

```
BlazerEditor/
├── Components/           # Blazor components
│   ├── EmailEditor.razor
│   ├── EmailEditor.razor.cs
│   ├── EmailEditor.razor.css
│   └── PreviewModal.razor
├── Models/              # Data models
│   ├── EditorOptions.cs
│   ├── EmailDesign.cs
│   └── ExportResult.cs
├── Services/            # Business logic
│   ├── HtmlExportService.cs
│   ├── DesignService.cs
│   └── TemplateLibraryService.cs
├── docs/                # Documentation
├── Demo/                # Demo application
└── BlazerEditor.csproj  # Project file
```

## Configuration

### Target Framework

The library targets .NET 8.0. To change:

```xml
<TargetFramework>net8.0</TargetFramework>
```

### Dependencies

Minimal dependencies for maximum compatibility:
- Microsoft.AspNetCore.Components.Web
- System.Text.Json

## Troubleshooting

### Build Errors

**Error: SDK not found**
- Install .NET 8.0 SDK from https://dot.net

**Error: Package restore failed**
- Clear NuGet cache: `dotnet nuget locals all --clear`
- Restore again: `dotnet restore`

### Runtime Errors

**Error: Component not found**
- Ensure `@using BlazerEditor.Components` is in `_Imports.razor`
- Register services: `builder.Services.AddBlazerEditor()`

**Error: CSS not loading**
- Ensure CSS isolation is enabled
- Check browser console for 404 errors

## IDE Setup

### Visual Studio 2022

1. Open `BlazerEditor.sln`
2. Set Demo project as startup project
3. Press F5 to run

### VS Code

1. Install C# extension
2. Open folder in VS Code
3. Press F5 or use terminal: `dotnet run`

### JetBrains Rider

1. Open `BlazerEditor.sln`
2. Right-click Demo project → Run
3. Or use terminal: `dotnet run`

## Performance Optimization

### Release Build

Always use Release configuration for production:

```bash
dotnet build -c Release
dotnet publish -c Release
```

### AOT Compilation (Coming Soon)

For WebAssembly AOT:

```bash
dotnet publish -c Release -p:RunAOTCompilation=true
```

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md) for development guidelines.

## License

MIT License - see [LICENSE](LICENSE) for details.
