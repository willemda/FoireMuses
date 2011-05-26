//  
//  ScoreController.cs
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
using MindTouch.Tasking;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core.Utils;
using System.Collections.Generic;
using System.Linq;
using MindTouch.Xml;
using MindTouch.Dream;
using System.IO;
using MusicXml;

namespace FoireMuses.Core.Controllers
{
	using Yield = IEnumerator<IYield>;

	public class ScoreController : IScoreController
	{
		private static readonly log4net.ILog theLogger = log4net.LogManager.GetLogger(typeof(ScoreController));

		private readonly IScoreDataMapper theScoreDataMapper;

		public ScoreController(IScoreDataMapper aController)
		{
			ArgCheck.NotNull("aController", aController);
			theScoreDataMapper = aController;
		}

		public Result<SearchResult<IScore>> GetScoresFromSource(string aSourceId, int offset, int max, Result<SearchResult<IScore>> aResult)
		{
			ArgCheck.NotNull("aSourceId", aSourceId);
			Coroutine.Invoke(GetScoresFromSourceHelper, offset, max, aSourceId, new Result<SearchResult<IScore>>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IScore> Insert(IScore aScore, Result<IScore> aResult)
		{
			ArgCheck.NotNull("aScore", aScore);
			ArgCheck.NotNull("aResult", aResult);

			Coroutine.Invoke(CreateHelper, aScore, new Result<IScore>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IScore> Retrieve(string aScoreId, Result<IScore> aResult)
		{
			ArgCheck.NotNull("aScoreId",aScoreId);
			ArgCheck.NotNull("aResult", aResult);

			theScoreDataMapper.Retrieve(aScoreId, new Result<IScore>()).WhenDone(
					aResult.Return,
					aResult.Throw
					);
			return aResult;
		}

		public bool HasAuthorization(IScore aScore)
		{
			ArgCheck.NotNull("aScore", aScore);
			return Context.Current.User != null && (IsCreator(aScore) || IsCollaborator(aScore));
		}

		public Result<IScore> Update(string aScoreId, string aScoreRev, IScore aDoc, Result<IScore> aResult)
		{
			Coroutine.Invoke(UpdateHelper, aScoreId, aScoreRev, aDoc, new Result<IScore>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<bool> Delete(string id, string rev, Result<bool> aResult)
		{
			theScoreDataMapper.Delete(id, rev, new Result<bool>()).WhenDone(
					a =>
					{
						aResult.Return(a);
					},
					aResult.Throw
					);
			return aResult;
		}

		public Result<SearchResult<IScore>> GetAll(int offset, int max, Result<SearchResult<IScore>> aResult)
		{
			theScoreDataMapper.GetAll(offset, max, new Result<SearchResult<IScore>>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public IScore FromJson(string aJson)
		{
			return theScoreDataMapper.FromJson(aJson);
		}

		public string ToJson(IScore aScore)
		{
			return theScoreDataMapper.ToJson(aScore);
		}

		public IScore CreateNew()
		{
			return theScoreDataMapper.CreateNew();
		}

		public Result<bool> Exists(string id, Result<bool> aResult)
		{
			this.Retrieve(id, new Result<IScore>()).WhenDone(
				a =>
				{
					if (a != null)
						aResult.Return(true);
					else
						aResult.Return(false);
				},
				aResult.Throw
				);
			return aResult;
		}

		public string ToJson(SearchResult<IScore> aSearchResult)
		{
			return theScoreDataMapper.ToJson(aSearchResult);
		}

		public Result<IScore> AttachMusicXml(IScore aScore, XDoc xdoc, bool overwriteMusicXmlValues, Result<IScore> aResult)
		{
			Coroutine.Invoke(AttachMusicXmlHelper, aScore, xdoc, overwriteMusicXmlValues, new Result<IScore>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<bool> AddAttachment(string id, Stream file, string fileName, Result<bool> aResult)
		{
			theScoreDataMapper.AddAttachment(id, file, fileName, new Result<bool>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<Stream> GetAttachment(string aDocumentId, string aFileName, Result<Stream> aResult)
		{
			theScoreDataMapper.GetAttachment(aDocumentId, aFileName, new Result<Stream>()).WhenDone(
				aResult.Return,
				aResult.Throw
			);
			return aResult;
		}

		public Result<bool> DeleteAttachment(string aDocumentId, string aFileName, Result<bool> aResult)
		{
			theScoreDataMapper.DeleteAttachment(aDocumentId, aFileName, new Result<bool>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<Stream> GetConvertedScore(MimeType type, string scoreId, Result<Stream> aResult)
		{
			this.GetAttachment(scoreId, "$musicxml.xml", new Result<Stream>()).WhenDone(
				a =>
				{
					using (TemporaryFile input = new TemporaryFile())
					using (TemporaryFile output = new TemporaryFile())
					{
						XDoc doc = XDocFactory.From(a, MimeType.XML);
						doc.Save(input.Path);
						IConverter conv = Context.Current.Instance.ConverterFactory.GetConverter(type);
						IList<string> res = conv.Convert(input.Path, output.Path);
						string filePath = res.First();
						aResult.Return(new MemoryStream(File.ReadAllBytes(filePath)));
						File.Delete(filePath);
					}
				},
				aResult.Throw
				);
			return aResult;
		}

		private Yield GetScoresFromSourceHelper(int offset, int max, string aSourceId, Result<SearchResult<IScore>> aResult)
		{
			//Check if the source exists
			Result<bool> resultExists = new Result<bool>();
			yield return Context.Current.Instance.SourceController.Exists(aSourceId, resultExists);
			if (resultExists.Value)
			{
				//Get the scores that have this source id as textual or musical source.
				Result<SearchResult<IScore>> resultSearch = new Result<SearchResult<IScore>>();
				yield return theScoreDataMapper.ScoresFromSource(offset, max, aSourceId, resultSearch);
				aResult.Return(resultSearch.Value);
			}
			else
				aResult.Throw(new DreamNotFoundException("Source not found"));
		}

		private Yield CreateHelper(IScore aDoc, Result<IScore> aResult)
		{

			//Check that the sources aren't null and does exists
			yield return Coroutine.Invoke(CheckSources, aDoc, new Result());

			//Insert a the score and return
			Result<IScore> resultCreate = new Result<IScore>();
			yield return theScoreDataMapper.Create(aDoc, resultCreate);
			aResult.Return(resultCreate.Value);
		}

		private static Yield CheckSources(IScore aScore, Result aResult)
		{
			// if this source exists
			if (aScore.TextualSource != null)
			{
				if (aScore.TextualSource.SourceId == null)
				{
					aResult.Throw(new ArgumentException());
					yield break;
				}
				Result<bool> validSourceResult = new Result<bool>();
				yield return Context.Current.Instance.SourceController.Exists(aScore.TextualSource.SourceId, validSourceResult);
				if (!validSourceResult.Value)
				{
					aResult.Throw(new ArgumentException());
					yield break;
				}
			}
			//if this source exists
			if (aScore.MusicalSource != null)
			{
				if (aScore.MusicalSource.SourceId == null)
				{
					aResult.Throw(new ArgumentException());
					yield break;
				}
				Result<bool> validSourceResult = new Result<bool>();
				yield return Context.Current.Instance.SourceController.Exists(aScore.MusicalSource.SourceId, validSourceResult);
				if (!validSourceResult.Value)
				{
					aResult.Throw(new ArgumentException());
					yield break;
				}
			}
			aResult.Return();
		}

		private Yield UpdateHelper(string id, string rev, IScore aDoc, Result<IScore> aResult)
		{
			//Check if a score with this id exists.
			Result<IScore> validScoreResult = new Result<IScore>();
			yield return theScoreDataMapper.Retrieve(aDoc.Id, validScoreResult);
			if (validScoreResult.Value == null)
			{
				aResult.Throw(new ArgumentException());
				yield break;
			}

			//Check if the sources in the score exists and are not null.
			Coroutine.Invoke(CheckSources, aDoc, new Result());

			//Update and return the updated score.
			Result<IScore> scoreResult = new Result<IScore>();
			yield return theScoreDataMapper.Update(id, rev, aDoc, scoreResult);
			aResult.Return(scoreResult.Value);
		}

		private static bool IsCreator(IScore aDoc)
		{
			return aDoc.CreatorId == Context.Current.User.Id;
		}

		private static bool IsCollaborator(IScore aDoc)
		{
			IUser current = Context.Current.User;

			foreach (string collab in aDoc.CollaboratorsId)
			{
				if (current.Groups.Contains(collab) || collab == current.Id)
					return true;
			}
			return false;
		}

		private Yield AttachMusicXmlHelper(IScore aScore, XDoc aMusicXmlDoc, bool overwriteMusicXmlValues, Result<IScore> aResult)
		{
			if (overwriteMusicXmlValues)
			{
				XScore musicXml = new XScore(aMusicXmlDoc);
				aScore.CodageMelodiqueRISM = musicXml.GetCodageMelodiqueRISM();
				aScore.CodageParIntervalles = musicXml.GetCodageParIntervalle();
				aScore.Title = musicXml.MovementTitle;
				aScore.Composer = musicXml.Identification.Composer;
				aScore.Verses = musicXml.GetText();
			}

			Result<IScore> result = new Result<IScore>();
			if (aScore.Id != null)
			{
				yield return Update(aScore.Id, aScore.Rev, aScore, result);
			}
			else
			{
				yield return Insert(aScore, result);
			}

			using(TemporaryFile inputFile = new TemporaryFile())
			using(TemporaryFile outputFile = new TemporaryFile())
			{
				theLogger.Info("Saving xdoc to " + inputFile.Path);
				File.Delete(inputFile.Path);
				aMusicXmlDoc.Save(inputFile.Path);
				theLogger.Info("XDoc saved");
				theLogger.Info("Getting Converter and converting");
				//yield return Context.Current.Instance.SourceController.Exists("bla", new Result<bool>());
				IList<string> pngsFilePath = Context.Current.Instance.ConverterFactory.GetConverter(MimeType.PNG).Convert(inputFile.Path, outputFile.Path);
				int i = 1;
				foreach (string pngFile in pngsFilePath)
				{
					yield return AddAttachment(result.Value.Id, File.OpenRead(pngFile), "$partition" + i + ".png", new Result<bool>());
					File.Delete(pngFile);
					i++;
				}
			}
			//attach music xml to the created /updated score
			Stream stream = new MemoryStream(aMusicXmlDoc.ToBytes());
			yield return AddAttachment(result.Value.Id, stream, "$musicxml.xml", new Result<bool>());
			aResult.Return(result.Value);
		}
	}
}
