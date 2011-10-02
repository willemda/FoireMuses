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
	public class ConverterFactory : IConverterFactory
	{
		private readonly IDictionary<MimeType,IConverter> theConverters = new Dictionary<MimeType,IConverter>();

		public ConverterFactory(ISettingsController aSettingsController)
		{
			IConverter musicXmlToLilypound = new Converter(aSettingsController.ToLyCommand, aSettingsController.ToLyArgs, aSettingsController.ToLyExpectedFile);
			IConverter lilypoundToPdf = new PDFConverter(aSettingsController.LilyPondCommand, aSettingsController.ToPdfArgs, aSettingsController.ToPdfExpectedFile);
			IConverter lilypoundToPostscript = new MusicXmlToPsConverter(aSettingsController.LilyPondCommand);
			IConverter postcriptToPng = new Converter(aSettingsController.ToPngCommand, aSettingsController.ToPngArgs, aSettingsController.ToPngExpectedFile);
			IConverter musicXmlToPostscript = new CombinedConverter(musicXmlToLilypound, lilypoundToPostscript);
			IConverter musicXmlToPng = new CombinedConverter(musicXmlToPostscript, postcriptToPng);
			IConverter musicXmlToPdf = new CombinedConverter(musicXmlToLilypound, lilypoundToPdf);
			IConverter lilypoundToMidi = new MIDIConverter(aSettingsController.LilyPondCommand, aSettingsController.ToMidiArgs, aSettingsController.ToMidiExpectedFile);
			IConverter musicXmlToMidi = new CombinedConverter(musicXmlToLilypound, lilypoundToMidi);
			theConverters.Add(Constants.LilyPond, musicXmlToLilypound);
			theConverters.Add(MimeType.PDF, musicXmlToPdf);
			theConverters.Add(MimeType.PNG, musicXmlToPng);
			theConverters.Add(Constants.Midi, musicXmlToMidi);
		}
		
		public IConverter GetConverter(MimeType type)
		{
			IConverter converter;
			theConverters.TryGetValue(type, out converter);
			return converter;
		}
	}
}
