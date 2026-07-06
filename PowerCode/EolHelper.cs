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

internal static class EolHelper
{
	public static string ConvertEOL( string text )
	  => text.ToLower().Replace("\\r", "\r").Replace("\\n", "\n").Replace("os", Environment.NewLine).Replace("default", Environment.NewLine);

	public static string? DetectEol( string fileName, string? defaultEol = null )
	{
		using var stream = File.OpenRead(fileName);
		return DetectEol(stream, defaultEol);
	}

	public static string? DetectEol( this Stream stream, string? defaultEol = null )
	{
		if (!stream.CanSeek || !stream.CanRead) {
			throw new Exception("DetectEol() requires a seekable and readable Stream");
		}

		stream.Position = 0;

		const int BUF_SIZE = 4096;
		var buffer = new byte[BUF_SIZE];
		var previousWasCR = false;

		while (true) {
			var count = stream.Read(buffer, 0, buffer.Length);

			if (count == 0) {
				break;
			}

			for (var i = 0; i < count; i++) {
				var b = buffer[i];

				if (previousWasCR) {
					if (b == '\n') {
						stream.Position = 0;
						return "\r\n";
					}

					stream.Position = 0;
					return "\r";
				}

				if (b == '\r') {
					previousWasCR = true;
				} else if (b == '\n') {
					stream.Position = 0;
					return "\n";
				}
			}
		}

		// File ended with lone CR
		if (previousWasCR) {
			stream.Position = 0;
			return "\r";
		}

		stream.Position = 0;
		return defaultEol;
	}
}
