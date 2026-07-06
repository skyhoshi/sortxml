# Regenerates all *_sorted.xml files by running the current sortxml build
# against each corresponding input file (e.g. b.xml -> b_sorted.xml).
#
# Usage: .\SortXML.Tests\test_files\regenerate_expected.ps1

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$projectDir = Split-Path -Parent (Split-Path -Parent $scriptDir)
$projectFile = Join-Path $projectDir "SortXML.csproj"

$inputFiles = Get-ChildItem -Path $scriptDir -Filter "*.xml" |
  Where-Object { $_.Name -notmatch "_(sorted|handsorted)\.xml$" }

foreach ($file in $inputFiles) {
  $baseName = [System.IO.Path]::GetFileNameWithoutExtension($file.Name)
  $sortedFile = Join-Path $scriptDir "${baseName}_sorted.xml"

  # Copy input to the _sorted.xml path, then run sortxml in-place on it.
  Copy-Item $file.FullName $sortedFile -Force

  Write-Host "Generating: $($file.Name) -> ${baseName}_sorted.xml"
  dotnet run --no-launch-profile --project "$projectFile" -- "$sortedFile"

  if ($LASTEXITCODE -ne 0) {
    Write-Host "  ** FAILED (exit code $LASTEXITCODE) **" -ForegroundColor Red
  } else {
    Write-Host "  OK" -ForegroundColor Green
  }
}

Write-Host "`nDone. Review the changes with 'git diff SortXML.Tests/test_files/'."
