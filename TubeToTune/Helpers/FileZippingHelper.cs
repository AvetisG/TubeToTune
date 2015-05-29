using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Ionic.Zip;

namespace TubeToTune.Helpers
{
	public class FileZippingHelper
	{		
		public static string ZipConvertedAudioFiles(IEnumerable<string> convertedAudioFilenames)
		{
			var zippedFileName = string.Empty;

			try
			{
				zippedFileName = FileNameGenerationHelper.GenerateFilename();
				var zip = new ZipFile(Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data"), zippedFileName));
				zip.AddFiles(convertedAudioFilenames.Select(caf => Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data"), caf)), false, "");
				zip.Save();
				zip.Dispose();
			}
			catch (Exception e)
			{
				throw new ZipException(e.Message);
			}

			return zippedFileName;
		}
	}
}