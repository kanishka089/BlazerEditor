# Publishing BlazerEditor to NuGet

## Prerequisites

1. **NuGet Account**: Create an account at https://www.nuget.org/
2. **API Key**: Get your API key from https://www.nuget.org/account/apikeys

## Step 1: Update Package Information

Edit `BlazerEditor.csproj` and update:
- `<Authors>` - Your name
- `<Company>` - Your company name
- `<Version>` - Current version (e.g., 1.0.0)
- `<RepositoryUrl>` - Your GitHub repository URL
- `<PackageProjectUrl>` - Your project website or GitHub URL

## Step 2: Add Package Icon (Optional but Recommended)

1. Create a 128x128 PNG icon
2. Save it as `BlazerEditor/icon.png`
3. It's already configured in the .csproj file

## Step 3: Build the Package

Open terminal in the `BlazerEditor` folder and run:

```bash
dotnet pack -c Release
```

This creates a `.nupkg` file in `bin/Release/`

## Step 4: Test the Package Locally (Recommended)

Before publishing, test it locally:

```bash
# Create a local NuGet source
mkdir C:\LocalNuGet

# Copy the package
copy bin\Release\BlazerEditor.1.0.0.nupkg C:\LocalNuGet\

# Add local source
dotnet nuget add source C:\LocalNuGet -n LocalNuGet

# Test in another project
dotnet add package BlazerEditor --source LocalNuGet
```

## Step 5: Publish to NuGet

### Option A: Using Command Line

```bash
dotnet nuget push bin/Release/BlazerEditor.1.0.0.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json
```

### Option B: Using NuGet.org Website

1. Go to https://www.nuget.org/packages/manage/upload
2. Upload the `.nupkg` file from `bin/Release/`
3. Fill in any additional information
4. Click "Submit"

## Step 6: Verify Publication

After publishing (takes 5-15 minutes to index):
1. Visit https://www.nuget.org/packages/BlazerEditor
2. Test installation: `dotnet add package BlazerEditor`

## Version Updates

When releasing new versions:

1. Update `<Version>` in `.csproj` (e.g., 1.0.0 → 1.0.1)
2. Update `<PackageReleaseNotes>` with changes
3. Build and publish again

### Version Guidelines:
- **Major** (1.0.0 → 2.0.0): Breaking changes
- **Minor** (1.0.0 → 1.1.0): New features, backward compatible
- **Patch** (1.0.0 → 1.0.1): Bug fixes

## Quick Publish Script

Create `publish.ps1` in BlazerEditor folder:

```powershell
# Build
dotnet pack -c Release

# Get version from csproj
$version = Select-Xml -Path "BlazerEditor.csproj" -XPath "//Version" | Select-Object -ExpandProperty Node | Select-Object -ExpandProperty InnerText

# Publish
dotnet nuget push "bin/Release/BlazerEditor.$version.nupkg" --api-key $env:NUGET_API_KEY --source https://api.nuget.org/v3/index.json

Write-Host "Published BlazerEditor version $version to NuGet!"
```

Set your API key as environment variable:
```powershell
$env:NUGET_API_KEY = "your-api-key-here"
```

Then run:
```powershell
.\publish.ps1
```

## Troubleshooting

### Package already exists
- You cannot overwrite a published version
- Increment the version number and republish

### Missing dependencies
- Make sure all PackageReferences are correct in .csproj
- Dependencies are automatically included in the package

### CSS not loading
- Verify `.razor.css` files are in the same folder as `.razor` files
- Check that consumers added the CSS reference to their HTML

## Best Practices

1. ✅ Test locally before publishing
2. ✅ Use semantic versioning
3. ✅ Update README.md with each release
4. ✅ Add release notes
5. ✅ Tag releases in Git
6. ✅ Keep a CHANGELOG.md

## Resources

- NuGet Documentation: https://docs.microsoft.com/en-us/nuget/
- Semantic Versioning: https://semver.org/
- Package Icon Guidelines: https://docs.microsoft.com/en-us/nuget/reference/nuspec#icon
