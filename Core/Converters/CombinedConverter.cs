using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core.Utils;
using System.IO;

namespace FoireMuses.Core.Converters
{
	public class CombinedConverter : IConverter
	{
		private readonly IConverter thePreConverter;
		private readonly IConverter thePostConverter;

		public CombinedConverter(IConverter aPreConverter, IConverter aPostConverter){
			thePreConverter = aPreConverter;
			thePostConverter = aPostConverter;
		}

		public IList<string> Convert(string inputFilePath, string outputFilePath)
		{
			using (TemporaryFile temp = new TemporaryFile(".tmp",false))
			{
				// generate preConversion file and gives path to this file
				string generated = thePreConverter.Convert(inputFilePath, temp.Path).FirstOrDefault();
				if (generated == null)
					throw new Exception("Unable to pre-convert the file");
				// we pass the preGenerated file path and get the result paths
				IList<string> paths = thePostConverter.Convert(generated, outputFilePath);
				//deleted pre generated file which is useless
				File.Delete(generated);
				//return paths
				return paths;
			}
		}

	}
}
