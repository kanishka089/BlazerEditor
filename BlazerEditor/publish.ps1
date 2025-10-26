# BlazerEditor NuGet Publish Script

param(
    [string]$ApiKey = $env:NUGET_API_KEY,
    [switch]$LocalOnly
)

Write-Host "🚀 BlazerEditor NuGet Publisher" -ForegroundColor Cyan
Write-Host "================================" -ForegroundColor Cyan
Write-Host ""

# Clean previous builds
Write-Host "🧹 Cleaning previous builds..." -ForegroundColor Yellow
Remove-Item -Path "bin/Release" -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item -Path "obj/Release" -Recurse -Force -ErrorAction SilentlyContinue

# Build the package
Write-Host "📦 Building package..." -ForegroundColor Yellow
dotnet pack -c Release

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Build failed!" -ForegroundColor Red
    exit 1
}

# Get version from csproj
[xml]$csproj = Get-Content "BlazerEditor.csproj"
$version = $csproj.Project.PropertyGroup.Version

$packagePath = "bin/Release/BlazerEditor.$version.nupkg"

if (-not (Test-Path $packagePath)) {
    Write-Host "❌ Package file not found: $packagePath" -ForegroundColor Red
    exit 1
}

Write-Host "✅ Package built successfully: BlazerEditor.$version.nupkg" -ForegroundColor Green
Write-Host ""

# Show package contents
Write-Host "📋 Package contents:" -ForegroundColor Yellow
dotnet nuget list source

if ($LocalOnly) {
    Write-Host "📁 Local build only - skipping publish" -ForegroundColor Yellow
    Write-Host "Package location: $packagePath" -ForegroundColor Cyan
    exit 0
}

# Publish to NuGet
if ([string]::IsNullOrEmpty($ApiKey)) {
    Write-Host "⚠️  No API key provided!" -ForegroundColor Yellow
    Write-Host "Set environment variable: `$env:NUGET_API_KEY = 'your-key'" -ForegroundColor Yellow
    Write-Host "Or run: .\publish.ps1 -ApiKey 'your-key'" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Package built at: $packagePath" -ForegroundColor Cyan
    Write-Host "To publish manually:" -ForegroundColor Cyan
    Write-Host "dotnet nuget push `"$packagePath`" --api-key YOUR_KEY --source https://api.nuget.org/v3/index.json" -ForegroundColor Gray
    exit 0
}

Write-Host "🚀 Publishing to NuGet.org..." -ForegroundColor Yellow
dotnet nuget push $packagePath --api-key $ApiKey --source https://api.nuget.org/v3/index.json

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "✅ Successfully published BlazerEditor $version to NuGet!" -ForegroundColor Green
    Write-Host "🔗 View at: https://www.nuget.org/packages/BlazerEditor/$version" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "⏳ Note: It may take 5-15 minutes to appear in search results" -ForegroundColor Yellow
} else {
    Write-Host ""
    Write-Host "❌ Publish failed!" -ForegroundColor Red
    Write-Host "Check the error message above for details" -ForegroundColor Yellow
}
