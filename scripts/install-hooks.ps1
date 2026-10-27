$ErrorActionPreference = 'Stop'
$repoRoot = git -C $PSScriptRoot rev-parse --show-toplevel
if ($LASTEXITCODE -ne 0) {
    Write-Error 'Could not determine the Git repository root.'
    exit 1
}
$customHooksPath = git -C $repoRoot config --get core.hooksPath
if ($LASTEXITCODE -gt 1) {
    Write-Error 'Could not inspect Git hook configuration.'
    exit 1
}
if (-not [string]::IsNullOrWhiteSpace($customHooksPath)) {
    Write-Error 'core.hooksPath is already configured. This installer leaves custom hook configurations unchanged.'
    exit 1
}
$hookPath = git -C $repoRoot rev-parse --path-format=absolute --git-path hooks/pre-commit
if ($LASTEXITCODE -ne 0) {
    Write-Error 'Could not determine the Git pre-commit hook location.'
    exit 1
}
$managedMarker = '# Managed by Reference Data scripts/install-hooks.ps1'
if ((Test-Path -LiteralPath $hookPath) -and -not ([System.IO.File]::ReadAllText($hookPath).Contains($managedMarker))) {
    Write-Error 'An existing unmanaged pre-commit hook was found. It has not been overwritten.'
    exit 1
}
$hookContent = @'
#!/bin/sh
# Managed by Reference Data scripts/install-hooks.ps1
repo_root=$(git rev-parse --show-toplevel) || exit 1
if command -v pwsh >/dev/null 2>&1; then
    exec pwsh -NoProfile -File "$repo_root/scripts/pre-commit.ps1"
elif command -v powershell.exe >/dev/null 2>&1; then
    exec powershell.exe -ExecutionPolicy Bypass -NoProfile -File "$repo_root/scripts/pre-commit.ps1"
else
    echo "PowerShell is required for Reference Data pre-commit checks." >&2
    exit 1
fi
'@
[System.IO.Directory]::CreateDirectory([System.IO.Path]::GetDirectoryName($hookPath)) | Out-Null
[System.IO.File]::WriteAllText($hookPath, $hookContent + [char]10, [System.Text.Encoding]::ASCII)
if ([System.Environment]::OSVersion.Platform -ne [System.PlatformID]::Win32NT) {
    chmod +x $hookPath
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
}
Write-Host "Pre-commit hook installed at $hookPath"
exit 0
