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

		public ConverterFactory(ISettingsController aSettingsController)
		{
			IConverter XmlToLilyPoundConverter = new Converter(aSettingsController.ToLyCommand, aSettingsController.ToLyArgs, aSettingsController.ToLyExpectedFile);
			IConverter LilyPoundToPdfConverter = new PDFConverter(aSettingsController.LilyPondCommand, aSettingsController.ToPdfArgs, aSettingsController.ToPdfExpectedFile);
			IConverter LilyPoundToPostScriptConverter = new Converter(aSettingsController.LilyPondCommand, aSettingsController.ToPsArgs, aSettingsController.ToPsExpectedFile);
			IConverter PostScriptToPngConverter = new Converter(aSettingsController.ToPngCommand, aSettingsController.ToPngArgs, aSettingsController.ToPngExpectedFile);
			IConverter XmlToPostScriptConverter = new AndConverter(XmlToLilyPoundConverter, LilyPoundToPostScriptConverter);
			IConverter XmlToPngConverter = new AndConverter(XmlToPostScriptConverter, PostScriptToPngConverter);
			IConverter XmlToPdfConverter = new AndConverter(XmlToLilyPoundConverter, LilyPoundToPdfConverter);
			IConverter LilyPoundToMidiConverter = new MIDIConverter(aSettingsController.LilyPondCommand, aSettingsController.ToMidiArgs, aSettingsController.ToMidiExpectedFile);
			IConverter XmlToMidiConverter = new AndConverter(XmlToLilyPoundConverter, LilyPoundToMidiConverter);
			Converters.Add(Constants.LilyPond, XmlToLilyPoundConverter);
			Converters.Add(MimeType.PDF, XmlToPdfConverter);
			Converters.Add(MimeType.PNG, XmlToPngConverter);
			Converters.Add(Constants.Midi, XmlToMidiConverter);
		}
		
		public IConverter GetConverter(MimeType type)
		{
			IConverter converter;
			Converters.TryGetValue(type, out converter);
			return converter;
		}
	}
}
