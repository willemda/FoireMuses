using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveSeat;
using MindTouch.Tasking;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Business;

namespace FoireMuses.Core.Interfaces
{
	/// <summary>
	/// A Interface that describes what a store controller must be able to do.
	/// </summary>
	internal interface IStoreController
	{
		Result<IScore> CreateScore (IScore aDocument, Result<IScore> aResult);
		Result<IScore> GetScore(IScore aDocument, Result<IScore> aResult);
		Result<IScore> GetScoreById(string id, Result<IScore> aResult);
		Result<IScore> UpdateScore(IScore aDocument, Result<IScore> aResult);
		Result<bool> DeleteScore (IScore aDocument, Result<bool> aResult);
		Result<IList<IScore>> SearchScoreForText(int offset, int max, string textSearch, IScore aScore, Result<IList<IScore>> aResult);
		Result<IList<IScore>> SearchScoreForCode(int offset, int max, string code, IScore aScore, Result<IList<IScore>> aResult);
		Result<IList<IScore>> ScoresFromSource(int offset, int max, ISource aSource, Result<IList<IScore>> aResult);

		Result<IUser> CreateUser(IUser aDocument, Result<IUser> aResult);
		Result<IUser> GetUser(IUser aDocument, Result<IUser> aResult);
		Result<IUser> GetUserByUsername(string username, Result<IUser> aResult);
		Result<IUser> GetUserById(string id, Result<IUser> aResult);
		Result<IUser> UpdateUser(IUser aDocument, Result<IUser> aResult);
		Result<bool> DeleteUser(IUser aDocument, Result<bool> aResult);
		Result<IList<IUser>> SearchUserForText(string textSearch, IUser aUser, Result<IList<IUser>> aResult);
	}
}
