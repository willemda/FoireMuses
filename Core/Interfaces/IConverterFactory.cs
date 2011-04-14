using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Dream;

namespace FoireMuses.Core.Interfaces
{
	public interface IConverterFactory
	{
		IConverter GetConverter(MimeType type);
	}
}
