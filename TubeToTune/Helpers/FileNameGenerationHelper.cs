using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TubeToTune.Helpers
{
	public class FileNameGenerationHelper
	{
		public static string RemoveIllegalPathCharacters(string path)
		{
			string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
			var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
			return r.Replace(path, "").Replace("&", "and");
		}

		public static string GenerateFilename()
		{
			var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			var random = new Random();
			var result = new string(
				Enumerable.Repeat(chars, 13)
					.Select(s => s[random.Next(s.Length)])
					.ToArray());

			return result + ".zip";
		}
	}
}