$ErrorActionPreference = 'Stop'
$repoRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$solution = Join-Path $repoRoot 'ThunderbirdsBoardGameEngine.ReferenceData.sln'
if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    Write-Error 'The .NET SDK is required to run pre-commit checks.'
    exit 1
}
Push-Location $repoRoot
try {
    Write-Host 'Running formatting verification...'
    dotnet format $solution --verify-no-changes
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

    Write-Host 'Building the Reference Data solution (Release)...'
    dotnet build $solution --configuration Release
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

    $unitTestProjects = @(Get-ChildItem -LiteralPath (Join-Path $repoRoot 'tests/UnitTests') -Recurse -Filter '*.UnitTests.csproj' -File | Sort-Object FullName)
    if ($unitTestProjects.Count -eq 0) {
        Write-Error 'No Reference Data unit-test projects were found.'
        exit 1
    }
    foreach ($project in $unitTestProjects) {
        Write-Host "Running unit tests: $($project.Name)"
        dotnet test $project.FullName --configuration Release --no-build --no-restore
        if ($LASTEXITCODE -ne 0) {
            Write-Host "Unit tests failed: $($project.Name)"
            exit $LASTEXITCODE
        }
    }
    Write-Host 'Pre-commit checks passed.'
}
finally {
    Pop-Location
}
exit 0
