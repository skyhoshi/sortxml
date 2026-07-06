# Test Fixtures

The xUnit tests in `SortXML.Tests/` execute the CLI against XML files under `SortXML.Tests/test_files/`.

## Fixture Layout

Each input fixture has a matching expected output fixture:

```text
b.xml
b_sorted.xml
c.xml
c_sorted.xml
```

Tests copy input files to temporary paths before running in-place behavior so the committed fixtures are not modified during the test run.

## Current Test State

`dotnet test` now builds the CLI project before running tests because `SortXML.Tests.csproj` references `..\SortXML.csproj`.

The remaining known failures are indentation differences. Actual output currently uses two spaces where expected fixtures preserve tabs or one-space indentation. Treat this as a behavior issue to investigate before regenerating expected files.

## Expected File Policy

Expected files are behavior contracts. Update them only when the desired behavior changes or when a bug fix intentionally changes output.

Before updating expected files, confirm:

- The CLI behavior is documented in `cli-behavior.md` or `../DESIGN.md`.
- The corresponding task in `../TODO.md` is updated.
- The fixture diff shows only intentional changes.

## Regeneration Workflow

Run the helper from the repo root:

```powershell
.\SortXML.Tests\test_files\regenerate_expected.ps1
```

Review the resulting diff:

```powershell
git diff -- SortXML.Tests/test_files/
```

Do not commit regenerated fixtures without reviewing the XML behavior change that caused them.