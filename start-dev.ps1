# Start Development Servers
# This script starts both the .NET backend and Next.js frontend in separate windows

Write-Host "Starting Warframe Utils Development Environment..." -ForegroundColor Cyan

# Get script directory
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path

# Define paths
$backendPath = Join-Path $scriptPath "Warframe Utils .NET"
$frontendPath = Join-Path $scriptPath "warframe-frontend"

# Start .NET backend in new window
Write-Host "`nStarting .NET Backend in new window..." -ForegroundColor Green
$dotnetProcess = Start-Process powershell -ArgumentList @(
    "-NoExit",
    "-Command",
    "Set-Location '$backendPath'; Write-Host 'Starting .NET Backend...' -ForegroundColor Cyan; dotnet run"
) -PassThru

Start-Sleep -Seconds 2

# Start Next.js frontend in new window
Write-Host "Starting Next.js Frontend in new window..." -ForegroundColor Green
$npmProcess = Start-Process powershell -ArgumentList @(
    "-NoExit", 
    "-Command",
    "Set-Location '$frontendPath'; Write-Host 'Starting Next.js Frontend...' -ForegroundColor Cyan; npm run dev"
) -PassThru

Write-Host "`n============================================" -ForegroundColor Cyan
Write-Host "Development servers started!" -ForegroundColor Green
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "Backend (.NET):  Running in separate window" -ForegroundColor Yellow
Write-Host "Frontend (Next): Running in separate window" -ForegroundColor Yellow
Write-Host "`nExpected URLs:" -ForegroundColor Cyan
Write-Host "  Backend:  http://localhost:5089" -ForegroundColor White
Write-Host "  Frontend: http://localhost:3000" -ForegroundColor White
Write-Host "`nClose the terminal windows to stop the servers" -ForegroundColor Red
Write-Host "============================================" -ForegroundColor Cyan
