using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using System.IO;
using System.Text.RegularExpressions;

namespace FoireMuses.Core.Converters
{
	public class MIDIConverter : Converter, IConverter
	{

		public MIDIConverter(string command, string args, string expectedName)
			: base(command, args, expectedName)
		{
		}

		public override IList<string> Convert(string inputFile, string outputFile)
		{
			string lilypoundFile = File.ReadAllText(inputFile);
			lilypoundFile = Regex.Replace(lilypoundFile, @"(?<avant>.*)%\s*" + Regex.Escape(@"\midi") + "(?<apres>.*)", @"${avant}\midi${apres}");
			File.WriteAllText(inputFile,lilypoundFile);
			IList<string> result = base.Convert(inputFile, outputFile);
			// it generates ps and pdf files, delete those.
			try
			{
				File.Delete(outputFile + ".pdf");
				File.Delete(outputFile + ".ps");
			}
			catch (Exception e)
			{
				//not important
			}
			return result;
		}
	}
}
