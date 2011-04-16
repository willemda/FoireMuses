using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoireMuses.Core.Interfaces
{
	public interface IConverter
	{
		IList<string> Convert(string inputFilePath, string outputFilePath);
	}
}
