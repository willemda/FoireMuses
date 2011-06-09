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

namespace FoireMuses.Core.Loveseat
{
	public class LoveseatPlayDataMapper: BaseDataMapper<IPlay>, IPlayDataMapper
	{
		public LoveseatPlayDataMapper(ISettingsController aSettingsController)
			:base(aSettingsController)
		{
			if (!CouchDatabase.DocumentExists("_design/plays"))
			{
				CouchDesignDocument view = new CouchDesignDocument(CouchViews.VIEW_PLAYS);
				view.Views.Add(CouchViews.VIEW_ALL,new CouchView(@"
function(doc){
	if(doc.otype && doc.otype == 'play')
		emit(doc._id, doc._rev)
}"));
				view.Views.Add(CouchViews.VIEW_PLAYS_FROM_SOURCE,new CouchView(@"
function(doc){
	if(doc.otype && doc.otype=='play' && doc.sourceID)
		emit([doc.sourceID,doc._id],null)
}"));
				CouchDatabase.CreateDocument(view);
			}
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

			CouchDatabase.GetView<string[], string, JPlay>(CouchViews.VIEW_PLAYS, CouchViews.VIEW_PLAYS_FROM_SOURCE, viewOptions, new Result<ViewResult<string[], string, JPlay>>()).WhenDone(
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

			CouchDatabase.GetView<string, string, JPlay>(CouchViews.VIEW_PLAYS, CouchViews.VIEW_ALL, viewOptions, new Result<ViewResult<string, string, JPlay>>()).WhenDone(
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
			CouchDatabase.CreateDocument<JPlay>(aDocument as JPlay, new Result<JPlay>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IPlay> Retrieve(string aDocumentId, Result<IPlay> aResult)
		{
			CouchDatabase.GetDocument<JPlay>(aDocumentId, new Result<JPlay>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IPlay> Update(string aDocumentId, string aRev, IPlay aDocument, Result<IPlay> aResult)
		{
			aDocument.Id = aDocumentId;
			aDocument.Rev = aRev;
			CouchDatabase.CreateDocument<JPlay>(aDocument as JPlay, new Result<JPlay>()).WhenDone(
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

		public Result<bool> AddAttachment(string id, Stream file, string fileName, Result<bool> aResult)
		{

			CouchDatabase.AddAttachment(id, file, fileName, new Result<JObject>()).WhenDone(
				a =>
				{
					aResult.Return(true);
				},
				aResult.Throw
				);
			return aResult;
		}

		public IPlay FromJson(string json)
		{
			return new JPlay(JObject.Parse(json));
		}

        public IPlay CreateNew()
        {
            return new JPlay();
        }

	}
}
