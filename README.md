# sortxml

![dotnet-core-build](https://github.com/kodybrown/sortxml/workflows/dotnet-core-build/badge.svg)
![dotnet-core-release](https://github.com/kodybrown/sortxml/workflows/dotnet-core-release/badge.svg)

sortxml is a command-line utility that sorts and prettifies XML files using the Microsoft XML .NET APIs.

Latest release: <https://github.com/kodybrown/sortxml/releases/latest/>

## Project Layout

- `Program.cs`: CLI entry point and XML load/sort/write flow.
- `AppOptions.cs`: command-line options and option-derived behavior.
- `PowerCode/`: embedded helper utilities for argument binding, output, stdin/stdout, encoding, EOL, indentation, and expansion.
- `SortXML.Tests/`: xUnit tests and XML fixtures.
- `Properties/PublishProfiles/`: publish profiles.
- `docs/`: focused documentation.

## Main Capabilities

- Sort XML nodes and attributes.
- Prettify XML output.
- Process one or more files in place.
- Write to stdout or a separate output file.
- Read XML from redirected stdin.
- Preserve or override encoding, line endings, indentation, and XML declaration behavior.
- Configure case-sensitive or case-insensitive sorting.
- Put a primary attribute first when sorting attributes.

## Quick Start

Show help:

```powershell
dotnet run --project SortXML.csproj -- -help
```

Sort a file in place:

```powershell
dotnet run --project SortXML.csproj -- .\SortXML.Tests\test_files\f.xml
```

Write sorted XML to a separate file:

```powershell
dotnet run --project SortXML.csproj -- .\SortXML.Tests\test_files\f.xml -out-file .\out.xml
```

## Core Docs

- [docs/README.md](docs/README.md): focused documentation index.
- [DEVELOPMENT.md](DEVELOPMENT.md): local setup, build, test, fixture, and publish workflow.
- [DESIGN.md](DESIGN.md): durable CLI/XML behavior decisions.
- [TODO.md](TODO.md): actionable backlog.
- [AGENTS.md](AGENTS.md): AI/code-agent instructions.
- [CODEFORMAT.md](CODEFORMAT.md): formatting and style conventions.
- [DOCS_STRUCTURE.md](DOCS_STRUCTURE.md): documentation organization guide.

## Development

See [DEVELOPMENT.md](DEVELOPMENT.md).