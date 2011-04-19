using System;
using System.Collections.Generic;
using LoveSeat;
using FoireMuses.Core.Interfaces;
using MindTouch.Tasking;
using FoireMuses.Core.Business;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Utils;
using FoireMuses.Core.Loveseat.Business;
using System.Reflection;
using MusicXml;
using FoireMuses.MusicXMLImport;
using System.IO;

namespace FoireMuses.Core.Loveseat{
	/// <summary>
	/// An Controller that stores in CouchDb
	/// </summary>
	public class LoveseatScoreDataMapper : Convert<IScore>, IScoreDataMapper
	{
		private readonly CouchDatabase theCouchDatabase;
		private readonly CouchClient theCouchClient;

		public LoveseatScoreDataMapper(ISettingsController aSettingsController)
		{
			theCouchClient = new CouchClient(aSettingsController.Host, aSettingsController.Port, aSettingsController.Username, aSettingsController.Password);
			theCouchDatabase = theCouchClient.GetDatabase(aSettingsController.DatabaseName);
		}

		

		public Result<IScore> Create(IScore aDocument, Result<IScore> aResult)
		{
			theCouchDatabase.CreateDocument<JScore>(aDocument as JScore, new Result<JScore>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}


		public Result<IScore> Retrieve(string id, Result<IScore> aResult)
		{
			theCouchDatabase.GetDocument<JScore>(id, new Result<JScore>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IScore> Update(string id, string rev, IScore aDocument, Result<IScore> aResult)
		{
			aDocument.Id = id;
			aDocument.Rev = rev;
			theCouchDatabase.UpdateDocument<JScore>(aDocument as JScore, new Result<JScore>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<bool> Delete(string id, string rev, Result<bool> aResult)
		{
			JDocument d = new JDocument();
			d.Id = id;
			d.Rev = rev;

			theCouchDatabase.DeleteDocument(d, new Result<JObject>()).WhenDone(
				a =>
				{
					aResult.Return(true);
				},
				aResult.Throw
				);
			return aResult;
		}

		public Result<bool> AddAttachment(string id, Stream file, string fileName, Result<bool> aResult)
		{

			theCouchDatabase.AddAttachment(id, file, fileName, new Result<JObject>()).WhenDone(
				a =>
				{
					aResult.Return(true);
				},
				aResult.Throw
				);
			return aResult;
		}


		public Result<Stream> GetAttachment(string id, string fileName, Result<Stream> aResult)
		{
			theCouchDatabase.GetAttachment(id, fileName, new Result<Stream>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<SearchResult<IScore>> SearchScoreForText(int offset, int max, string textSearch, IScore aScore, Result<SearchResult<IScore>> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<SearchResult<IScore>> SearchScoreForCode(int offset, int max, string code, IScore aScore, Result<SearchResult<IScore>> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<SearchResult<IScore>> ScoresFromSource(int offset, int max, string aSourceId, Result<SearchResult<IScore>> aResult)
		{
			ViewOptions viewOptions = new ViewOptions();
			viewOptions.Skip = offset;
			viewOptions.StartKey.Add(new JRaw("[\"" + aSourceId + "\"]"));
			viewOptions.EndKey.Add(new JRaw("[\"" + aSourceId + "\",{}]"));
			viewOptions.InclusiveEnd = false;
			if (max > 0)
				viewOptions.Limit = max;

			theCouchDatabase.GetView<string[], string, JScore>(CouchViews.VIEW_SCORES, CouchViews.VIEW_SCORES_FROM_SOURCE, viewOptions, new Result<ViewResult<string[], string, JScore>>()).WhenDone(
				a =>
				{
					IList<IScore> results = new List<IScore>();
					foreach (ViewResultRow<string[], string, JScore> row in a.Rows)
					{
						results.Add(row.Doc);
					}
					aResult.Return(new SearchResult<IScore>(results, a.OffSet, max, a.TotalRows));
				},
				aResult.Throw
				);
			return aResult;
		}

		public Result<SearchResult<IScore>> GetAll(int offset, int max, Result<SearchResult<IScore>> aResult)
		{
			ViewOptions viewOptions = new ViewOptions();
			viewOptions.Skip = offset;
			if (max > 0)
				viewOptions.Limit = max;

			theCouchDatabase.GetView<string, string, JScore>(CouchViews.VIEW_SCORES, CouchViews.VIEW_ALL, viewOptions, new Result<ViewResult<string, string, JScore>>()).WhenDone(
				a =>
				{
					IList<IScore> list = new List<IScore>();
					foreach (ViewResultRow<string, string, JScore> row in a.Rows)
					{
						list.Add(row.Doc);
					}
					aResult.Return(new SearchResult<IScore>(list, a.OffSet, max, a.TotalRows));
				},
				aResult.Throw
				);
			return aResult;
		}

		public IScore FromXml(MindTouch.Xml.XDoc aXML)
		{
			XScore xscore = new XScore(aXML);
			JScore js = new JScore();
			js["codageParIntervalle"] = xscore.GetCodageParIntervalle();
			js["codageMelodiqueRISM"] = xscore.GetCodageMelodiqueRISM();
			js["verses"] = xscore.GetText();
			js["title"] = xscore.MovementTitle;
			js["composer"] = xscore.Identification.Composer;
			return js;

		}

		public MindTouch.Xml.XDoc ToXml(IScore anObject)
		{
			throw new NotImplementedException();
		}

		public string ToXml(SearchResult<IScore> aSearchResult)
		{
			throw new NotImplementedException();
		}

		public IScore FromJson(string json)
		{
			return new JScore(JObject.Parse(json));
		}

        public IScore CreateNew()
        {
            return new JScore();
        }
	}
}
