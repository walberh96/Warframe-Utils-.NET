# Warframe Utils - Quick Setup Script
# Run this script to set up the frontend

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Warframe Utils - Frontend Setup" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Check if Node.js is installed
Write-Host "Checking prerequisites..." -ForegroundColor Yellow
$nodeVersion = node --version 2>$null
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Node.js is not installed!" -ForegroundColor Red
    Write-Host "Please install Node.js from https://nodejs.org/" -ForegroundColor Red
    exit 1
}
Write-Host "✓ Node.js version: $nodeVersion" -ForegroundColor Green

# Check if npm is installed
$npmVersion = npm --version 2>$null
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ npm is not installed!" -ForegroundColor Red
    exit 1
}
Write-Host "✓ npm version: $npmVersion" -ForegroundColor Green
Write-Host ""

# Navigate to frontend directory
Write-Host "Navigating to frontend directory..." -ForegroundColor Yellow
$frontendPath = Join-Path $PSScriptRoot "warframe-frontend"
if (-not (Test-Path $frontendPath)) {
    Write-Host "❌ Frontend directory not found!" -ForegroundColor Red
    exit 1
}
Set-Location $frontendPath
Write-Host "✓ Found frontend directory" -ForegroundColor Green
Write-Host ""

# Install dependencies
Write-Host "Installing dependencies..." -ForegroundColor Yellow
Write-Host "This may take a few minutes..." -ForegroundColor Gray
npm install
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Failed to install dependencies!" -ForegroundColor Red
    exit 1
}
Write-Host "✓ Dependencies installed successfully" -ForegroundColor Green
Write-Host ""

# Done
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  ✓ Setup Complete!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. Ensure your .NET backend is running" -ForegroundColor White
Write-Host "   cd 'Warframe Utils .NET'" -ForegroundColor Gray
Write-Host "   dotnet run" -ForegroundColor Gray
Write-Host ""
Write-Host "2. Start the frontend development server" -ForegroundColor White
Write-Host "   cd warframe-frontend" -ForegroundColor Gray
Write-Host "   npm run dev" -ForegroundColor Gray
Write-Host ""
Write-Host "3. Open your browser to:" -ForegroundColor White
Write-Host "   http://localhost:3000" -ForegroundColor Cyan
Write-Host ""
Write-Host "For detailed instructions, see SETUP_GUIDE.md" -ForegroundColor Yellow
Write-Host ""
