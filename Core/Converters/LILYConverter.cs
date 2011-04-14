using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using MindTouch.Tasking;
using System.IO;

namespace FoireMuses.Core.Converters
{
	public class LILYConverter : IConverter
	{
		public IList<string> Convert(string filePath)
		{
            Async.ExecuteProcess("python.exe", @"C:\Program Files (x86)\LilyPond\usr\bin\musicxml2ly " + filePath, Stream.Null, Stream.Null, Stream.Null, new Result<int>()).Wait();
			IList<string> convertedFilesPaths = new List<string>();
			if (File.Exists(filePath + ".ly"))
			{
				convertedFilesPaths.Add(filePath + ".ly");
			}
			return convertedFilesPaths;
		}
	}
}
