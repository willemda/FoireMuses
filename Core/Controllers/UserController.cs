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
		private IUserDataMapper theStoreController;

		public  UserController(IUserDataMapper aController)
		{
			theStoreController = aController;
		}

		public Result<IUser> GetByUsername(string username, Result<IUser> aResult)
		{
			theStoreController.RetrieveByUsername(username, new Result<IUser>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<SearchResult<IUser>> GetAll(int offset, int max, Result<SearchResult<IUser>> aResult)
		{
			theStoreController.GetAllUsers(offset, max, new Result<SearchResult<IUser>>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IUser> Create(IUser aDoc, Result<IUser> aResult)
		{
			theStoreController.Create(aDoc, new Result<IUser>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IUser> Get(string id, Result<IUser> aResult)
		{
			theStoreController.Retrieve(id, new Result<IUser>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IUser> Update(IUser aDoc, Result<IUser> aResult)
		{
			theStoreController.Update(aDoc, new Result<IUser>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<bool> Delete(IUser aDoc, Result<bool> aResult)
		{
			theStoreController.Delete(aDoc, new Result<bool>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IUser> Get(IUser aDoc, Result<IUser> aResult)
		{
			return Get(aDoc.Id, aResult);
		}
	}
}
