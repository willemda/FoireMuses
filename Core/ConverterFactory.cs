using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Xml;
using Autofac.Builder;
using MindTouch.Dream;
using Autofac;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core.Converters;
using System.Net.Mime;

namespace FoireMuses.Core
{
	public class ConverterFactory : IConverterFactory
	{
		public IConverter PNGConverter { get; private set; }
		public IConverter PDFConverter { get; private set; }
		public IConverter LILYConverter { get; private set; }
		public IConverter MIDIConverter { get; private set; }

		public ConverterFactory()
		{
			PNGConverter = new PNGConverter();
			PDFConverter = new PDFConverter();
			LILYConverter = new LILYConverter();
			MIDIConverter = new MIDIConverter();
		}

		public IConverter GetConverter(MimeType type)
		{
			ContentType ct = new ContentType("text/lily");
			switch (type.MainType)
			{
				case "PDF":
					return PDFConverter;
				case "PNG":
					return PNGConverter;
				case "LILY":
					return LILYConverter;
				case "MIDI":
					return MIDIConverter;
			}
			throw new Exception();
		}
	}
}
