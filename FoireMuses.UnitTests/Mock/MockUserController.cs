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

		public IUser FromJson(string aJson)
		{
			throw new NotImplementedException();
		}

		public string ToJson(IUser anObject)
		{
			throw new NotImplementedException();
		}

		public string ToJson(SearchResult<IUser> aSearchResult)
		{
			throw new NotImplementedException();
		}

		public IUser FromXml(MindTouch.Xml.XDoc aXml)
		{
			throw new NotImplementedException();
		}

		public MindTouch.Xml.XDoc ToXml(IUser anObject)
		{
			throw new NotImplementedException();
		}

		public Result<IUser> Insert(IUser aDoc, Result<IUser> aResult)
		{
			user = aDoc;
			aResult.Return(user);
			return aResult;
		}

		public Result<IUser> Update(string id, string rev, IUser aDoc, Result<IUser> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<IUser> Retrieve(string id, Result<IUser> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<bool> Delete(string id, string rev, Result<bool> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<bool> Exists(string id, Result<bool> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<SearchResult<IUser>> GetAll(int offset, int max, Result<SearchResult<IUser>> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<IUser> Login(string username, string password, Result<IUser> aResult)
		{
            aResult.Return(new JUser() { { "_id",username},{"password",password} });
            return aResult;
		}


        public IUser CreateNew()
        {
            throw new NotImplementedException();
        }
    }
}
