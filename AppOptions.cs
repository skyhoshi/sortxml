namespace sortxml;

using PowerCode;

internal sealed class AppOptions
{
	private const int COMMON = AppArgumentAttribute.DefaultGlobalOrder + 100;
	private const int FILE = AppArgumentAttribute.DefaultPropertyOrder + 200;
	private const int FORMATTING = AppArgumentAttribute.DefaultPropertyOrder + 300;
	private const int FILETYPE = AppArgumentAttribute.DefaultPropertyOrder + 400;
	private const int SORT = AppArgumentAttribute.DefaultPropertyOrder + 500;
	private const int COMPARE = AppArgumentAttribute.DefaultPropertyOrder + 600;

	// Common options

	[AppArgument(
		namedParameters: ["help", "?", "h"],
		description: "Show help and usage information.",
		order: COMMON + 1,
		valueIsOptional: true,
		defaultIfNoValue: "",
		defaultIfMissing: null,
		allowEnvar: false
	)]
	public string Help { get; set; }
	public bool HelpWasSet => Help != null;

	[AppArgument(
		namedParameters: ["about"],
		description: "Show full app details.",
		order: COMMON + 3,
		valueIsOptional: true,
		defaultIfNoValue: true,
		allowEnvar: false
	)]
	public bool About { get; set; }

	[AppArgument(
		namedParameters: ["v"],
		description: "Show version (short).",
		order: COMMON + 10,
		valueIsOptional: true,
		defaultIfNoValue: true,
		allowEnvar: false
	)]
	public bool Version { get; set; }

	[AppArgument(
		namedParameters: ["version"],
		description: "Show full version details.",
		order: COMMON + 11,
		valueIsOptional: true,
		defaultIfNoValue: true,
		allowEnvar: false
	)]
	public bool VersionFull { get; set; }

	//[AppArgument(
	//	namedParameters: ["quiet"],
	//	description: "Limits the output.",
	//	order: COMMON + 20,
	//	allowEnvar: true
	//)]
	//public bool Quiet { get; set; }

	[AppArgument(
		namedParameters: ["pause"],
		description: "Pause before exiting always or when there's an error.",
		order: COMMON + 21,
		valueIsOptional: true,
		defaultIfNoValue: Pause.Always,
		defaultIfMissing: Pause.Never,
		allowEnvar: true
	)]
	public Pause Pause { get; set; } = Pause.Never;

	//[AppArgument(
	//	namedParameters: ["e", "verbosity"],
	//	description: "Set the verbosity level of the output.",
	//	order: COMMON + 30,
	//	defaultIfNoValue: Verbosity.Verbose,
	//	defaultIfMissing: Verbosity.None,
	//	allowEnvar: true
	//)]
	//public Verbosity Verbosity { get; set; } = Verbosity.None;

	[AppArgument(
		namedParameters: ["show-examples", "examples"],
		showInHelp: false
	)]
	public bool ShowExamples { get; set; }

	[AppArgument(
		namedParameters: ["show-envars", "envars"],
		showInHelp: false
	)]
	public bool ShowEnvars { get; set; }

	// App options

	[UnhandledArguments(
		name: "files",
		description: "The XML file(s) to sort (or pipe XML via stdin)",
		required: false
	)]
	public List<string> Files { get; set; }

	[AppArgument(
		namedParameters: ["o", "out-file"],
		description: "Saves the output to the specified file (instead of overwriting input file)\nYou can only specify one input file when using this option",
		order: FILE + 1,
		defaultIfMissing: null,
		allowEnvar: false
	)]
	public string OutFile { get; set; }
	public bool OutFileWasSet => !string.IsNullOrEmpty(OutFile);

	[AppArgument(
		namedParameters: ["stdout", "redirect-to-stdout"],
		description: "Redirects sorted file to stdout out (does not change input file)",
		order: FILE + 1,
		defaultIfNoValue: true,
		allowEnvar: false
	)]
	public bool RedirectToStdOut { get; set; }

	// FORMATTING

	[AppArgument(
		namedParameters: ["p", "pretty", "prettify"],
		description: "Ignores the input format and prettifies the output",
		order: FORMATTING + 1,
		defaultIfMissing: true,
		allowEnvar: true
	)]
	public bool Prettify { get; set; }

	[AppArgument(
		namedParameters: ["attributes-on-new-line"],
		description: "Separates each attribute onto its own line",
		order: FORMATTING + 10,
		valueIsOptional: true,
		defaultIfNoValue: true,
		defaultIfMissing: false,
		allowEnvar: true
	)]
	public bool AttributesOnNewLine { get; set; }
	public bool AttributesOnNewLineWasSet { get; set; } = false;

	[AppArgument(
		namedParameters: ["include-xml-declaration"],
		description: "Includes the XML declaration in the output (overrides the source file)",
		order: FORMATTING + 11,
		valueIsOptional: true,
		defaultIfNoValue: true,
		defaultIfMissing: false,
		allowEnvar: true
	)]
	public bool IncludeXmlDeclaration { get; set; }
	public bool IncludeXmlDeclarationWasSet { get; set; } = false;

	[AppArgument(
		namedParameters: ["omit-xml-declaration"],
		description: "Omits the XML declaration from the output (overrides the source file)",
		order: FORMATTING + 11,
		valueIsOptional: true,
		defaultIfNoValue: true,
		defaultIfMissing: false,
		allowEnvar: true
	)]
	public bool OmitXmlDeclaration {
		get => !IncludeXmlDeclaration;
		set => IncludeXmlDeclaration = !value;
	}
	public bool OmitXmlDeclarationWasSet { get; set; } = false;

	// FILETYPE

