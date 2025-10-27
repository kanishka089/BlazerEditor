# Publish BlazerEditor to NuGet.org
# Usage: .\publish-to-nuget.ps1 -ApiKey YOUR_API_KEY

param(
    [Parameter(Mandatory=$true)]
    [string]$ApiKey,
    
    [Parameter(Mandatory=$false)]
    [string]$Source = "https://api.nuget.org/v3/index.json"
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Publishing BlazerEditor to NuGet.org" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Check if package exists
$packagePath = ".\nupkg\BlazerEditor.1.0.4.nupkg"
if (-not (Test-Path $packagePath)) {
    Write-Host "ERROR: Package not found at $packagePath" -ForegroundColor Red
    Write-Host "Please run 'dotnet pack -c Release -o ./nupkg' first" -ForegroundColor Yellow
    exit 1
}

Write-Host "Package found: $packagePath" -ForegroundColor Green
Write-Host ""

# Confirm before publishing
Write-Host "You are about to publish BlazerEditor v1.0.4 to NuGet.org" -ForegroundColor Yellow
Write-Host "This action cannot be undone!" -ForegroundColor Yellow
Write-Host ""
$confirm = Read-Host "Are you sure you want to continue? (yes/no)"

if ($confirm -ne "yes") {
    Write-Host "Publishing cancelled." -ForegroundColor Yellow
    exit 0
}

Write-Host ""
Write-Host "Publishing package..." -ForegroundColor Cyan

# Push the package
try {
    dotnet nuget push $packagePath --api-key $ApiKey --source $Source
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "========================================" -ForegroundColor Green
        Write-Host "SUCCESS! Package published to NuGet.org" -ForegroundColor Green
        Write-Host "========================================" -ForegroundColor Green
        Write-Host ""
        Write-Host "Your package will be available at:" -ForegroundColor Cyan
        Write-Host "https://www.nuget.org/packages/BlazerEditor/" -ForegroundColor Cyan
        Write-Host ""
        Write-Host "It may take a few minutes to appear in search results." -ForegroundColor Yellow
        Write-Host ""
        Write-Host "Next steps:" -ForegroundColor Cyan
        Write-Host "1. Verify package on NuGet.org" -ForegroundColor White
        Write-Host "2. Create GitHub release (git tag v1.0.0)" -ForegroundColor White
        Write-Host "3. Announce on social media" -ForegroundColor White
        Write-Host "4. Update README with NuGet badge" -ForegroundColor White
    } else {
        Write-Host ""
        Write-Host "ERROR: Failed to publish package" -ForegroundColor Red
        Write-Host "Exit code: $LASTEXITCODE" -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Host ""
    Write-Host "ERROR: An exception occurred" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    exit 1
}
