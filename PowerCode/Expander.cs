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

internal static class Expander
{
	public static string ExpandAll( string text, DateTime? dateTime = null )
	{
		if (dateTime is not null) {
			text = ExpandDates(text, dateTime.Value);
		}
		var expandedEnvars = ExpandEnvars(text);
		return expandedEnvars;
	}

	public static string ExpandEnvars( string text )
	{
		if (string.IsNullOrWhiteSpace(text)) {
			return text;
		}

		// Just in case.
		const int MaxIterations = 50;
		var iterationCount = 0;
		bool changed;

		do {
			changed = false;

			if (text.Contains('%')) {
				// Support "%VAR%" syntax
				if (GetEnvironmentVariable(text, "%", "%", out var fullVariable, out var variableValue)) {
					text = text.Replace(fullVariable, variableValue, StringComparison.OrdinalIgnoreCase);
					changed = true;
				}
			}
			if (text.Contains("$(")) {
				// Support "$(VAR)" syntax
				if (GetEnvironmentVariable(text, "$(", ")", out var fullVariable, out var variableValue)) {
					text = text.Replace(fullVariable, variableValue, StringComparison.OrdinalIgnoreCase);
					changed = true;
				}
			}
			if (text.Contains("${env:")) {
				// Support "${env:VAR}" syntax
				if (GetEnvironmentVariable(text, "${env:", "}", out var fullVariable, out var variableValue)) {
					text = text.Replace(fullVariable, variableValue, StringComparison.OrdinalIgnoreCase);
					changed = true;
				}
			}
			if (text.Contains("${")) {
				// Support "${VAR}" syntax
				if (GetEnvironmentVariable(text, "${", "}", out var fullVariable, out var variableValue)) {
					text = text.Replace(fullVariable, variableValue, StringComparison.OrdinalIgnoreCase);
					changed = true;
				}
			}

			if (++iterationCount >= MaxIterations) {
				break;
			}
		} while (changed);

		return text;
	}

	private static bool GetEnvironmentVariable( string text, string varPrefix, string varSuffix, out string fullVariable, out string variableValue )
	{
		fullVariable = string.Empty;
		variableValue = string.Empty;

		if (string.IsNullOrWhiteSpace(text)) {
			return false;
		}
		if (string.IsNullOrWhiteSpace(varPrefix) || string.IsNullOrWhiteSpace(varSuffix)) {
			return false;
		}

		var start = text.IndexOf(varPrefix, StringComparison.OrdinalIgnoreCase);
		if (start == -1) {
			return false;
		}

		var end = text.IndexOf(varSuffix, start + 1);
		if (end < start) {
			return false;
		}

		fullVariable = text[start..(end + varSuffix.Length)]; // e.g. '<varPrefix>VAR_NAME<varSuffix>'

		var varNameOnly = fullVariable[varPrefix.Length..^varSuffix.Length]; // e.g. 'VAR_NAME'
		if (string.IsNullOrEmpty(varNameOnly)) {
			return false;
		}

		switch (varNameOnly.ToUpperInvariant()) {
			case "HOST":
			case "HOSTNAME":
			case "COMPUTERNAME":
				variableValue = Environment.MachineName;
				return true;
			case "HOME":
			case "USERPROFILE":
				variableValue = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
				return true;
		}

		var envValue = Environment.GetEnvironmentVariable(varNameOnly);
		if (!string.IsNullOrEmpty(envValue)) {
			variableValue = envValue;
			return true;
		}
		return false;
	}

	public static string ExpandDates( string text, DateTime dateTime )
	{
		if (string.IsNullOrWhiteSpace(text)) {
			return text;
		}

		// Just in case.
		const int MaxIterations = 50;
		var iterationCount = 0;
		bool changed;

		do {
			changed = false;

			if (text.Contains("%date:", StringComparison.OrdinalIgnoreCase)) {
				// Support "%date:<format>%" syntax
				if (GetDateVariable(text, "%date:", "%", dateTime, out var fullVariable, out var variableValue)) {
					text = text.Replace(fullVariable, variableValue, StringComparison.OrdinalIgnoreCase);
					changed = true;
				}
			}
			if (text.Contains("$(date:", StringComparison.OrdinalIgnoreCase)) {
				// Support "$(date:<format>)" syntax
				if (GetDateVariable(text, "$(date:", ")", dateTime, out var fullVariable, out var variableValue)) {
					text = text.Replace(fullVariable, variableValue, StringComparison.OrdinalIgnoreCase);
					changed = true;
				}
			}
			if (text.Contains("${date:", StringComparison.OrdinalIgnoreCase)) {
				// Support "${date:<format>}" syntax
				if (GetDateVariable(text, "${date:", "}", dateTime, out var fullVariable, out var variableValue)) {
					text = text.Replace(fullVariable, variableValue, StringComparison.OrdinalIgnoreCase);
					changed = true;
				}
			}

			if (++iterationCount >= MaxIterations) {
				break;
			}
		} while (changed);

		return text;
	}

	private static bool GetDateVariable( string text, string varPrefix, string varSuffix, DateTime dateTime, out string fullVariable, out string variableValue )
	{
		fullVariable = string.Empty;
		variableValue = string.Empty;

		if (string.IsNullOrWhiteSpace(text)) {
			return false;
		}
		if (string.IsNullOrWhiteSpace(varPrefix) || string.IsNullOrWhiteSpace(varSuffix)) {
			return false;
		}

		var start = text.IndexOf(varPrefix, StringComparison.OrdinalIgnoreCase);
		if (start == -1) {
			return false;
		}

		var end = text.IndexOf(varSuffix, start + 1);
		if (end < start) {
			return false;
		}

		fullVariable = text[start..(end + varSuffix.Length)]; // e.g. '<varPrefix>yyyyMMdd<varSuffix>'

		var varNameOnly = fullVariable[varPrefix.Length..^varSuffix.Length]; // e.g. 'yyyyMMdd'
		if (string.IsNullOrEmpty(varNameOnly)) {
			return false;
		}
		//if (!ContainsValidDateFormat(varNameOnly)) {
		//  return false;
		//}

		try {
			variableValue = dateTime.ToString(varNameOnly);
		} catch (FormatException) {
			return false;
		}

		return true;
	}

	//private static bool ContainsValidDateFormat( string format )
	//{
	//  // Check for known valid patterns or validate against DateTime's parsing rules
	//  var knownFormats = new[] { "yyyy", "MM", "dd", "HH", "mm", "ss" };
	//  return knownFormats.Any(f => format.Contains(f));
	//}
}
