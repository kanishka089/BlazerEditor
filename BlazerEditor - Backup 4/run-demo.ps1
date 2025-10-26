# BlazerEditor Demo Runner
Write-Host "🎨 BlazerEditor Demo Runner" -ForegroundColor Cyan
Write-Host "================================" -ForegroundColor Cyan
Write-Host ""

# Step 1: Build the library
Write-Host "📦 Step 1: Building BlazerEditor library..." -ForegroundColor Yellow
dotnet build BlazerEditor.csproj
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Build failed!" -ForegroundColor Red
    exit 1
}
Write-Host "✅ Library built successfully!" -ForegroundColor Green
Write-Host ""

# Step 2: Run the demo
Write-Host "🚀 Step 2: Starting demo application..." -ForegroundColor Yellow
Write-Host ""
Write-Host "Demo will be available at:" -ForegroundColor Cyan
Write-Host "  https://localhost:5001" -ForegroundColor White
Write-Host "  http://localhost:5000" -ForegroundColor White
Write-Host ""
Write-Host "Press Ctrl+C to stop the demo" -ForegroundColor Gray
Write-Host ""

Set-Location Demo
dotnet run
