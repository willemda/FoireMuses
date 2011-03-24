using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Tasking;
using FoireMuses.Core.Business;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Interfaces;

namespace FoireMuses.UnitTests.Mock
{
	internal class MockUserController : IUserController
	{
		public JUser theUser = null;



		public Result<IUser> GetByUsername(string username, Result<IUser> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<IUser> Create(IUser aDoc, Result<IUser> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<IUser> Update(IUser aDoc, Result<IUser> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<IUser> Get(IUser aDoc, Result<IUser> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<IUser> Get(string id, Result<IUser> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<bool> Delete(IUser aDoc, Result<bool> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<Core.SearchResult<IUser>> GetAll(int offset, int max, Result<Core.SearchResult<IUser>> aResult)
		{
			throw new NotImplementedException();
		}
	}
}
