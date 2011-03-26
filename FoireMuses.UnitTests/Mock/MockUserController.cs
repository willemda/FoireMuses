using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Tasking;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Interfaces;

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

		public Result<IUser> Update(IUser aDoc, Result<IUser> aResult)
		{
			user = aDoc;
			aResult.Return(user);
			return aResult;
		}

		public Result<IUser> Get(IUser aDoc, Result<IUser> aResult)
		{
			aResult.Return(user);
			return aResult;
		}

		public Result<IUser> Get(string id, Result<IUser> aResult)
		{;
			aResult.Return(user);
			return aResult;
		}

		public Result<bool> Delete(IUser aDoc, Result<bool> aResult)
		{
			user = null;
			aResult.Return(true);
			return aResult;
		}

		public Result<Core.SearchResult<IUser>> GetAll(int offset, int max, Result<Core.SearchResult<IUser>> aResult)
		{
			throw new NotImplementedException();
		}
	}
}
