using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Tasking;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core;
using FoireMuses.Core.Business;

namespace FoireMuses.UnitTests.Mock
{
	internal class MockUserController : IUserController
	{
		public IUser user = null;

		public Result<IUser> GetByUsername(string username, Result<IUser> aResult)
		{
			aResult.Return(user);
			return aResult;
		}

		public Result<IUser> Create(IUser aDoc, Result<IUser> aResult)
		{
			user = aDoc;
			aResult.Return(user);
			return aResult;
		}

		public Result<IUser> Update(string id, string rev, IUser aDoc, Result<IUser> aResult)
		{
			user = aDoc;
			aResult.Return(user);
			return aResult;
		}

		public Result<IUser> Retrieve(string id, Result<IUser> aResult)
		{
			aResult.Return(user);
			return aResult;
		}

		public Result<bool> Delete(string id, string rev, Result<bool> aResult)
		{
			user = null;
			aResult.Return(true);
			return aResult;
		}

		public Result<SearchResult<IUser>> GetAll(int offset, int max, Result<SearchResult<IUser>> aResult)
		{
			aResult.Return(new SearchResult<IUser>(new IUser[] { user }, offset, max, 1));
			return aResult;
		}

		public IUser FromJson(string aJson)
		{
			return new JUser(JObject.Parse(aJson));
		}

		public string ToJson(IUser aJson)
		{
			return aJson.ToString();
		}
	}
}
