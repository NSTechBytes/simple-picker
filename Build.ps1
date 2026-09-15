param(
    [string]$Configuration = "Release",
    [string]$Platform = "x86"
)

$ErrorActionPreference = "Stop"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  SimplePicker Build Script" -ForegroundColor Cyan
Write-Host "  Configuration: $Configuration | Platform: $Platform" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

# 1. Locate solution file
$slnPath = Join-Path $PSScriptRoot "simple-picker.sln"
if (-not (Test-Path $slnPath)) {
    Write-Error "Solution file not found at: $slnPath"
    exit 1
}

Write-Host "[1/3] Solution: $slnPath" -ForegroundColor Green

# 2. Build using dotnet CLI
Write-Host "[2/3] Building solution ($Configuration|$Platform)..." -ForegroundColor Yellow

& dotnet build $slnPath `/p:Configuration=$Configuration `/p:Platform=$Platform `/verbosity:minimal

if ($LASTEXITCODE -ne 0) {
    Write-Error "Build failed with exit code $LASTEXITCODE"
    exit $LASTEXITCODE
}

Write-Host "Build succeeded!" -ForegroundColor Green

# 3. Populate dist\ folder
$distDir    = Join-Path $PSScriptRoot "dist"
$outDir     = Join-Path $PSScriptRoot "src\bin\$Platform\$Configuration\net8.0-windows"

Write-Host "`n[3/3] Preparing dist folder: $distDir" -ForegroundColor Cyan

# Clean and recreate dist
if (Test-Path $distDir) {
    Remove-Item -Recurse -Force $distDir
}
New-Item -ItemType Directory -Path $distDir -Force | Out-Null

# Files to copy from build output
$filesToCopy = @(
    "simple-picker.exe",
    "simple-picker.dll",
    "simple-picker.deps.json",
    "simple-picker.runtimeconfig.json"
)

foreach ($file in $filesToCopy) {
    $src = Join-Path $outDir $file
    if (Test-Path $src) {
        Copy-Item -Path $src -Destination $distDir -Force
        Write-Host "[dist] Copied $file" -ForegroundColor Green
    } else {
        Write-Warning "[dist] Not found: $src"
    }
}

# Copy resources folder from project root (source of truth)
$srcResources = Join-Path $PSScriptRoot "resources"
$dstResources = Join-Path $distDir "resources"

if (Test-Path $srcResources) {
    New-Item -ItemType Directory -Path $dstResources -Force | Out-Null
    Copy-Item -Path "$srcResources\*" -Destination $dstResources -Recurse -Force
    Write-Host "[dist] Copied resources folder from $srcResources" -ForegroundColor Green
} else {
    Write-Warning "[dist] resources folder not found at: $srcResources"
}

Write-Host "`nDist ready at: $distDir" -ForegroundColor Green
