# CLI Behavior

This is the focused source of truth for sortxml command-line behavior. Broader design principles live in `../DESIGN.md`; setup and command examples live in `../DEVELOPMENT.md`.

## Inputs

sortxml accepts one or more XML file paths. When no files are provided, redirected stdin may be processed as input.

## Outputs

- Default file mode updates each input file in place.
- `-out-file` writes to a named output file and is valid only with one input file.
- `-stdout` writes sorted XML to stdout and leaves the input file unchanged.
- Stdin input defaults to stdout unless `-out-file` is supplied.

## Formatting

Pretty output is enabled by default. When formatting options are omitted, sortxml should attempt to preserve source encoding, EOL, and indentation. Explicit options override source detection.

Relevant options:

- `-encoding`: output encoding override.
- `-eol` or `-end-of-line`: output line ending override.
- `-indentation`: output indentation override.
- `-attributes-on-new-line`: writes each attribute on its own line when supported by the XML writer.
- `-include-xml-declaration` and `-omit-xml-declaration`: override source XML declaration behavior.

## Sorting

Node and attribute sorting are enabled by default.

- `-sort` toggles both node and attribute sorting.
- `-sort-nodes` controls node sorting.
- `-sort-attributes` controls attribute sorting.
- `-primary-attribute` moves a named attribute before other attributes.
- `-sort-childless-nodes-first` influences tie-breaking for same-name nodes.

## Comparison

Case-insensitive comparison is the intended default for nodes and attributes. Case-sensitive comparison can be selected globally or separately for nodes and attributes.

## Implementation Notes

Option presence is meaningful. An unset formatting option means "detect from source"; an explicitly supplied option means "use this value." Default values in the argument binder must not erase that distinction.