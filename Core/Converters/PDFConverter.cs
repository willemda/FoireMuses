using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using System.IO;

namespace FoireMuses.Core.Converters
{
	public class PDFConverter : Converter, IConverter
	{

		public PDFConverter(string command, string args, string expectedName)
			: base(command, args, expectedName)
		{
		}

		public override IList<string> Convert(string inputFile, string outputFile)
		{
			IList<string> result = base.Convert(inputFile, outputFile);
			// it generates ps file, delete those.
			try
			{
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
