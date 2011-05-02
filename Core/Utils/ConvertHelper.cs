using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Dream;

namespace FoireMuses.Core.Utils
{
	public class ConvertHelper
	{
		
		//general
		public static string PathToTemp = @"G:\Temp\";
		//public static string LilyPondCommand = @"G:\Program Files\LilyPond\usr\bin\lilypond.exe";
        public static string LilyPondCommand = @"C:\Program Files (x86)\LilyPond\usr\bin\lilypond.exe";

		//convert to lily from xml
		public static MimeType LilyPond = new MimeType("text/x-lilypond");
		//public static string ToLyCommand = @"G:\Program Files\LilyPond\usr\bin\musicxml2ly.py";
        public static string ToLyCommand = @"C:\Program Files (x86)\LilyPond\usr\bin\musicxml2ly.py";
		public static string ToLyArgs = "-o {0} {1}";
		public static string ToLyExpectedFile = ".ly";

		//make .ps file from xml
		public static string ToPsArgs = "-fps -o {0} {1}";
		public static string ToPsExpectedFile = ".ps";

		//convert to pdf from lily
		public static string ToPdfArgs = "--pdf -o {0} {1}";
		public static string ToPdfExpectedFile = ".pdf";

		//convert to png from lily
		//public static string ToPngCommand = @"G:\Program Files\ImageMagick-6.6.9-Q16\convert.exe";
        public static string ToPngCommand = @"C:\Program Files\ImageMagick-6.6.9-Q16\convert.exe";
		public static string ToPngArgs = "\"{1}\" \"{0}.png\"";
		public static string ToPngExpectedFile = ".png";

		//convert to midi from midi
		public static MimeType Midi = new MimeType("application/x-midi");
		public static string ToMidiArgs = "-o {0} {1}";
		public static string ToMidiExpectedFile = ".mid";
	}
}
