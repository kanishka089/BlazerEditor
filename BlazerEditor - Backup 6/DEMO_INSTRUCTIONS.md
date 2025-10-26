# How to Run the BlazerEditor Demo

Due to the complexity of setting up a Blazor Server demo with the current project structure, here's the simplest way to see BlazerEditor in action:

## Option 1: Create a New Blazor Server App (Recommended)

1. **Create a new Blazor Server app:**
   ```bash
   dotnet new blazorserver -o BlazerEditorTest
   cd BlazerEditorTest
   ```

2. **Add project reference:**
   ```bash
   dotnet add reference ../BlazerEditor/BlazerEditor.csproj
   ```

3. **Register services in `Program.cs`:**
   ```csharp
   using BlazerEditor;
   
   // Add this line after builder.Services.AddServerSideBlazor();
   builder.Services.AddBlazerEditor();
   ```

4. **Add imports to `_Imports.razor`:**
   ```razor
   @using BlazerEditor
   @using BlazerEditor.Components
   @using BlazerEditor.Models
   ```

5. **Replace `Pages/Index.razor` content with:**
   ```razor
   @page "/"
   @inject IJSRuntime JS

   <PageTitle>BlazerEditor Demo</PageTitle>

   <div style="display: flex; flex-direction: column; height: 100vh;">
       <div style="padding: 20px; background: linear-gradient(135deg, #007aff 0%, #0051d5 100%); color: white;">
           <h1>🎨 BlazerEditor Demo</h1>
           <div style="display: flex; gap: 10px; margin-top: 10px;">
               <button @onclick="ExportHtml" style="padding: 10px 20px; background: rgba(255,255,255,0.2); border: 1px solid rgba(255,255,255,0.3); color: white; border-radius: 6px; cursor: pointer;">
                   Export HTML
               </button>
               <button @onclick="SaveDesign" style="padding: 10px 20px; background: rgba(255,255,255,0.2); border: 1px solid rgba(255,255,255,0.3); color: white; border-radius: 6px; cursor: pointer;">
                   Save Design
               </button>
           </div>
       </div>
       
       <div style="flex: 1;">
           <EmailEditor @ref="editor" Options="options" />
       </div>
   </div>

   @code {
       private EmailEditor? editor;
       private EditorOptions options = new()
       {
           Appearance = new AppearanceConfig { Theme = "modern_light" },
           MinHeight = 600
       };

       private async Task ExportHtml()
       {
           if (editor == null) return;
           var result = await editor.ExportHtmlAsync();
           Console.WriteLine("Exported HTML:");
           Console.WriteLine(result.Html);
           await JS.InvokeVoidAsync("alert", "HTML exported! Check browser console (F12)");
       }

       private async Task SaveDesign()
       {
           if (editor == null) return;
           var design = await editor.SaveDesignAsync();
           var json = System.Text.Json.JsonSerializer.Serialize(design, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
           Console.WriteLine("Saved Design:");
           Console.WriteLine(json);
           await JS.InvokeVoidAsync("alert", "Design saved! Check browser console (F12)");
       }
   }
   ```

6. **Run the app:**
   ```bash
   dotnet run
   ```

7. **Open browser:**
   - Navigate to `https://localhost:5001` or the URL shown in terminal

## Option 2: Quick Test Script

Save this as `test-blazereditor.ps1` in the BlazerEditor folder:

```powershell
Write-Host "Creating test Blazor app..." -ForegroundColor Cyan
dotnet new blazorserver -o ../BlazerEditorTest -f net8.0

Write-Host "Adding BlazerEditor reference..." -ForegroundColor Cyan
cd ../BlazerEditorTest
dotnet add reference ../BlazerEditor/BlazerEditor.csproj

Write-Host "Setup complete! Now:" -ForegroundColor Green
Write-Host "1. Add 'builder.Services.AddBlazerEditor();' to Program.cs" -ForegroundColor Yellow
Write-Host "2. Add BlazerEditor usings to _Imports.razor" -ForegroundColor Yellow
Write-Host "3. Use <EmailEditor /> component in your pages" -ForegroundColor Yellow
Write-Host "4. Run 'dotnet run'" -ForegroundColor Yellow
```

## What You'll See

Once running, you'll see:
- ✅ Drag-and-drop email editor
- ✅ Component library (Text, Heading, Image, Button, Divider)
- ✅ Property editor
- ✅ Undo/redo buttons
- ✅ Desktop/mobile preview toggle
- ✅ Export and save functionality

## Troubleshooting

### Build Errors
- Make sure .NET 8.0 SDK is installed: `dotnet --version`
- Clean and rebuild: `dotnet clean && dotnet build`

### Editor Not Showing
- Check browser console (F12) for errors
- Verify services are registered in Program.cs
- Check _Imports.razor has BlazerEditor usings

### CSS Not Loading
- Hard refresh browser (Ctrl+Shift+R)
- Check that CSS isolation is working

## Next Steps

After seeing the demo:
1. Read the [API Documentation](docs/API.md)
2. Check out [Examples](docs/Examples.md)
3. Customize the editor for your needs
4. Integrate into your application

---

**Note**: The Demo folder in this project has some configuration issues. The instructions above provide the simplest way to test BlazerEditor.
