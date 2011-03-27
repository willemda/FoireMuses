using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using MindTouch.Tasking;
using FoireMuses.Core.Utils;
using Newtonsoft.Json.Linq;

namespace FoireMuses.Core.Controllers
{
	public class UserController : IUserController
	{
		private IUserDataMapper theUserDataMapper;

		public  UserController(IUserDataMapper aController)
		{
			theUserDataMapper = aController;
		}

		public Result<IUser> GetByUsername(string username, Result<IUser> aResult)
		{
			theUserDataMapper.RetrieveByUsername(username, new Result<IUser>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<SearchResult<IUser>> GetAll(int offset, int max, Result<SearchResult<IUser>> aResult)
		{
			theUserDataMapper.GetAllUsers(offset, max, new Result<SearchResult<IUser>>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IUser> Create(IUser aDoc, Result<IUser> aResult)
		{
			theUserDataMapper.Create(aDoc, new Result<IUser>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IUser> Retrieve(string id, Result<IUser> aResult)
		{
			theUserDataMapper.Retrieve(id, new Result<IUser>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IUser> Update(string id,string rev,IUser aDoc, Result<IUser> aResult)
		{
			theUserDataMapper.Update(aDoc, new Result<IUser>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<bool> Delete(string id, Result<bool> aResult)
		{
			theUserDataMapper.Delete(id, new Result<bool>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public IUser FromJson(string aJson)
		{
			return theUserDataMapper.FromJson(aJson);
		}
		public string ToJson(IUser aUser)
		{
			return theUserDataMapper.ToJson(aUser);
		}
	}
}
