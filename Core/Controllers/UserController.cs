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
		public Result<IUser> GetByUsername(string username, Result<IUser> aResult)
		{
			Context.Current.Instance.StoreController.GetUserByUsername(username, new Result<IUser>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<SearchResult<IUser>> GetAll(int offset, int max, Result<SearchResult<IUser>> aResult)
		{
			Context.Current.Instance.StoreController.GetAllUsers(offset, max, new Result<SearchResult<IUser>>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IUser> Create(IUser aDoc, Result<IUser> aResult)
		{
			Context.Current.Instance.StoreController.CreateUser(aDoc, new Result<IUser>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IUser> Get(string id, Result<IUser> aResult)
		{
			Context.Current.Instance.StoreController.GetUserById(id, new Result<IUser>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IUser> Get(IUser aDoc, Result<IUser> aResult)
		{
			Context.Current.Instance.StoreController.GetUser(aDoc, new Result<IUser>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IUser> Update(IUser aDoc, Result<IUser> aResult)
		{
			Context.Current.Instance.StoreController.UpdateUser(aDoc, new Result<IUser>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<bool> Delete(IUser aDoc, Result<bool> aResult)
		{
			Context.Current.Instance.StoreController.DeleteUser(aDoc, new Result<bool>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}
	}
}
