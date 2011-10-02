using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using MindTouch.Tasking;
using DreamSeat;
using FoireMuses.Core.Utils;
using FoireMuses.Core.Business;
using Newtonsoft.Json.Linq;
using System.IO;

namespace FoireMuses.Core.Loveseat
{
	public class LoveseatSourceDataMapper : BaseDataMapper<ISource>, ISourceDataMapper
	{
		public LoveseatSourceDataMapper(ISettingsController aSettingsController)
			:base(aSettingsController)
		{
			if (!CouchDatabase.DocumentExists("_design/sources"))
			{
				CouchDesignDocument view = new CouchDesignDocument("sources");
				view.Views.Add("all", new CouchView(@"function(doc){
if(doc.otype && doc.otype == 'source')
	emit(doc._id, doc._rev)
}"));
				view.Views.Add("title", new CouchView(@"function(doc){
if(doc.otype && doc.otype=='source' && doc.name)
	emit(doc._id, doc.name)
}"));
				CouchDatabase.CreateDocument(view);
			}
		}

		public Result<SearchResult<ISource>> GetAll(int offset, int max, Result<SearchResult<ISource>> aResult)
		{
			ViewOptions viewOptions = new ViewOptions();
			viewOptions.Skip = offset;
			if (max > 0)
				viewOptions.Limit = max;

			CouchDatabase.GetView<string, string, JSource>(CouchViews.VIEW_SOURCES, CouchViews.VIEW_ALL, viewOptions, new Result<ViewResult<string, string, JSource>>()).WhenDone(
				a =>
				{
					IList<ISource> list = new List<ISource>();
					foreach (ViewResultRow<string, string, JSource> row in a.Rows)
					{
						list.Add(row.Doc);
					}
					aResult.Return(new SearchResult<ISource>(list, a.OffSet, max, a.TotalRows));
				},
				aResult.Throw
				);
			return aResult;
		}

		public Result<ISource> Create(ISource aDocument, Result<ISource> aResult)
		{
			CouchDatabase.CreateDocument<JSource>(aDocument as JSource, new Result<JSource>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<ISource> Retrieve(string aDocumentId, Result<ISource> aResult)
		{
			CouchDatabase.GetDocument<JSource>(aDocumentId, new Result<JSource>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<ISource> Update(string aDocumentId, string aRev, ISource aDocument, Result<ISource> aResult)
		{
			aDocument.Id = aDocumentId;
			aDocument.Rev = aRev;
			CouchDatabase.UpdateDocument<JSource>(aDocument as JSource, new Result<JSource>()).WhenDone(
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

			CouchDatabase.DeleteDocument(d, new Result<JObject>()).WhenDone(
				a =>
				{
					aResult.Return(true);
				},
				aResult.Throw
				);
			return aResult;
		}

		public ISource FromJson(string json)
		{
			return new JSource(JObject.Parse(json));
		}

        public ISource CreateNew()
        {
            return new JSource();
        }
	}
}
