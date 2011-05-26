using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using MindTouch.Tasking;
using FoireMuses.Core.Utils;
using Newtonsoft.Json.Linq;
using System.Security;

namespace FoireMuses.Core.Controllers
{
	public class UserController : IUserController
	{
		private IUserDataMapper theUserDataMapper;

		public  UserController(IUserDataMapper aController)
		{
			theUserDataMapper = aController;
		}

		public string ToJson(SearchResult<IUser> aSearchResult)
		{
			return theUserDataMapper.ToJson(aSearchResult);
		}

		public Result<SearchResult<IUser>> GetAll(int offset, int max, Result<SearchResult<IUser>> aResult)
		{
			theUserDataMapper.GetAll(offset, max, new Result<SearchResult<IUser>>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IUser> Insert(IUser aDoc, Result<IUser> aResult)
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

		public Result<IUser> Update(string id, string rev,IUser aDoc, Result<IUser> aResult)
		{
			theUserDataMapper.Update(id,rev, aDoc, new Result<IUser>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<bool> Delete(string id, string rev, Result<bool> aResult)
		{
			theUserDataMapper.Delete(id,rev, new Result<bool>()).WhenDone(
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

		public Result<bool> Exists(string id, Result<bool> aResult)
		{
			this.Retrieve(id, new Result<IUser>()).WhenDone(
				a =>
				{
					if (a != null)
						aResult.Return(true);
					else
						aResult.Return(false);
				},
				aResult.Throw
				);
			return aResult;
		}

		public IUser CreateNew()
		{
			return theUserDataMapper.CreateNew();
		}
	}
}
