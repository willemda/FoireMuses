﻿using System;
using System.Collections.Generic;
using DreamSeat;
using FoireMuses.Core.Interfaces;
using MindTouch.Tasking;
using FoireMuses.Core.Business;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Utils;
using System.IO;
using MindTouch.Xml;

namespace FoireMuses.Core.Loveseat
{
	/// <summary>
	/// An ScoreDataMapper that stores in CouchDb
	/// </summary>
	public class LoveseatScoreDataMapper : BaseDataMapper<IScore>, IScoreDataMapper
	{
		public LoveseatScoreDataMapper(ISettingsController aSettingsController)
			: base(aSettingsController)
		{
			if (!CouchDatabase.DocumentExists("_design/scores"))
			{
				CouchDesignDocument view = new CouchDesignDocument("scores");
				view.Views.Add(CouchViews.VIEW_ALL,
							   new CouchView(
								  @"function(doc){
										if(doc.otype && doc.otype == 'score')
											emit(doc._id, doc._rev)
									}"));
				view.Views.Add(CouchViews.VIEW_SCORES_FROM_SOURCE,
							   new CouchView(
								  @"function(doc){
										if(doc.otype && doc.otype=='score'){
											if(doc.textualSource && doc.textualSource.id)
												emit([doc.textualSource.id,doc._id],null)
											if(doc.musicalSource && doc.musicalSource.id) 
												emit([doc.musicalSource.id, doc._id],null)
										}
									}"));
				CouchDatabase.CreateDocument(view);
			}
		}

		public Result<IScore> Create(IScore aDocument, Result<IScore> aResult)
		{
			CouchDatabase.CreateDocument<JScore>(aDocument as JScore, new Result<JScore>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IScore> Retrieve(string id, Result<IScore> aResult)
		{
			CouchDatabase.GetDocument<JScore>(id, new Result<JScore>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IScore> Update(string id, string rev, IScore aDocument, Result<IScore> aResult)
		{
			aDocument.Id = id;
			aDocument.Rev = rev;
			CouchDatabase.UpdateDocument<JScore>(aDocument as JScore, new Result<JScore>()).WhenDone(
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

			CouchDatabase.GetView<string[], string, JScore>(CouchViews.VIEW_SCORES, CouchViews.VIEW_SCORES_FROM_SOURCE, viewOptions, new Result<ViewResult<string[], string, JScore>>()).WhenDone(
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

			CouchDatabase.GetView<string, string, JScore>(CouchViews.VIEW_SCORES, CouchViews.VIEW_ALL, viewOptions, new Result<ViewResult<string, string, JScore>>()).WhenDone(
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
