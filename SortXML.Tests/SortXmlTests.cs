namespace SortXML.Tests;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

public class SortXmlTests
{
	static readonly string _sortXmlProject = typeof(SortXmlTests).Assembly
	  .GetCustomAttributes(typeof(System.Reflection.AssemblyMetadataAttribute), false)
	  .Cast<System.Reflection.AssemblyMetadataAttribute>()
	  .First(a => a.Key == "SortXmlProjectPath")
	  .Value!;

	static readonly string _testFilesDir = Path.Combine(AppContext.BaseDirectory, "test_files");

	public static IEnumerable<object[]> TestFilePairs()
	{
		if (!Directory.Exists(_testFilesDir)) {
			yield break;
		}

		foreach (var sortedFile in Directory.GetFiles(_testFilesDir, "*_sorted.xml")) {
			var name = Path.GetFileNameWithoutExtension(sortedFile);
			var baseName = name.Replace("_sorted", "");
			var inputFile = Path.Combine(_testFilesDir, $"{baseName}.xml");

			if (File.Exists(inputFile)) {
				yield return new object[] { baseName, inputFile, sortedFile };
			}
		}
	}

	[Theory]
	[MemberData(nameof(TestFilePairs))]
	public void SortXml_ProducesExpectedOutput( string testName, string inputFile, string expectedFile )
	{
		// sortxml now modifies files in-place, so we copy the input to a temp file first.
		var tempFile = Path.Combine(Path.GetTempPath(), $"sortxml_test_{testName}_{Guid.NewGuid():N}.xml");
		File.Copy(inputFile, tempFile, overwrite: true);

		try {
			var psi = new ProcessStartInfo {
				FileName = "dotnet",
				Arguments = $"run --no-launch-profile --no-build --no-restore --project \"{_sortXmlProject}\" -- \"{tempFile}\"",
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true,
			};

			using var process = Process.Start(psi)!;
			var stderr = process.StandardError.ReadToEnd();
			process.WaitForExit(TimeSpan.FromSeconds(30));

			Assert.True(
			  process.ExitCode == 0,
			  $"sortxml exited with code {process.ExitCode}. stderr: {stderr}");

			var expected = NormalizeXml(File.ReadAllText(expectedFile));
			var actual = NormalizeXml(File.ReadAllText(tempFile));

			Assert.Equal(expected, actual);
		} finally {
			if (File.Exists(tempFile)) {
				File.Delete(tempFile);
			}
		}
	}

	[Theory]
	[MemberData(nameof(TestFilePairs))]
	public void SortXml_OutFile_ProducesExpectedOutput( string testName, string inputFile, string expectedFile )
	{
		// Tests the --out-file option: input file is NOT modified, output goes to a separate file.
		var outFile = Path.Combine(Path.GetTempPath(), $"sortxml_outfile_{testName}_{Guid.NewGuid():N}.xml");

		try {
			var psi = new ProcessStartInfo {
				FileName = "dotnet",
				Arguments = $"run --no-launch-profile --no-build --no-restore --project \"{_sortXmlProject}\" -- \"{inputFile}\" -out-file \"{outFile}\"",
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true,
			};

			using var process = Process.Start(psi)!;
			var stderr = process.StandardError.ReadToEnd();
			process.WaitForExit(TimeSpan.FromSeconds(30));

			Assert.True(
			  process.ExitCode == 0,
			  $"sortxml exited with code {process.ExitCode}. stderr: {stderr}");

			Assert.True(
			  File.Exists(outFile),
			  $"Output file was not created: {outFile}");

			// Verify the output matches expected.
			var expected = NormalizeXml(File.ReadAllText(expectedFile));
			var actual = NormalizeXml(File.ReadAllText(outFile));
			Assert.Equal(expected, actual);

			// Verify the input file was NOT modified.
			var originalInput = File.ReadAllText(inputFile);
			var inputAfter = File.ReadAllText(inputFile);
			Assert.Equal(originalInput, inputAfter);
		} finally {
			if (File.Exists(outFile)) {
				File.Delete(outFile);
			}
		}
	}

	static string NormalizeXml( string text )
	{
		// Strip BOM and normalize line endings for comparison.
		return text
		  .TrimStart('\uFEFF')
		  .ReplaceLineEndings("\n")
		  .TrimEnd();
	}
}
