using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using System.IO;
using FoireMuses.Core.Utils;

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

		public virtual IList<string> Convert(string inputFilePath, string outputFilePath)
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

	public class MusicXmlToPsConverter : IConverter
	{
		private string theLilypondPath;

		public MusicXmlToPsConverter(string lilypondPath)
		{
			theLilypondPath = lilypondPath;
		}

		public IList<string> Convert(string inputFilePath, string outputFilePath)
		{
			using (TemporaryFile tmp = new TemporaryFile())
			{
				System.Diagnostics.Process proc = new System.Diagnostics.Process();
				proc.StartInfo.FileName = theLilypondPath;
				proc.StartInfo.Arguments = String.Format("-fps -o {0} {1}", tmp.Path, inputFilePath);
				proc.StartInfo.UseShellExecute = true;
				proc.Start();
				proc.WaitForExit();

				string outputFileName = tmp.Path + ".ps";
				//if (File.Exists(outputFileName))
				//{
				//    File.Move(outputFileName, outputFilePath);
				//}

				return new[] { outputFileName };
			}
		}
	}
}
