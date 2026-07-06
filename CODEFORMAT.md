# Code Format

Formatting conventions are defined primarily by `.editorconfig`, with matching editor settings in `omnisharp.json`.

## C# Style

- Target modern C# with `LangVersion` set to `latest` in the `.csproj` project file.
- Use nullable annotations; the projects enable `<Nullable>enable</Nullable>`.
- Use file-scoped namespaces.
- Place `using` directives inside the namespace, matching `.editorconfig`.
- Use tabs for C# indentation where the existing files do.
- Keep braces on their own line for types and methods, matching the current source style.
- Prefer `var` when the type is apparent.
- Keep nullable annotations enabled and avoid weakening null-safety to silence warnings.

## Markdown Style

- Root project docs use uppercase filenames, such as `DEVELOPMENT.md` and `TODO.md`.
- Focused docs under `docs/` use lower kebab-case filenames.
- Prefer relative Markdown links.
- Keep scratch or historical material under `docs/scratch/`.

## XML Fixture Style

- Expected XML fixtures should represent intended CLI behavior, not incidental output from a temporary bug.
- Regenerate `*_sorted.xml` fixtures only after deciding and documenting the behavior being blessed.
- Preserve fixture readability and review fixture diffs carefully.

## Formatting Commands

```powershell
dotnet format SortXML.slnx
```

If `dotnet format` changes many unrelated files, stop and inspect the diff before continuing.