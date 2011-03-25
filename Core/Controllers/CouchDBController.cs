using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveSeat;
using MindTouch.Tasking;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core.Utils;
using System.ComponentModel;
using FoireMuses.Core.Business;
using LoveSeat.Support;
using System.Collections;

namespace FoireMuses.Core.Controllers
{
	/// <summary>
	/// An Controller that stores in CouchDb
	/// </summary>
	internal class CouchDBController : IStoreController
	{
		private CouchDatabase CouchDatabase;
		private CouchClient CouchClient;

		public CouchDBController(ISettingsController aSettingsController)
		{
			CouchClient = new CouchClient(aSettingsController.Host, aSettingsController.Port, aSettingsController.Username, aSettingsController.Password);
			CouchDatabase = CouchClient.GetDatabase(aSettingsController.DatabaseName);
		}

		public Result<IScore> CreateScore(IScore aDocument, Result<IScore> aResult)
		{
			CouchDatabase.CreateDocument<JScore>(aDocument as JScore, new Result<JScore>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IScore> GetScore(IScore aDocument, Result<IScore> aResult)
		{
			return GetScoreById((aDocument as JScore).Id, aResult);
		}

		public Result<IScore> GetScoreById(string id, Result<IScore> aResult)
		{
			CouchDatabase.GetDocument<JScore>(id, new Result<JScore>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IScore> UpdateScore(IScore aDocument, Result<IScore> aResult)
		{
			CouchDatabase.UpdateDocument<JScore>(aDocument as JScore, new Result<JScore>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<bool> DeleteScore(IScore aDocument, Result<bool> aResult)
		{
			CouchDatabase.DeleteDocument(aDocument as JScore, new Result<JObject>()).WhenDone(
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

		public Result<IUser> CreateUser(IUser aDocument, Result<IUser> aResult)
		{
			CouchDatabase.CreateDocument<JUser>(aDocument as JUser, new Result<JUser>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IUser> GetUser(IUser aDocument, Result<IUser> aResult)
		{
			return GetUserById((aDocument as JUser).Id, aResult);
		}

		public Result<IUser> GetUserByUsername(string username, Result<IUser> aResult)
		{
			ViewOptions viewOptions = new ViewOptions();
			viewOptions.Key.Add(username);

			CouchDatabase.GetView<string, string, JUser>(CouchViews.VIEW_USERS, CouchViews.VIEW_USERS_BY_USERNAME, viewOptions, new Result<ViewResult<string, string, JUser>>()).WhenDone(
				a =>
				{
					IUser result = null;
					foreach (ViewResultRow<string, string, JUser> row in a.Rows)
					{
						result = row.Doc;
					}
					aResult.Return(result);
				},
				aResult.Throw
				);
			return aResult;
		}

		public Result<IUser> GetUserById(string id, Result<IUser> aResult)
		{
			CouchDatabase.GetDocument<JUser>(id, new Result<JUser>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IUser> UpdateUser(IUser aDocument, Result<IUser> aResult)
		{
			CouchDatabase.UpdateDocument<JUser>(aDocument as JUser, new Result<JUser>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<bool> DeleteUser(IUser aDocument, Result<bool> aResult)
		{
			CouchDatabase.DeleteDocument(aDocument as JUser, new Result<JObject>()).WhenDone(
				a=>{
					aResult.Return(true);
				},
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

			CouchDatabase.GetView<string,string,JScore>(CouchViews.VIEW_SCORES,CouchViews.VIEW_SCORES_FROM_SOURCE,viewOptions, new Result<ViewResult<string,string,JScore>>()).WhenDone(
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

			CouchDatabase.GetView<string, string, JScore>(CouchViews.VIEW_SCORES, CouchViews.VIEW_ALL, viewOptions, new Result<ViewResult<string, string, JScore>>()).WhenDone(
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

			CouchDatabase.GetView<string, string, JUser>(CouchViews.VIEW_USERS, CouchViews.VIEW_ALL, viewOptions, new Result<ViewResult<string, string, JUser>>()).WhenDone(
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
	}
}
