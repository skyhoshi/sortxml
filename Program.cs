/*!
	Copyright (c) 2014-2026 Kody Brown (@kodybrown)

	MIT License:

	Permission is hereby granted, free of charge, to any person obtaining a copy
	of this software and associated documentation files (the "Software"), to
	deal in the Software without restriction, including without limitation the
	rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
	sell copies of the Software, and to permit persons to whom the Software is
	furnished to do so, subject to the following conditions:

	The above copyright notice and this permission notice shall be included in
	all copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
	FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
	DEALINGS IN THE SOFTWARE.
*/

namespace sortxml;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using PowerCode;

public class Program
{
	public static int Main( string[] args )
	{
		return new Program().Run(args);
	}

	private readonly AppOptions Opt = new();
	private int ExitCode { get; set; }
	private Dictionary<int, string> ErrorMessages { get; set; } = [];

	public Program()
	{
		App.AppName = "sortxml";
		App.AppDescription = @"A command-line utility that sorts the nodes and attributes of XML files.
Features:
- update the input files or output to stdio.
- accept multiple files at once or piped input.
- prettify the output with configurable encoding, indentation, and EOL characters.
- include or omit the XML declaration.
- specify a primary attribute to always sort first.
- choose to sort nodes and/or attributes.
- specify string comparison options for sorting.
- optionally put attributes on new lines.";
		App.AppProduct = "PowerTools";
		App.AppAuthors = "Kody Brown (@kodybrown)";
		App.AppCopyright = "Copyright (c) 2014-2026 Kody Brown";
		App.AppEnvarPrefix = "sortxml_";
		App.AppRepositoryUrl = "https://github.com/kodybrown/sortxml";
	}

