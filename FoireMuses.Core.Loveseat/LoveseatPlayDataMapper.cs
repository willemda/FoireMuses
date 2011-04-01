using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using LoveSeat;
using MindTouch.Tasking;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Utils;
using FoireMuses.Core.Business;

namespace FoireMuses.Core.Loveseat
{
	public class LoveseatPlayDataMapper: Convert<IPlay>, IPlayDataMapper
	{
		private readonly CouchDatabase theCouchDatabase;
		private readonly CouchClient theCouchClient;

		public LoveseatPlayDataMapper(ISettingsController aSettingsController)
		{
			theCouchClient = new CouchClient(aSettingsController.Host, aSettingsController.Port, aSettingsController.Username, aSettingsController.Password);
			theCouchDatabase = theCouchClient.GetDatabase(aSettingsController.DatabaseName);
		}

		public Result<SearchResult<IPlay>> GetPlaysFromSource(int offset, int max, string aSourceId, Result<SearchResult<IPlay>> aResult)
		{
			ViewOptions viewOptions = new ViewOptions();
			viewOptions.Skip = offset;
			viewOptions.StartKey.Add(new JRaw("[\"" + aSourceId + "\"]"));
			viewOptions.EndKey.Add(new JRaw("[\"" + aSourceId + "\",{}]"));
			viewOptions.InclusiveEnd = false;
			if (max > 0)
				viewOptions.Limit = max;

			theCouchDatabase.GetView<string[], string, JPlay>(CouchViews.VIEW_PLAYS, CouchViews.VIEW_PLAYS_FROM_SOURCE, viewOptions, new Result<ViewResult<string[], string, JPlay>>()).WhenDone(
				a =>
				{
					IList<IPlay> results = new List<IPlay>();
					foreach (ViewResultRow<string[], string, JPlay> row in a.Rows)
					{
						results.Add(row.Doc);
					}
					aResult.Return(new SearchResult<IPlay>(results, a.OffSet, max, a.TotalRows));
				},
				aResult.Throw
				);
			return aResult;
		}

		public Result<SearchResult<IPlay>> GetAll(int offset, int max, Result<SearchResult<IPlay>> aResult)
		{
			ViewOptions viewOptions = new ViewOptions();
			viewOptions.Skip = offset;
			if (max > 0)
				viewOptions.Limit = max;

			theCouchDatabase.GetView<string, string, JPlay>(CouchViews.VIEW_PLAYS, CouchViews.VIEW_ALL, viewOptions, new Result<ViewResult<string, string, JPlay>>()).WhenDone(
				a =>
				{
					IList<IPlay> list = new List<IPlay>();
					foreach (ViewResultRow<string, string, JPlay> row in a.Rows)
					{
						list.Add(row.Doc);
					}
					aResult.Return(new SearchResult<IPlay>(list, a.OffSet, max, a.TotalRows));
				},
				aResult.Throw
				);
			return aResult;
		}

		public Result<IPlay> Create(IPlay aDocument, Result<IPlay> aResult)
		{
			theCouchDatabase.CreateDocument<JPlay>(aDocument as JPlay, new Result<JPlay>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IPlay> Retrieve(string aDocumentId, Result<IPlay> aResult)
		{
			theCouchDatabase.GetDocument<JPlay>(aDocumentId, new Result<JPlay>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IPlay> Update(string aDocumentId, string aRev, IPlay aDocument, Result<IPlay> aResult)
		{
			aDocument.Id = aDocumentId;
			aDocument.Rev = aRev;
			theCouchDatabase.CreateDocument<JPlay>(aDocument as JPlay, new Result<JPlay>()).WhenDone(
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

		IPlay IDataMapper<IPlay>.FromXml(MindTouch.Xml.XDoc aJson)
		{
			throw new NotImplementedException();
		}

		public MindTouch.Xml.XDoc ToXml(IPlay anObject)
		{
			throw new NotImplementedException();
		}

		public string ToXml(SearchResult<IPlay> aSearchResult)
		{
			throw new NotImplementedException();
		}

		public IPlay FromJson(string json)
		{
			return new JPlay(JObject.Parse(json));
		}

	}
}
