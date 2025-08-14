# NBRM Exchange Rates Application Launcher
# Internship Project - National Bank of North Macedonia Integration

Write-Host "🏦 NBRM Exchange Rates Application" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green
Write-Host ""

# Check if .NET is installed
try {
    $dotnetVersion = dotnet --version
    Write-Host "✅ .NET SDK Version: $dotnetVersion" -ForegroundColor Green
} catch {
    Write-Host "❌ .NET SDK not found. Please install .NET 8.0 SDK" -ForegroundColor Red
    exit 1
}

# Build the project
Write-Host ""
Write-Host "🔨 Building project..." -ForegroundColor Yellow
dotnet build

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Build successful!" -ForegroundColor Green
} else {
    Write-Host "❌ Build failed!" -ForegroundColor Red
    exit 1
}

# Start the application
Write-Host ""
Write-Host "🚀 Starting NBRM Exchange Rates Application..." -ForegroundColor Yellow
Write-Host ""
Write-Host "📱 Once started, open your browser to:" -ForegroundColor Cyan
Write-Host "   • https://localhost:5001 (HTTPS)" -ForegroundColor Cyan
Write-Host "   • http://localhost:5000 (HTTP)" -ForegroundColor Cyan
Write-Host ""
Write-Host "⭐ Features:" -ForegroundColor Magenta
Write-Host "   • Current exchange rates from NBRM" -ForegroundColor White
Write-Host "   • Historical rates by date" -ForegroundColor White
Write-Host "   • Currency search and filtering" -ForegroundColor White
Write-Host "   • Bilingual interface (MK/EN)" -ForegroundColor White
Write-Host ""
Write-Host "🛑 Press Ctrl+C to stop the application" -ForegroundColor Red
Write-Host ""

# Run the application
dotnet run
