using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;

namespace Editor.imec.Roslyn
{
	[InitializeOnLoad]
	public class RulesetConfig : AssetPostprocessor
	{
		public RulesetConfig()
		{
			OnGeneratedCSProjectFiles();
		}

		private static void OnGeneratedCSProjectFiles()
		{
			string dir = Directory.GetCurrentDirectory();
			foreach (var file in Directory.GetFiles(dir, "*.csproj"))
				FixProject(file);
		}

		private static void FixProject(string file)
		{
			string text = File.ReadAllText(file);
			const string find = "<PropertyGroup>";
			const string replace = find + "\n<CodeAnalysisRuleSet>.\\global.ruleset</CodeAnalysisRuleSet>";
			if (text.Contains("CodeAnalysisRuleSet") || !text.Contains(find))
			{
				return;
			}

			Regex regex = new Regex(Regex.Escape(find));
			string newText = regex.Replace(text, replace, 1);
			File.WriteAllText(file, newText);
		}
	}
}
