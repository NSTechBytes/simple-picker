param(
    [Parameter(Position = 0)]
    [string]$Version,

    [ValidateSet("major", "minor", "patch")]
    [string]$Bump = "patch",

    [switch]$DryRun
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

# ---------------------------------------------------------------------------
# Helpers
# ---------------------------------------------------------------------------
function Parse-Version {
    param([string]$Value)
    if ($Value -notmatch '^\d+\.\d+(\.\d+)?$') {
        throw "Invalid version format: '$Value'. Expected MAJOR.MINOR or MAJOR.MINOR.PATCH (e.g. 1.2 or 1.2.1)."
    }
    return ($Value -split '\.') | ForEach-Object { [int]$_ }
}

function Join-Version {
    param([int[]]$Parts)
    return ($Parts -join '.')
}

function Increment-Version {
    param([int[]]$Parts, [string]$Level)
    switch ($Level) {
        "major" { $Parts[0]++; if ($Parts.Count -gt 1) { $Parts[1] = 0 }; if ($Parts.Count -gt 2) { $Parts[2] = 0 } }
        "minor" { if ($Parts.Count -gt 1) { $Parts[1]++ } else { $Parts += 1 }; if ($Parts.Count -gt 2) { $Parts[2] = 0 } }
        "patch" { if ($Parts.Count -gt 2) { $Parts[2]++ } else { $Parts += 1 } }
        default { throw "Unsupported bump level: $Level" }
    }
    # Ensure we have at least 2 parts
    while ($Parts.Count -lt 2) { $Parts += 0 }
    return $Parts
}

function Update-FileContent {
    param([string]$Path, [scriptblock]$Transform)

    if (-not (Test-Path -LiteralPath $Path)) {
        throw "File not found: $Path"
    }

    $reader = [System.IO.StreamReader]::new($Path, $true)
    try {
        $original = $reader.ReadToEnd()
        $encoding = $reader.CurrentEncoding
    } finally {
        $reader.Dispose()
    }

    $updated = & $Transform $original

    if ($original -ne $updated) {
        if ($DryRun) {
            Write-Host "[DryRun] Would update: $Path" -ForegroundColor Yellow
        } else {
            [System.IO.File]::WriteAllText($Path, $updated, $encoding)
            Write-Host "Updated : $Path" -ForegroundColor Green
        }
    } else {
        Write-Host "No change: $Path"
    }
}

# ---------------------------------------------------------------------------
# Locate repo root and files
# ---------------------------------------------------------------------------
$repoRoot   = Split-Path -Parent $MyInvocation.MyCommand.Path
$versionIni = Join-Path $repoRoot ".github\version.ini"
$nsiFile    = Join-Path $repoRoot "Installer\Installer.nsi"
$manifest   = Join-Path $repoRoot "app.manifest"

# ---------------------------------------------------------------------------
# Read current version from .github\version.ini
# (format: version = 1.1)
# ---------------------------------------------------------------------------
if (-not (Test-Path -LiteralPath $versionIni)) {
    throw "Could not locate .github\version.ini at: $versionIni"
}

$iniText = Get-Content -LiteralPath $versionIni -Raw
$match   = [regex]::Match($iniText, 'version\s*=\s*(?<v>\d+\.\d+(?:\.\d+)?)')
if (-not $match.Success) {
    throw "Could not parse current version from .github\version.ini"
}

$currentVersion = $match.Groups["v"].Value

# ---------------------------------------------------------------------------
# Determine target version
# ---------------------------------------------------------------------------
$targetVersion = $Version
if ([string]::IsNullOrWhiteSpace($targetVersion)) {
    $parts       = Parse-Version $currentVersion
    $nextParts   = Increment-Version $parts $Bump
    $targetVersion = Join-Version $nextParts
}

[void](Parse-Version $targetVersion)   # validate format

$v        = $targetVersion -split '\.'
$verMajor = $v[0]
$verMinor = $v[1]
$verPatch = if ($v.Count -gt 2) { $v[2] } else { 0 }
$verFull  = "$verMajor.$verMinor.$verPatch.0"

Write-Host ""
Write-Host "=======================================" -ForegroundColor Cyan
Write-Host "  SimplePicker Version Bump" -ForegroundColor Cyan
Write-Host "=======================================" -ForegroundColor Cyan
Write-Host "  Current : $currentVersion"
Write-Host "  Target  : $targetVersion"
if ($DryRun) {
    Write-Host "  Mode    : DRY RUN" -ForegroundColor Yellow
}
Write-Host ""

# ---------------------------------------------------------------------------
# 1. .github\version.ini
#    - version = x.x
# ---------------------------------------------------------------------------
Update-FileContent -Path $versionIni -Transform {
    param($text)
    [regex]::Replace(
        $text,
        '(?m)(version\s*=\s*)\d+\.\d+(?:\.\d+)?',
        ('${1}' + $targetVersion)
    )
}

# ---------------------------------------------------------------------------
# 2. Installer\Installer.nsi
#    - !define VERSIONMAJOR x
#    - !define VERSIONMINOR x
# ---------------------------------------------------------------------------
if (Test-Path -LiteralPath $nsiFile) {
    Update-FileContent -Path $nsiFile -Transform {
        param($text)
        $text = [regex]::Replace($text, '(?m)^(!define\s+VERSIONMAJOR\s+)\d+', ('${1}' + $verMajor))
        $text = [regex]::Replace($text, '(?m)^(!define\s+VERSIONMINOR\s+)\d+', ('${1}' + $verMinor))
        return $text
    }
} else {
    Write-Warning "Installer.nsi not found at: $nsiFile — skipping NSIS version update."
}

# ---------------------------------------------------------------------------
# 3. app.manifest
#    - assemblyIdentity version="x.x.x.0"
# ---------------------------------------------------------------------------
if (Test-Path -LiteralPath $manifest) {
    Update-FileContent -Path $manifest -Transform {
        param($text)
        [regex]::Replace(
            $text,
            '(?m)(<assemblyIdentity\s+version=")\d+\.\d+\.\d+\.\d+(")',
            ('${1}' + $verFull + '${2}')
        )
    }
} else {
    Write-Warning "app.manifest not found at: $manifest — skipping manifest version update."
}

# ---------------------------------------------------------------------------
# Done
# ---------------------------------------------------------------------------
Write-Host ""
Write-Host "=======================================" -ForegroundColor Green
Write-Host "  Version bump complete: $targetVersion" -ForegroundColor Green
Write-Host "=======================================" -ForegroundColor Green
Write-Host ""
Write-Host "Updated files:"
Write-Host "  - .github\version.ini"
Write-Host "  - Installer\Installer.nsi (VERSIONMAJOR/VERSIONMINOR)"
Write-Host "  - app.manifest (assemblyIdentity version)"
Write-Host ""
Write-Host "Next steps:"
Write-Host "  1. git add -A"
Write-Host "  2. git commit -m `"chore: bump version to $targetVersion`""
Write-Host "  3. git tag v$targetVersion"
Write-Host "  4. git push origin main --tags"
