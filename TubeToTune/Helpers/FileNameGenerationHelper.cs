using System.IO;
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
	}
}