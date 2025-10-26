# NuGet Publishing Quick Start

## 🚀 Quick Publish (3 Steps)

### 1. Get Your NuGet API Key
- Go to https://www.nuget.org/account/apikeys
- Create new API key with "Push" permission
- Copy the key

### 2. Set API Key
```powershell
$env:NUGET_API_KEY = "your-api-key-here"
```

### 3. Run Publish Script
```powershell
cd BlazerEditor
.\publish.ps1
```

Done! Your package will be live on NuGet in 5-15 minutes.

---

## 📝 Before First Publish

Update these in `BlazerEditor.csproj`:

```xml
<Authors>Your Name</Authors>
<Company>Your Company</Company>
<RepositoryUrl>https://github.com/yourusername/blazereditor</RepositoryUrl>
```

---

## 🧪 Test Locally First

```powershell
# Build only (don't publish)
.\publish.ps1 -LocalOnly

# Test in another project
dotnet add package BlazerEditor --source ./bin/Release
```

---

## 📦 Manual Publish (Without Script)

```bash
# 1. Build
dotnet pack -c Release

# 2. Publish
dotnet nuget push bin/Release/BlazerEditor.1.0.0.nupkg --api-key YOUR_KEY --source https://api.nuget.org/v3/index.json
```

---

## 🔄 Update Version

In `BlazerEditor.csproj`:
```xml
<Version>1.0.1</Version>
<PackageReleaseNotes>Bug fixes and improvements</PackageReleaseNotes>
```

Then run `.\publish.ps1` again.

---

## ✅ Verify Publication

After publishing:
1. Visit: https://www.nuget.org/packages/BlazerEditor
2. Test install: `dotnet add package BlazerEditor`

---

## 📚 Full Documentation

See `PUBLISH.md` for complete details.
