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
	public class LoveseatUserDataMapper: Convert<IUser>, IUserDataMapper
	{
		private readonly CouchDatabase theCouchDatabase;
		private readonly CouchClient theCouchClient;

		public LoveseatUserDataMapper(ISettingsController aSettingsController)
		{
			theCouchClient = new CouchClient(aSettingsController.Host, aSettingsController.Port, aSettingsController.Username, aSettingsController.Password);
			theCouchDatabase = theCouchClient.GetDatabase(aSettingsController.DatabaseName);
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

		public Result<SearchResult<IUser>> GetAll(int offset, int max, Result<SearchResult<IUser>> aResult)
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

		IUser IDataMapper<IUser>.FromXml(MindTouch.Xml.XDoc aJson)
		{
			throw new NotImplementedException();
		}

		public MindTouch.Xml.XDoc ToXml(IUser anObject)
		{
			throw new NotImplementedException();
		}

		public string ToXml(SearchResult<IUser> aSearchResult)
		{
			throw new NotImplementedException();
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
