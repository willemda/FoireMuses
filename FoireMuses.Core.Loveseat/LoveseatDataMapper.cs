﻿using System;
using System.Collections.Generic;
using LoveSeat;
using FoireMuses.Core.Interfaces;
using MindTouch.Tasking;
using FoireMuses.Core.Business;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Utils;

namespace FoireMuses.Core.Loveseat
{
	/// <summary>
	/// An Controller that stores in CouchDb
	/// </summary>
	public class LoveseatDataMapper : IScoreDataMapper, IUserDataMapper, ISourceDataMapper
	{
		private readonly CouchDatabase theCouchDatabase;
		private readonly CouchClient theCouchClient;

		public LoveseatDataMapper(ISettingsController aSettingsController)
		{
			theCouchClient = new CouchClient(aSettingsController.Host, aSettingsController.Port, aSettingsController.Username, aSettingsController.Password);
			theCouchDatabase = theCouchClient.GetDatabase(aSettingsController.DatabaseName);
		}

		public Result<ISource> Create(ISource aDocument, Result<ISource> aResult)
		{
			theCouchDatabase.CreateDocument<JSource>(aDocument as JSource, new Result<JSource>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<ISource> Retrieve(string aDocumentId, Result<ISource> aResult)
		{
			theCouchDatabase.GetDocument<JSource>(aDocumentId, new Result<JSource>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<ISource> Update(string aDocumentId, string aRev, ISource aDocument, Result<ISource> aResult)
		{
			aDocument.Id = aDocumentId;
			aDocument.Rev = aRev;
			theCouchDatabase.UpdateDocument<JSource>(aDocument as JSource, new Result<JSource>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
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

		public Result<SearchResult<IScore>> SearchScoreForText(int offset, int max, string textSearch, IScore aScore, Result<SearchResult<IScore>> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<SearchResult<IScore>> SearchScoreForCode(int offset, int max, string code, IScore aScore, Result<SearchResult<IScore>> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<IUser> Create(IUser aDocument, Result<IUser> aResult)
		{
			theCouchDatabase.CreateDocument<JUser>(aDocument as JUser, new Result<JUser>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IUser> Retrieve(string id, Result<IUser> aResult)
		{
			theCouchDatabase.GetDocument<JUser>(id, new Result<JUser>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IUser> Update(string id, string rev, IUser aDocument, Result<IUser> aResult)
		{
			aDocument.Id = id;
			aDocument.Rev = rev;
			theCouchDatabase.UpdateDocument<JUser>(aDocument as JUser, new Result<JUser>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<SearchResult<IUser>> SearchUserForText(int offset, int max, string textSearch, IUser aUser, Result<SearchResult<IUser>> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<SearchResult<IScore>> ScoresFromSource(int offset, int max, ISource aSource, Result<SearchResult<IScore>> aResult)
		{
			ViewOptions viewOptions = new ViewOptions();
			viewOptions.Skip = offset;
			viewOptions.Key.Add((aSource as JSource).Id);
			if(max > 0)
				viewOptions.Limit = max;

			theCouchDatabase.GetView<string,string,JScore>(CouchViews.VIEW_SCORES,CouchViews.VIEW_SCORES_FROM_SOURCE,viewOptions, new Result<ViewResult<string,string,JScore>>()).WhenDone(
				a =>
				{
					IList<IScore> results = new List<IScore>();
					foreach (ViewResultRow<string,string,JScore> row in a.Rows)
					{
						results.Add(row.Doc);
					}
					aResult.Return(new SearchResult<IScore>(results,a.OffSet,max,a.TotalRows));
				},
				aResult.Throw
				);
			return aResult;
		}

		public Result<SearchResult<IScore>> GetAllScores(int offset, int max, Result<SearchResult<IScore>> aResult)
		{
			ViewOptions viewOptions = new ViewOptions();
			viewOptions.Skip = offset;
			if(max > 0)
				viewOptions.Limit = max;

			theCouchDatabase.GetView<string, string, JScore>(CouchViews.VIEW_SCORES, CouchViews.VIEW_ALL, viewOptions, new Result<ViewResult<string, string, JScore>>()).WhenDone(
				a =>
				{
					IList<IScore> list = new List<IScore>();
					foreach (ViewResultRow<string, string, JScore> row in a.Rows)
					{
						list.Add(row.Doc);
					}
					aResult.Return(new SearchResult<IScore>(list,a.OffSet,max,a.TotalRows));
				},
				aResult.Throw
				);
			return aResult;
		}


		public Result<SearchResult<IUser>> GetAllUsers(int offset, int max, Result<SearchResult<IUser>> aResult)
		{
			ViewOptions viewOptions = new ViewOptions();
			viewOptions.Skip = offset;
			if (max > 0)
				viewOptions.Limit = max;

			theCouchDatabase.GetView<string, string, JUser>(CouchViews.VIEW_USERS, CouchViews.VIEW_ALL, viewOptions, new Result<ViewResult<string, string, JUser>>()).WhenDone(
				a =>
				{
					IList<IUser> list = new List<IUser>();
					foreach (ViewResultRow<string, string, JUser> row in a.Rows)
					{
						list.Add(row.Doc);
					}
					aResult.Return(new SearchResult<IUser>(list, a.OffSet, max, a.TotalRows));
				},
				aResult.Throw
				);
			return aResult;
		}

		IScore IDataMapper<IScore>.FromJson(string aJson)
		{
			return new JScore(JObject.Parse(aJson));
		}

		public string ToJson(IScore anObject)
		{
			return anObject.ToString();
		}

		public IScore FromXml(MindTouch.Xml.XDoc aXML)
		{
			throw new NotImplementedException();
		}

		public MindTouch.Xml.XDoc ToXml(IScore anObject)
		{
			throw new NotImplementedException();
		}

		IUser IDataMapper<IUser>.FromJson(string aJson)
		{
			return new JUser(JObject.Parse(aJson));
		}

		public string ToJson(IUser anObject)
		{
			return anObject.ToString();
		}

		IUser IDataMapper<IUser>.FromXml(MindTouch.Xml.XDoc aJson)
		{
			throw new NotImplementedException();
		}

		public MindTouch.Xml.XDoc ToXml(IUser anObject)
		{
			throw new NotImplementedException();
		}

		

		ISource IDataMapper<ISource>.FromJson(string aJson)
		{
			return new JSource(JObject.Parse(aJson));
		}

		public string ToJson(ISource anObject)
		{
			return anObject.ToString();
		}

		ISource IDataMapper<ISource>.FromXml(MindTouch.Xml.XDoc aJson)
		{
			throw new NotImplementedException();
		}

		public MindTouch.Xml.XDoc ToXml(ISource anObject)
		{
			throw new NotImplementedException();
		}
	}
}