	[AppArgument(
		namedParameters: ["encoding"],
		description: "Overrides the output encoding. If unset defaults to source file",
		order: FILETYPE + 10,
		allowedValues: ["utf8", "ascii", "unicode", "utf32", "os"],
		defaultIfMissing: "utf8",
		allowEnvar: true
	)]
	public string Encoding { get; set; } = string.Empty;
	public bool EncodingWasSet { get; set; } = false;

	[AppArgument(
		namedParameters: ["eol", "end-of-line"],
		description: "Overrides the EOL character(s). If unset defaults to source file",
		order: FILETYPE + 10,
		allowedValues: ["\\n", "\\r\\n", "\\r", "os"],
		defaultIfMissing: "os",
		allowEnvar: true
	)]
	public string Eol { get; set; } = string.Empty;
	public bool EolWasSet { get; set; } = false;

	[AppArgument(
		namedParameters: ["indentation"],
		description: "Overrides the indentation. If unset (attempts) to use source file.\nYou can use \"\\t\" for tab and \" \" or \"\\s\" for space",
		order: FILETYPE + 10,
		defaultIfMissing: "  ",
		allowEnvar: true
	)]
	public string Indentation { get; set; } = string.Empty;
	public bool IndentationWasSet { get; set; } = false;

	// SORT

	[AppArgument(
		namedParameters: ["sort"],
		description: "Sorts nodes and attributes",
		order: SORT + 1,
		valueIsOptional: true,
		defaultIfNoValue: true,
		defaultIfMissing: true,
		allowEnvar: true
	)]
	public bool SortNodesAndAttributes { set { SortNodes = value; SortAttributes = value; } }

	[AppArgument(
		namedParameters: ["sort-nodes"],
		description: "Sorts nodes (defaults to true)",
		order: SORT + 2,
		valueIsOptional: true,
		defaultIfNoValue: true,
		defaultIfMissing: true,
		allowEnvar: true
	)]
	public bool SortNodes { get; set; }

	[AppArgument(
		namedParameters: ["sort-attributes"],
		description: "Sorts attributes (defaults to true)",
		order: SORT + 3,
		valueIsOptional: true,
		defaultIfNoValue: true,
		defaultIfMissing: true,
		allowEnvar: true
	)]
	public bool SortAttributes { get; set; }

	[AppArgument(
		namedParameters: ["sort-childless-nodes-first"],
		description: "Sorts nodes without children first (before nodes with children)",
		order: SORT + 5,
		valueIsOptional: true,
		defaultIfNoValue: true,
		defaultIfMissing: false,
		allowEnvar: true
	)]
	public bool SortChildlessNodesFirst { get; set; }

	[AppArgument(
		namedParameters: ["primary-attribute"],
		description: "Specifies an attribute that will always be sorted first",
		order: SORT + 10,
		defaultIfMissing: null,
		allowEnvar: true
	)]
	public string PrimarySortAttribute { get; set; }

	// COMPARE

	public StringComparison NodeStringComparison { get; set; } = StringComparison.CurrentCulture;
	public StringComparison AttributeStringComparison { get; set; } = StringComparison.CurrentCulture;

	[AppArgument(
		namedParameters: ["i", "ignore-case"],
		description: "Compares nodes and attributes without regard to letter case",
		order: COMPARE + 1,
		valueIsOptional: true,
		defaultIfNoValue: true,
		defaultIfMissing: true,
		allowEnvar: true
	)]
	public bool IgnoreCase {
		set {
			NodeStringComparison = StringComparison.CurrentCultureIgnoreCase;
			AttributeStringComparison = StringComparison.CurrentCultureIgnoreCase;
		}
	}

	[AppArgument(
		namedParameters: ["in", "ignore-case-nodes"],
		description: "Compares nodes and attributes without regard to letter case",
		order: COMPARE + 2,
		valueIsOptional: true,
		defaultIfNoValue: true,
		defaultIfMissing: true,
		allowEnvar: true
	)]
	public bool IgnoreCaseNodes {
		set => NodeStringComparison = StringComparison.CurrentCultureIgnoreCase;
	}

	[AppArgument(
		namedParameters: ["ia", "ignore-case-attributes"],
		description: "Compares attributes without regard to letter case",
		order: COMPARE + 3,
		valueIsOptional: true,
		defaultIfNoValue: true,
		defaultIfMissing: true,
		allowEnvar: true
	)]
	public bool IgnoreCaseAttributes {
		set => AttributeStringComparison = StringComparison.CurrentCultureIgnoreCase;
	}

	[AppArgument(
		namedParameters: ["t", "compare-case"],
		description: "Compares nodes and attributes comparing letter case",
		order: COMPARE + 11,
		valueIsOptional: true,
		defaultIfNoValue: true,
		defaultIfMissing: false,
		allowEnvar: true
	)]
	public bool CompareCase {
		set {
			NodeStringComparison = StringComparison.CurrentCulture;
			AttributeStringComparison = StringComparison.CurrentCulture;
		}
	}

	[AppArgument(
		namedParameters: ["tn", "compare-case-nodes"],
		description: "Compares nodes comparing letter case",
		order: COMPARE + 12,
		valueIsOptional: true,
		defaultIfNoValue: true,
		defaultIfMissing: false,
		allowEnvar: true
	)]
	public bool CompareCaseNodes {
		set {
			NodeStringComparison = StringComparison.CurrentCulture;
		}
	}

	[AppArgument(
		namedParameters: ["ta", "compare-case-attributes"],
		description: "Compares attributes comparing letter case",
		order: COMPARE + 13,
		valueIsOptional: true,
		defaultIfNoValue: true,
		defaultIfMissing: false,
		allowEnvar: true
	)]
	public bool CompareCaseAttributes {
		set {
			AttributeStringComparison = StringComparison.CurrentCulture;
		}
	}
}
