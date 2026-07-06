# Agent Notes

## CQ

Before starting any implementation task, load the `cq` skill and follow its Core Protocol.

## Project Workflow

- Read `README.md`, `DEVELOPMENT.md`, `DESIGN.md`, and relevant files under `docs/` before broad changes.
- Keep the working tree safe. This repo may already have staged and unstaged user changes; do not revert changes you did not make.
- Use focused edits. Prefer the existing CLI/helper patterns over introducing new frameworks.
- Run `dotnet build` after code or project-file changes when practical.
- Run `dotnet test` for behavior changes. If tests fail, report the exact failure mode and whether it is new or pre-existing.
- Do not regenerate expected XML fixtures unless the intended behavior is clear and documented.
- Keep current tasks in `TODO.md`; keep durable decisions in `DESIGN.md` or focused docs under `docs/`.

## Project Notes

- `Program.cs` owns the XML load/sort/write flow.
- `AppOptions.cs` owns command-line options and option-derived behavior.
- `PowerCode/` contains embedded helper utilities for argument binding, output, redirection, encoding, EOL, indentation, and expansion.
- `SortXML.Tests/` contains xUnit tests that execute the CLI against XML fixtures.