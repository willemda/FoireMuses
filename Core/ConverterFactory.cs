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
using FoireMuses.Core.Utils;

namespace FoireMuses.Core
{
	public class MimeTypeHelper
	{

	}

	public class ConverterFactory : IConverterFactory
	{
		IDictionary<MimeType,IConverter> Converters = new Dictionary<MimeType,IConverter>();

		public ConverterFactory()
		{
			IConverter XmlToLilyPoundConverter = new Converter(ConvertHelper.ToLyCommand, ConvertHelper.ToLyArgs, ConvertHelper.ToLyExpectedFile);
			IConverter LilyPoundToPdfConverter = new PDFConverter(ConvertHelper.LilyPondCommand, ConvertHelper.ToPdfArgs, ConvertHelper.ToPdfExpectedFile);
			IConverter LilyPoundToPostScriptConverter = new Converter(ConvertHelper.LilyPondCommand, ConvertHelper.ToPsArgs, ConvertHelper.ToPsExpectedFile);
			IConverter PostScriptToPngConverter = new Converter(ConvertHelper.ToPngCommand, ConvertHelper.ToPngArgs, ConvertHelper.ToPngExpectedFile);
			IConverter XmlToPostScriptConverter = new AndConverter(XmlToLilyPoundConverter, LilyPoundToPostScriptConverter);
			IConverter XmlToPngConverter = new AndConverter(XmlToPostScriptConverter, PostScriptToPngConverter);
			IConverter XmlToPdfConverter = new AndConverter(XmlToLilyPoundConverter, LilyPoundToPdfConverter);
			IConverter LilyPoundToMidiConverter = new MIDIConverter(ConvertHelper.LilyPondCommand, ConvertHelper.ToMidiArgs, ConvertHelper.ToMidiExpectedFile);
			IConverter XmlToMidiConverter = new AndConverter(XmlToLilyPoundConverter, LilyPoundToMidiConverter);
			Converters.Add(ConvertHelper.LilyPond, XmlToLilyPoundConverter);
			Converters.Add(MimeType.PDF, XmlToPdfConverter);
			Converters.Add(MimeType.PNG, XmlToPngConverter);
			Converters.Add(ConvertHelper.Midi, XmlToMidiConverter);
		}
		
		public IConverter GetConverter(MimeType type)
		{
			IConverter converter;
			Converters.TryGetValue(type, out converter);
			return converter;
		}
	}
}
