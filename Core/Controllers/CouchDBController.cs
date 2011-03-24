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
		private CouchDatabase CouchDatabase { get; private set; }
		private CouchClient CouchClient { get; private set; }

		public CouchDBController(ISettingsController aSettingsController)
		{
			CouchClient = new CouchClient(aSettingsController.Host, aSettingsController.Port, aSettingsController.Username, aSettingsController.Password);
			CouchDatabase = CouchClient.GetDatabase(aSettingsController.DatabaseName);
		}

		public Result<JScore> CreateScore(JScore aDocument, Result<JScore> aResult)
		{
			CouchDatabase.CreateDocument<JScore>(aDocument, new Result<JScore>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<JScore> GetScore(JScore aDocument, Result<JScore> aResult)
		{
			return GetScoreById(aDocument.Id, aResult);
		}

		public Result<JScore> GetScoreById(string id, Result<JScore> aResult)
		{
			CouchDatabase.GetDocument<JScore>(id, new Result<JScore>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<JScore> UpdateScore(JScore aDocument, Result<JScore> aResult)
		{
			CouchDatabase.UpdateDocument<JScore>(aDocument, new Result<JScore>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<bool> DeleteScore(JScore aDocument, Result<bool> aResult)
		{
			CouchDatabase.DeleteDocument(aDocument, new Result<JObject>()).WhenDone(
				a =>
				{
					aResult.Return(true);
				},
				aResult.Throw
				);
			return aResult;
		}

		public Result<SearchResult<JScore>> SearchScoreForText(string textSearch, JScore aScore, Result<SearchResult<JScore>> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<SearchResult<JScore>> SearchScoreForCode(string code, JScore aScore, Result<SearchResult<JScore>> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<JUser> CreateUser(JUser aDocument, Result<JUser> aResult)
		{
			CouchDatabase.CreateDocument<JUser>(aDocument, new Result<JUser>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<JUser> GetUser(JUser aDocument, Result<JUser> aResult)
		{
			return GetUserById(aDocument.Id, aResult);
		}

		public Result<JUser> GetUserByUsername(string username, Result<JUser> aResult)
		{
			ViewOptions viewOptions = new ViewOptions();
			viewOptions.Key.Add(username);

			CouchDatabase.GetView<string, string, JUser>(CouchViews.VIEW_USERS, CouchViews.VIEW_USERS_BY_USERNAME, viewOptions, new Result<ViewResult<string, string, JUser>>()).WhenDone(
				a =>
				{
					JUser result = null;
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

		public Result<JUser> GetUserById(string id, Result<JUser> aResult)
		{
			CouchDatabase.GetDocument<JUser>(id, new Result<JUser>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<JUser> UpdateUser(JUser aDocument, Result<JUser> aResult)
		{
			CouchDatabase.UpdateDocument<JUser>(aDocument, new Result<JUser>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<bool> DeleteUser(JUser aDocument, Result<bool> aResult)
		{
			CouchDatabase.DeleteDocument(aDocument, new Result<JObject>()).WhenDone(
				a=>{
					aResult.Return(true);
				},
				aResult.Throw
				);
			return aResult;
		}

		public Result<IEnumerable<JUser>> SearchUserForText(string textSearch, JUser aUser, Result<IEnumerable<JUser>> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<SearchResult<JScore>> ScoresFromSource(int offset, int max, JSource aSource, Result<SearchResult<JScore>> aResult)
		{
			ViewOptions viewOptions = new ViewOptions();
			viewOptions.Skip = offset;
			viewOptions.Key.Add(aSource.Id);
			if(max > 0)
				viewOptions.Limit = max;

			CouchDatabase.GetView<string,string,JScore>(CouchViews.VIEW_SCORES,CouchViews.VIEW_SCORES_FROM_SOURCE,viewOptions, new Result<ViewResult<string,string,JScore>>()).WhenDone(
				a =>
				{
					IList<JScore> results = new List<JScore>();
					foreach (ViewResultRow<string,string,JScore> row in a.Rows)
					{
						results.Add(row.Doc);
					}
					aResult.Return(new SearchResult<JScore>(results,a.OffSet,max,a.TotalRows));
				},
				aResult.Throw
				);
			return aResult;
		}
	}
}
