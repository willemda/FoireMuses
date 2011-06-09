using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using LoveSeat;
using MindTouch.Tasking;
using FoireMuses.Core.Business;
using FoireMuses.Core.Utils;
using Newtonsoft.Json.Linq;

namespace FoireMuses.Core.Loveseat
{
	public class LoveseatUserDataMapper: BaseDataMapper<IUser>, IUserDataMapper
	{
		public LoveseatUserDataMapper(ISettingsController aSettingsController)
			:base(aSettingsController)
		{
			if (!CouchDatabase.DocumentExists("_design/users"))
			{
				CouchDesignDocument view = new CouchDesignDocument(CouchViews.VIEW_USERS);
				view.Views.Add(CouchViews.VIEW_ALL,new CouchView(@"function(doc){
														if(doc.otype && doc.otype == 'user')
															emit(doc._id, doc._rev)
													}"));
				CouchDatabase.CreateDocument(view);
			}

			if (!CouchDatabase.DocumentExists("admin"))
			{
				JDocument doc = new JDocument();
				doc.Id = "admin";
				doc["password"] = "admin@foiremuses";
				doc["isAdmin"] = true;
				doc["otype"] = "user";
				CouchDatabase.CreateDocument(doc);
			}

		}

		public Result<IUser> Create(IUser aDocument, Result<IUser> aResult)
		{
			CouchDatabase.CreateDocument<JUser>(aDocument as JUser, new Result<JUser>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IUser> Retrieve(string id, Result<IUser> aResult)
		{
			CouchDatabase.GetDocument<JUser>(id, new Result<JUser>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IUser> Update(string id, string rev, IUser aDocument, Result<IUser> aResult)
		{
			aDocument.Id = id;
			aDocument.Rev = rev;
			CouchDatabase.UpdateDocument<JUser>(aDocument as JUser, new Result<JUser>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<SearchResult<IUser>> SearchUserForText(int offset, int max, string textSearch, IUser aUser, Result<SearchResult<IUser>> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<SearchResult<IUser>> GetAll(int offset, int max, Result<SearchResult<IUser>> aResult)
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

		public IUser FromJson(string json)
		{
			return new JUser(JObject.Parse(json));
		}

        public IUser CreateNew()
        {
            return new JUser();
        }
	}
}
