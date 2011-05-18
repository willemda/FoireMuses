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
using System.IO;
using FoireMuses.Core.Loveseat.Business;

namespace FoireMuses.Core.Loveseat
{
	public class LoveseatSourcePageDataMapper: Convert<ISourcePage>, ISourcePageDataMapper
	{
		private readonly CouchDatabase theCouchDatabase;
		private readonly CouchClient theCouchClient;

		public LoveseatSourcePageDataMapper(ISettingsController aSettingsController)
		{
			theCouchClient = new CouchClient(aSettingsController.Host, aSettingsController.Port, aSettingsController.Username, aSettingsController.Password);
			theCouchDatabase = theCouchClient.GetDatabase(aSettingsController.DatabaseName);
		}

		public Result<SearchResult<ISourcePage>> GetPagesFromSource(int offset, int max, string aSourceId, Result<SearchResult<ISourcePage>> aResult)
		{
			ViewOptions viewOptions = new ViewOptions();
			viewOptions.Skip = offset;
			viewOptions.StartKey.Add(new JRaw("[\"" + aSourceId + "\"]"));
			viewOptions.EndKey.Add(new JRaw("[\"" + aSourceId + "\",{}]"));
			viewOptions.InclusiveEnd = false;
			if (max > 0)
				viewOptions.Limit = max;

			theCouchDatabase.GetView<string[], string, JSourcePage>(CouchViews.VIEW_SOURCEPAGES, CouchViews.VIEW_SOURCEPAGES_FROM_SOURCE, viewOptions, new Result<ViewResult<string[], string, JSourcePage>>()).WhenDone(
				a =>
				{
					IList<ISourcePage> results = new List<ISourcePage>();
					foreach (ViewResultRow<string[], string, JSourcePage> row in a.Rows)
					{
						results.Add(row.Doc);
					}
					aResult.Return(new SearchResult<ISourcePage>(results, a.OffSet, max, a.TotalRows));
				},
				aResult.Throw
				);
			return aResult;
		}

		public Result<SearchResult<ISourcePage>> GetAll(int offset, int max, Result<SearchResult<ISourcePage>> aResult)
		{
			ViewOptions viewOptions = new ViewOptions();
			viewOptions.Skip = offset;
			if (max > 0)
				viewOptions.Limit = max;

			theCouchDatabase.GetView<string, string, JSourcePage>(CouchViews.VIEW_SOURCEPAGES, CouchViews.VIEW_ALL, viewOptions, new Result<ViewResult<string, string, JSourcePage>>()).WhenDone(
				a =>
				{
					IList<ISourcePage> list = new List<ISourcePage>();
					foreach (ViewResultRow<string, string, JSourcePage> row in a.Rows)
					{
						list.Add(row.Doc);
					}
					aResult.Return(new SearchResult<ISourcePage>(list, a.OffSet, max, a.TotalRows));
				},
				aResult.Throw
				);
			return aResult;
		}

		public Result<ISourcePage> Create(ISourcePage aDocument, Result<ISourcePage> aResult)
		{
			theCouchDatabase.CreateDocument<JSourcePage>(aDocument as JSourcePage, new Result<JSourcePage>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<ISourcePage> Retrieve(string aDocumentId, Result<ISourcePage> aResult)
		{
			theCouchDatabase.GetDocument<JSourcePage>(aDocumentId, new Result<JSourcePage>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<ISourcePage> Update(string aDocumentId, string aRev, ISourcePage aDocument, Result<ISourcePage> aResult)
		{
			aDocument.Id = aDocumentId;
			aDocument.Rev = aRev;
			theCouchDatabase.UpdateDocument<JSourcePage>(aDocument as JSourcePage, new Result<JSourcePage>()).WhenDone(
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

		ISourcePage IDataMapper<ISourcePage>.FromXml(MindTouch.Xml.XDoc aJson)
		{
			throw new NotImplementedException();
		}

		public MindTouch.Xml.XDoc ToXml(ISourcePage anObject)
		{
			throw new NotImplementedException();
		}

		public string ToXml(SearchResult<ISourcePage> aSearchResult)
		{
			throw new NotImplementedException();
		}

		public ISourcePage FromJson(string json)
		{
			return new JSourcePage(JObject.Parse(json));
		}

        public ISourcePage CreateNew()
        {
            return new JSourcePage();
        }

	}
}