	public int Run( string[] arguments )
	{
		var parse = App.ParseCommandLineArguments(arguments, Opt, allowEnvars: true);
		if (parse.ShouldExit) {
			return parse.ExitCode;
		}

		if (Opt.HelpWasSet || Opt.ShowExamples || Opt.ShowEnvars) {
			if (Opt.Help is "examples" or "show-examples" || Opt.ShowExamples) {
				ShowExamples(showHeader: true);
			} else if (Opt.Help is "envars" or "show-envars" || Opt.ShowEnvars) {
				App.ShowEnvars(Opt, showHeader: true);
			} else {
				App.ShowUsage(Opt, Opt.Help);
			}
			App.PauseIfNeeded(Opt.Pause);
			return 0;
		}

		var doc = new XmlDocument();
		string? stdinTempFile = null;

		// If no files were specified, check for piped stdin input.
		var fileCount = Opt.Files?.Count ?? 0;
		if (fileCount == 0) {
			if (App.IsInputRedirected) {
				// The user is piping or redirecting XML input to stdin, so we'll
				// read that input into a temporary file and process it like normal.
				stdinTempFile = App.ReadStdInToTempFile();
				if (stdinTempFile is null) {
					Console.Error.WriteLine("**** No input received from stdin. ****");
					return 1;
				}
				Opt.Files ??= [];
				Opt.Files.Add(stdinTempFile);
				if (!Opt.OutFileWasSet) {
					Opt.RedirectToStdOut = true;
				}
			} else {
				Console.WriteLine("**** Missing file. ****\n");
				App.ShowUsage(Opt, nameof(Opt.Files));
				return 1;
			}
		}

		if (Opt.OutFileWasSet && Opt.Files.Count > 1) {
			Console.WriteLine("**** Cannot specify more than one input file when using the -out-file option. ****");
			return 99;
		}

		foreach (var fileName in Opt.Files) {
			// Determine the output file name.
			// If the user specified an output file, use that, otherwise overwrite the input file.
			var outFile = Opt.OutFileWasSet
				? Opt.OutFile
				: fileName;

			try {
				// If the user is prettifying the output, then we MUST NOT preserve whitespace when loading the XML,
				// since we want to ignore the input formatting and reformat it according to the specified options.
				// However, if the user is not prettifying the output, then we SHOULD preserve whitespace when loading the XML,
				// since we want to maintain the input formatting as much as possible (except for sorting, of course).
				doc.PreserveWhitespace = !Opt.Prettify;

				// Load the XML document from the input file.
				doc.LoadXml(File.ReadAllText(fileName));
			} catch (Exception ex) {
				ErrorMessages.Add(10, $"Error processing file '{fileName}': {ex.Message}");
				continue;
			}

			if (doc is null || doc.DocumentElement is null) {
				ErrorMessages.Add(11, $"Error processing file '{fileName}': No root element found. No changes were made to this file.");
				continue;
			}

			// If the user didn't explicitly specify whether to include or omit the XML declaration,
			// then we'll preserve what input file does.
			if (!Opt.IncludeXmlDeclarationWasSet && !Opt.OmitXmlDeclarationWasSet) {
				Opt.IncludeXmlDeclaration = doc.OuterXml.StartsWith("<?xml");
			}

			//
			// SORT
			//
			if (Opt.SortAttributes) {
				SortNodeAttrs(doc.DocumentElement);
			}
			if (Opt.SortNodes) {
				SortNodes(doc.DocumentElement);
			}

			//
			// OUTPUT
			//
			Encoding encoding;
			string eol, indentation;

			if (Opt.EncodingWasSet && Opt.EolWasSet && Opt.IndentationWasSet) {
				encoding = EncodingHelper.ConvertEncoding(Opt.Encoding);
				eol = EolHelper.ConvertEOL(Opt.Eol);
				indentation = IndentationHelper.ConvertWhitespace(Opt.Indentation);
			} else {
				using var stream = File.OpenRead(fileName);
				encoding = Opt.EncodingWasSet
					? EncodingHelper.ConvertEncoding(Opt.Encoding)
					: EncodingHelper.DetectEncoding(stream) ?? Encoding.UTF8;
				eol = Opt.EolWasSet
					? EolHelper.ConvertEOL(Opt.Eol)
					: EolHelper.DetectEol(stream) ?? Environment.NewLine;
				indentation = Opt.IndentationWasSet
					? IndentationHelper.ConvertWhitespace(Opt.Indentation)
					: IndentationHelper.DetectIndentation(stream) ?? "  ";
			}

			// We need to create the xmlSettings for each file.
			// Set up XML writer settings based on the specified options.
			var xmlSettings = new XmlWriterSettings() {
				CloseOutput = true,
				Encoding = encoding,
				Indent = Opt.Prettify, //!string.IsNullOrEmpty(indent_chars),
				IndentChars = indentation,
				NewLineChars = eol,
				NewLineHandling = NewLineHandling.Replace,
				NewLineOnAttributes = Opt.AttributesOnNewLine,
				OmitXmlDeclaration = Opt.OmitXmlDeclaration,
			};

			if (Opt.RedirectToStdOut) {
				// Output to console.
				if (Opt.Prettify) {
					var xmlWriter = XmlWriter.Create(Console.Out, xmlSettings);
					doc.Save(xmlWriter);
				} else {
					doc.Save(Console.Out);
				}
			} else {
				// Save to file.
				try {
					if (Opt.Prettify) {
						var xmlWriter = XmlWriter.Create(outFile, xmlSettings);
						doc.Save(xmlWriter);
					} else {
						doc.Save(outFile);
					}
				} catch (Exception ex) {
					ErrorMessages.Add(13, $"Error processing file '{fileName}': Could not save output file.\n{ex.Message}");
					continue;
				}
			}
		}

		if (ErrorMessages.Count > 0) {
			Console.WriteLine("**** ERRORS: ****");
			var exitCode = 0;
			foreach (var err in ErrorMessages) {
				if (exitCode == 0) { exitCode = err.Key; }
				Console.WriteLine(err.Value);
			}
			App.PauseIfNeeded(Opt.Pause, exitCode: 1);
			return exitCode;
		}

		App.PauseIfNeeded(Opt.Pause);

		if (stdinTempFile is not null && File.Exists(stdinTempFile)) {
			File.Delete(stdinTempFile);
		}

		return 0;
	}

	/// <summary>
	/// Sorts the child nodes of the specified XML node according to a predefined comparison logic.
	/// </summary>
	/// <remarks>
	/// This method recursively sorts all descendant nodes of the specified node. The sorting is applied
	/// only if the sorting option is enabled and the node has child nodes. The order of child nodes is determined by a
	/// custom comparison delegate (`SortDelegate`). The method modifies the structure of the XML document in place.
	/// </remarks>
	/// <param name="node">The XML node whose child nodes will be recursively sorted. Cannot be null.</param>
	private void SortNodes( XmlNode node )
	{
		if (node is null || node.ChildNodes is null || node.ChildNodes.Count == 0) { return; }
		if (!Opt.SortNodes) { return; }

		var childCount = node.ChildNodes.Count;

		// Go down to the furthest child and start there first!
		// This is so we can include child nodes as a string in the current node's sort,
		// to break matching tag names (all within SortDelegate).
		for (var i = 0; i < childCount; i++) {
			if (node.ChildNodes[i] is XmlNode childNode && childNode is not null) {
				SortNodes(childNode);
			}
		}

		// Remove the node's children, sort them, then re-add them.
		var sortedNodes = new List<XmlNode>(node.ChildNodes.Count);
		for (var i = childCount - 1; i >= 0; i--) {
			if (node.ChildNodes[i] is XmlNode childNode && childNode is not null) {
				sortedNodes.Add(childNode);
				node.RemoveChild(childNode);
			}
		}
		sortedNodes.Sort(SortDelegate);
		for (var i = 0; i < sortedNodes.Count; i++) {
			node.AppendChild(sortedNodes[i]);
		}
	}

