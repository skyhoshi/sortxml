# TODO

## Test And Formatting Behavior

- [x] Add a `ProjectReference` from `SortXML.Tests` to `SortXML.csproj` so `dotnet test` builds the CLI before shell-out tests run.
- [ ] Fix argument-binder/default-value behavior so defaulted options do not mark `EncodingWasSet`, `EolWasSet`, `IndentationWasSet`, or XML declaration flags as explicitly supplied.
- [ ] Add focused coverage for default formatting detection versus explicit `-encoding`, `-eol`, and `-indentation` overrides.
- [ ] Re-run `dotnet test` after the option-presence fix and decide whether any expected fixtures still need regeneration.
- [ ] Regenerate `*_sorted.xml` fixtures only after the intended indentation behavior is documented.

## CI And Release

- [ ] Update GitHub Actions from .NET Core 3.1 to the current `net10.0` SDK target.
- [ ] Replace deprecated GitHub Actions syntax such as `set-env`.
- [ ] Refresh action versions for checkout, setup-dotnet, release creation, and asset upload.
- [ ] Decide whether `RuntimeIdentifier=win-x64` should remain in `SortXML.csproj` or move into publish profiles only.
- [ ] Update release publishing commands after CI is refreshed.

## Documentation

- [ ] Keep `README.md` as the project front door and move detailed workflow notes into `DEVELOPMENT.md`.
- [ ] Add focused docs when behavior areas grow beyond `docs/cli-behavior.md` and `docs/test-fixtures.md`.
- [ ] Refresh generated CLI usage text in docs after option behavior stabilizes.