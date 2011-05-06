//  
//  TheSettings.cs
//  
//  Author:
//       danny <${AuthorEmail}>
// 
//  Copyright (c) 2011 danny
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using FoireMuses.Core.Interfaces;
using MindTouch.Xml;

namespace FoireMuses.Core.Controllers
{
	internal class XmlSettingsController : ISettingsController
	{
		private readonly XDoc theInstanceSettings;

		public XmlSettingsController(XDoc instanceSettings)
		{
			theInstanceSettings = instanceSettings;
		}

		public string Host { get { return theInstanceSettings["@host"].AsText ?? "localhost"; } }
		public int Port { get { return theInstanceSettings["@port"].AsInt ?? 5984; } }
		public string Username { get { return theInstanceSettings["@username"].AsText ?? String.Empty; } }
		public string Password { get { return theInstanceSettings["@password"].AsText ?? String.Empty; } }
		public string DatabaseName { get { return theInstanceSettings["@databaseName"].AsText ?? "musicdatabasexml"; } }
		public string LilyPondCommand { get { return theInstanceSettings["convertersSettings/@lilyPondCommand"].AsText; } }
		public string ToLyCommand { get { return theInstanceSettings["convertersSettings/@toLyCommand"].AsText; } }
		public string ToLyArgs { get { return theInstanceSettings["convertersSettings/@toLyArgs"].AsText; } }
		public string ToLyExpectedFile { get { return theInstanceSettings["convertersSettings/@toLyExpectedFile"].AsText; } }
		public string ToPsArgs { get { return theInstanceSettings["convertersSettings/@toPsArgs"].AsText; } }
		public string ToPsExpectedFile { get { return theInstanceSettings["convertersSettings/@toPsExpectedFile"].AsText; } }
		public string ToPdfArgs { get { return theInstanceSettings["convertersSettings/@toPdfArgs"].AsText; } }
		public string ToPdfExpectedFile { get { return theInstanceSettings["convertersSettings/@toPdfExpectedFile"].AsText; } }
		public string ToPngCommand { get { return theInstanceSettings["convertersSettings/@toPngCommand"].AsText; } }
		public string ToPngArgs { get { return theInstanceSettings["convertersSettings/@toPngArgs"].AsText; } }
		public string ToPngExpectedFile { get { return theInstanceSettings["convertersSettings/@toPngExpectedFile"].AsText; } }
		public string ToMidiArgs { get { return theInstanceSettings["convertersSettings/@toMidiArgs"].AsText; } }
		public string ToMidiExpectedFile { get { return theInstanceSettings["convertersSettings/@toMidiExpectedFile"].AsText; } }
	}
}