	static string ConvertToXmlString( XmlNode node )
	{
		using var stringWriter = new StringWriter();
		using var xmlTextWriter = new XmlTextWriter(stringWriter) {
			Formatting = Formatting.None,
			Indentation = 0,
		};
		node.WriteTo(xmlTextWriter);
		xmlTextWriter.Flush();
		return stringWriter.GetStringBuilder().ToString();
	}

	private int SortDelegate( XmlNode a, XmlNode b )
	{
		var result = string.Compare(a.Name, b.Name, Opt.NodeStringComparison);
		if (result == 0) {
			if (Opt.SortChildlessNodesFirst) {
				if (a.ChildNodes.Count == 0 ^ b.ChildNodes.Count == 0) {
					// One of the nodes has child nodes, and the other doesn't..
					// the one without child nodes should be sorted first.
					return a.ChildNodes.Count == 0
					  ? -1
					  : 1;
				}
			}

			// > We should be able to simply convert the entire node to a string and compare that,
			//   to break ties when the tag names match, instead of going down into the attributes and child nodes separately.
			var aNode = ConvertToXmlString(a);
			var bNode = ConvertToXmlString(b);
			return aNode.CompareTo(bNode, Opt.NodeStringComparison);
		}

		return result;
	}

	private void SortNodeAttrs( XmlNode node )
	{
		// > No need to check for null node, since this is only called on a node that exists in the document.
		// > No need to check that Opt.SortAttributes is true, since this is only called when that option is enabled.

		// Go down to the furthest child and start there first, to be consistent with the node sorting (see SortNodes).
		// Sort the children's attributes.
		if (node.ChildNodes is XmlNodeList childNodes && childNodes is not null && childNodes.Count > 0) {
			for (var i = 0; i < childNodes.Count; i++) {
				if (childNodes[i] is XmlNode childNode && childNode is not null) {
					SortNodeAttrs(childNode);
				}
			}
		}

		// Remove, sort, then re-add the node's attributes.
		if (node.Attributes is XmlAttributeCollection nodeAttrs && nodeAttrs is not null && nodeAttrs.Count > 1) {
			var sortedAttrs = new List<XmlAttribute>(nodeAttrs.Count);

			for (var i = nodeAttrs.Count - 1; i >= 0; i--) {
				sortedAttrs.Add(nodeAttrs[i]);
				nodeAttrs.RemoveAt(i);
			}

			sortedAttrs.Sort(delegate ( XmlAttribute a, XmlAttribute b )
			{
				var result = string.Compare(a.Name, b.Name, Opt.AttributeStringComparison);
				if (result == 0) {
					return string.Compare(a.Value, b.Value, Opt.AttributeStringComparison);
				} else if (!string.IsNullOrEmpty(Opt.PrimarySortAttribute)) {
					// If a primary_attr is specified, it is always made the first attribute!
					if (a.Name.Equals(Opt.PrimarySortAttribute, Opt.AttributeStringComparison)) {
						return -1;
					} else if (b.Name.Equals(Opt.PrimarySortAttribute, Opt.AttributeStringComparison)) {
						return 1;
					}
				}
				return result;
			});

			for (var i = 0; i < sortedAttrs.Count; i++) {
				nodeAttrs.Append(sortedAttrs[i]);
			}
		}
	}

	private void ShowExamples(bool showHeader = true)
	{
		App.ShowHeader(includeDescription: false, includeBuildInfo: false);

		Console.WriteLine("EXAMPLES:");
		Console.WriteLine("---------");

		Console.WriteLine(@"
> type sample.xml
  <?xml version=""1.0"" encoding=""utf-8"" ?><root><node2 name=""abc"" value=""two""/><node value=""one"" name=""xyz""/></root>

> sortxml sample.xml
  <?xml version=""1.0"" encoding=""utf-8""?>
  <root>
      <node name=""xyz"" value=""one"" />
      <node2 name=""abc"" value=""two"" />
  </root>

> sortxml sample.xml -!pretty
  <?xml version=""1.0"" encoding=""utf-8""?><root><node name=""xyz"" value=""one"" /><node2 name=""abc"" value=""two"" /></root>

> sortxml sample.xml -primary-attribute value
  <?xml version=""1.0"" encoding=""utf-8""?>
  <root>
      <node value=""one"" name=""xyz"" />
      <node2 value=""two"" name=""abc"" />
  </root>

> sortxml sample.xml -indentation ' '
  <?xml version=""1.0"" encoding=""utf-8""?>
  <root>
   <node name=""xyz"" value=""one"" />
   <node2 name=""abc"" value=""two"" />
  </root>

> sortxml sample.xml -indentation ' ' -attributes-on-new-line
  <?xml version=""1.0"" encoding=""utf-8""?>
  <root>
   <node
    name=""xyz""
    value=""one"" />
   <node2
    name=""abc""
    value=""two"" />
  </root>
");
	}
}
