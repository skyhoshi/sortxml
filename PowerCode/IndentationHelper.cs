/*!
	Copyright (C) 2008-2026 Kody Brown (kody@bricksoft.com).

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

namespace PowerCode;

using System;
using System.IO;
using System.Text;

internal static class IndentationHelper
{
	public static string ConvertWhitespace( string text )
	  => text.ToLower().Replace("\\t", "\t").Replace("\\s", " ").Replace("\\w", " ");

	public static string RemoveOuterQuotes( string text )
	  => (text.StartsWith('"') && text.EndsWith('"')) || (text.StartsWith('\'') && text.EndsWith('\''))
		? (text = text[1..^1])
		: text;

	public static string? DetectIndentation( string fileName, string? defaultIndentation = null )
	{
		using var stream = File.OpenRead(fileName);
		return DetectIndentation(stream, defaultIndentation);
	}

	public static string? DetectIndentation( this Stream stream, string? defaultIndentation = null )
	{
		if (!stream.CanSeek || !stream.CanRead) {
			throw new Exception("DetectIndentation() requires a seekable and readable Stream");
		}

		stream.Position = 0;

		using var reader = new StreamReader(
			stream,
			Encoding.UTF8,
			detectEncodingFromByteOrderMarks: true,
			bufferSize: 4096,
			leaveOpen: true
		);

		string? firstIndentationFound = null;

		while (reader.ReadLine() is { } line) {
			if (line.Length == 0) {
				continue;
			}

			var i = 0;

			while (i < line.Length && (line[i] == ' ' || line[i] == '\t')) {
				i++;
			}

			if (i == 0) {
				continue;
			}

			firstIndentationFound = line[..i];
			break;
		}

		stream.Position = 0;

		return firstIndentationFound ?? defaultIndentation;
	}
}
