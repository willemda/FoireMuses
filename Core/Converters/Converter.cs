using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using System.IO;

namespace FoireMuses.Core.Converters
{
	public class Converter : IConverter
	{
		private string Command;
		private string Args;
		private string ExpectedFileName;


		public Converter(string command, string args, string expectedName)
		{
			this.Command = command;
			this.Args = args;
			this.ExpectedFileName = expectedName;
		}

		public IList<string> Convert(string inputFilePath, string outputFilePath)
		{
			System.Diagnostics.Process proc = new System.Diagnostics.Process();
			proc.StartInfo.FileName = Command;
			proc.StartInfo.Arguments = String.Format(Args, outputFilePath, inputFilePath);
			proc.StartInfo.UseShellExecute = true;
			proc.Start();
			proc.WaitForExit();

			DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(outputFilePath));
			string fileName = Path.GetFileName(outputFilePath);
			FileInfo[] rgFiles = di.GetFiles(fileName+"*"+ExpectedFileName);
			IList<string> convertedFilesPaths = new List<string>();
			foreach(FileInfo fi in rgFiles)
			{
				convertedFilesPaths.Add(fi.FullName);
			}
			return convertedFilesPaths;
		}

	}
}
