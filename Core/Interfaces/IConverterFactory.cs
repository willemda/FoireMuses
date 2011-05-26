using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Dream;

namespace FoireMuses.Core.Interfaces
{
	public interface IConverterFactory
	{
		/// <summary>
		/// Retrieve a Converter for MusicXml by specifying required output MimeType
		/// </summary>
		/// <param name="type">Output MimeType</param>
		/// <returns>Converter if exists, null otherwize</returns>
		IConverter GetConverter(MimeType type);
	}
}
