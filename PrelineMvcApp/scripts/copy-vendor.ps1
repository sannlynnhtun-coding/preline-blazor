$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $PSScriptRoot
$vendorDir = Join-Path $root "wwwroot/js/vendor"

New-Item -Path $vendorDir -ItemType Directory -Force | Out-Null

$assets = @(
    @{ Source = Join-Path $root "node_modules/preline/dist/preline.js"; Target = Join-Path $vendorDir "preline.js" },
    @{ Source = Join-Path $root "node_modules/highcharts/highcharts.js"; Target = Join-Path $vendorDir "highcharts.js" }
)

foreach ($asset in $assets) {
    if (-not (Test-Path $asset.Source)) {
        throw "Missing dependency asset: $($asset.Source)"
    }

    if (-not (Test-Path $asset.Target)) {
        Copy-Item -Path $asset.Source -Destination $asset.Target
        Write-Host "copied: $($asset.Target.Replace($root + '\\', ''))"
    }
    else {
        Write-Host "exists: $($asset.Target.Replace($root + '\\', ''))"
    }
}
