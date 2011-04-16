using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core.Utils;
using System.IO;

namespace FoireMuses.Core.Converters
{
	public class AndConverter : IConverter
	{
		private IConverter preConverter;
		private IConverter postConverter;

		public AndConverter(IConverter aPreConverter, IConverter aPostConverter){
			preConverter = aPreConverter;
			postConverter = aPostConverter;
		}

		public IList<string> Convert(string inputFilePath, string outputFilePath)
		{
			using (TemporaryFile temp = new TemporaryFile())
			{
				// generate preConversion file and gives path to this file
				string generated = preConverter.Convert(inputFilePath, temp.Path).First();
				// we pass the preGenerated file path and get the result paths
				IList<string> paths = postConverter.Convert(generated, outputFilePath);
				//deleted pre generated file which is useless
				File.Delete(generated);
				//return paths
				return paths;
			}
		}

	}
}
