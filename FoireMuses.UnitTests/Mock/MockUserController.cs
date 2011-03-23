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

		public Result<JUser> GetByUsername(string username, Result<JUser> aResult)
		{
			if (theUser != null)
			{
				JToken nom;
				theUser.TryGetValue("username", out nom);
				if (nom.Value<string>() == username)
				{
					aResult.Return(theUser);
				}
				else
				{
					aResult.Throw(new Exception("Todo"));
				}
			}
			else
			{
				aResult.Throw(new Exception("Todo"));
			}
			return aResult;
		}

		public Result<JUser> Create(JUser aDoc, Result<JUser> aResult)
		{
			theUser = aDoc;
			aResult.Return(theUser);
			return aResult;
		}

		public Result<JUser> GetById(string id, Result<JUser> aResult)
		{
			if (theUser != null && theUser.Id == id)
			{
				aResult.Return(theUser);
			}
			else
			{
				aResult.Throw(new Exception("Todo"));
			}
			return aResult;
		}

		public Result<JUser> Get(JUser aDoc, Result<JUser> aResult)
		{
			if (theUser != null && theUser.Id == aDoc.Id)
			{
				aResult.Return(theUser);
			}
			else
			{
				aResult.Throw(new Exception("Todo"));
			}
			return aResult;
		}

		public Result<JUser> Update(JUser aDoc, Result<JUser> aResult)
		{
			if (theUser != null && theUser.Id == aDoc.Id)
			{
				theUser = aDoc;
				aResult.Return(theUser);
			}
			else
			{
				aResult.Throw(new Exception("Todo"));
			}
			return aResult;
		}

		public Result<JObject> Delete(JUser aDoc, Result<JObject> aResult)
		{
			if (theUser != null && theUser.Id == aDoc.Id)
			{
				theUser = null;
				aResult.Return(aDoc);
			}
			else
			{
				aResult.Throw(new Exception("Todo"));
			}
			return aResult;
		}

		public void Created()
		{
			throw new NotImplementedException();
		}

		public void Updated()
		{
			throw new NotImplementedException();
		}

		public void Deleted()
		{
			throw new NotImplementedException();
		}

		public Result<LoveSeat.ViewResult<string, string, JUser>> GetAll(Result<LoveSeat.ViewResult<string, string, JUser>> aResult)
		{
			throw new NotImplementedException();
		}


		public void Readed(JUser doc, Result<JUser> res)
		{
			throw new NotImplementedException();
		}
	}
}
