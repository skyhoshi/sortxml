# Development

## Requirements

- .NET SDK 10.0 or newer for the current `net10.0` target.
- PowerShell for the fixture regeneration helper.

Check the local SDK:

```powershell
dotnet --info
```

## Restore And Build

```powershell
dotnet restore
dotnet build
```

The solution file is `SortXML.slnx`. The CLI project is `SortXML.csproj`, and tests live in `SortXML.Tests/`.

## Run Locally

Show help:

```powershell
dotnet run --project SortXML.csproj -- -help
```

Sort a file in place:

```powershell
dotnet run --project SortXML.csproj -- .\SortXML.Tests\test_files\f.xml
```

Write sorted output to a separate file:

```powershell
dotnet run --project SortXML.csproj -- .\SortXML.Tests\test_files\f.xml -out-file .\out.xml
```

Send sorted output to stdout:

```powershell
dotnet run --project SortXML.csproj -- .\SortXML.Tests\test_files\f.xml -stdout
```

## Tests

Run the test suite:

```powershell
dotnet test
```

The test project references the CLI project so `dotnet test` builds the executable before shelling out to `dotnet run --no-build`.

The full fixture suite is expected to pass. If a future change alters XML output, review the behavior before regenerating expected files.

## Regenerate Expected XML Fixtures

Only regenerate fixtures after confirming the intended behavior.

```powershell
.\SortXML.Tests\test_files\regenerate_expected.ps1
```

Then review fixture diffs:

```powershell
git diff -- SortXML.Tests/test_files/
```

## Publish

The project is configured for single-file publishing and currently has `RuntimeIdentifier` set to `win-x64` in `SortXML.csproj`.

Local Windows publish:

```powershell
dotnet publish SortXML.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=false
```

There are additional publish profiles under `Properties/PublishProfiles/`. The GitHub Actions workflows still need to be refreshed for the current `net10.0` target.

## Troubleshooting

If tests fail with a missing `sortxml.exe`, confirm `SortXML.Tests.csproj` contains a `ProjectReference` to `..\SortXML.csproj`.

If tests fail only on whitespace, investigate option presence tracking and formatting detection before changing expected fixtures.