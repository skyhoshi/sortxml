# Design

## Product Shape

sortxml is a command-line XML normalization tool. Its primary job is to sort XML nodes and attributes while making output formatting predictable enough for repeated use in source-controlled files.

## I/O Modes

- With file arguments, the default behavior is to update input files in place.
- `-out-file` writes one input file to a separate output file and should reject multiple inputs.
- `-stdout` writes sorted XML to stdout and should not modify the input file.
- With no file arguments, redirected stdin may be read into a temporary file and written to stdout unless an output file is specified.

## Sorting Rules

- Attribute sorting is recursive.
- Attributes sort by name and then value using the configured attribute comparison mode.
- A primary attribute, when configured, sorts before other attributes.
- Node sorting is recursive.
- Nodes sort by name using the configured node comparison mode.
- When node names match, the string form of the whole node breaks ties.
- `-sort-childless-nodes-first` optionally places nodes without children before nodes with children when names match.

## Formatting Rules

- Pretty output is enabled by default.
- If encoding, EOL, or indentation are not explicitly set, the app should attempt to preserve the source file's corresponding style.
- Explicit formatting options override source detection.
- XML declaration behavior should preserve the source by default unless `-include-xml-declaration` or `-omit-xml-declaration` is specified.

## Argument Binding

Option presence and option value are separate concerns. A default value applied by the argument binder must not mark an option as explicitly supplied by the user. This matters for options whose unset state means "detect from source," such as indentation, EOL, encoding, and XML declaration behavior.

## Tests As Contract

The XML fixtures in `SortXML.Tests/test_files/` are behavior contracts. Update expected files only when the intended behavior changes, not merely because current output changed during a refactor.