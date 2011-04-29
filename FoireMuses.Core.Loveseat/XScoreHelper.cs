using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MusicXml;

namespace FoireMuses.MusicXMLImport
{
	public static class XScoreHelper
	{
		/// <summary>
		/// Needs some documentation
		/// </summary>
		/// <param name="doc"></param>
		/// <returns></returns>
		public static string GetCodageParIntervalle(this XScore doc)
		{
			StringBuilder code2 = new StringBuilder();
			Pitch lastPitch = null;

			foreach (Part part in doc.Parts)
			{
				foreach (Measure measure in part.Measures)
				{
					foreach (Note note in measure.Notes)
					{
						Pitch pitch = note.Pitch;
						if ((pitch != null) && (lastPitch != null))
						{
							int value = GetDelta(lastPitch, pitch);

							code2.Append(value);
							code2.Append(" ");
						}
						if(pitch!=null)
							lastPitch = pitch;
					}
				}
				break;
			}

			return code2.ToString();
		}

		/// <summary>
		///  Needs some documentation
		/// </summary>
		/// <param name="doc"></param>
		/// <returns></returns>
		public static string GetCodageMelodiqueRISM(this XScore doc)
		{
			StringBuilder code1 = new StringBuilder();
			Pitch lastPitch = null;

			foreach (Part part in doc.Parts)
			{
				foreach (Measure measure in part.Measures)
				{
					foreach (Note note in measure.Notes)
					{
						Pitch pitch = note.Pitch;
						if (pitch == null)
							continue;

						if (lastPitch == null)
						{
							code1.Append("0 ");
							lastPitch = pitch;
						}
						else
						{
							int value = GetDelta(lastPitch, pitch);
							code1.Append(value);
							code1.Append(" ");
						}
					}
				}
				break;
			}

			return code1.ToString();
		}

		/// <summary>
		/// Needs some documentation
		/// </summary>
		/// <param name="doc"></param>
		/// <returns></returns>
		public static string GetText(this XScore doc)
		{
			StringBuilder text = new StringBuilder();

			foreach (Part part in doc.Parts)
			{
				foreach (Measure measure in part.Measures)
				{
					foreach (Note note in measure.Notes)
					{
						Lyric lyric = note.Lyric;
						if (lyric == null)
							continue;

						switch (lyric.Syllabic)
						{
							case Syllabic.End:
							case Syllabic.Single:
								text.AppendFormat("{0} ", lyric.Text);
								break;
							case Syllabic.Middle:
							case Syllabic.Begin:
								text.Append(lyric.Text);
								break;
						}
					}
				}
			}

			return text.ToString();
		}

		private static int GetDelta(Pitch lastPitch, Pitch pitch)
		{
			return GetStepValue(pitch) - GetStepValue(lastPitch);
		}
		private static int GetStepValue(Pitch aStep)
		{
			int value = aStep.Alter;

			value += aStep.Octave * 12;

			switch (aStep.Step)
			{
				case 'C':
					return value;
				case 'D':
					return value + 2;
				case 'E':
					return value + 4;
				case 'F':
					return value + 5;
				case 'G':
					return value + 7;
				case 'A':
					return value + 9;
				case 'B':
					return value + 11;
				default:
					throw new Exception("invalid pitch value");
			}
		}
	}
}
