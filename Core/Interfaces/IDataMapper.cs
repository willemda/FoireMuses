using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Tasking;
using Newtonsoft.Json.Linq;

namespace FoireMuses.Core.Interfaces
{
	public interface IDataMapper<T>
	{
		Result<T> Create(T aDocument, Result<T> aResult);
		Result<T> Retrieve(string aDocumentId, Result<T> aResult);
		Result<T> Update(T aDocument, Result<T> aResult);
		Result<bool> Delete(T aDocument, Result<bool> aResult);
	}

	/// <summary>
	/// A Interface that describes what a store controller must be able to do.
	/// </summary>
	public interface IScoreDataMapper : IDataMapper<IScore>
	{
		Result<SearchResult<IScore>> SearchScoreForText(int offset, int max, string textSearch, IScore aScore, Result<SearchResult<IScore>> aResult);
		Result<SearchResult<IScore>> SearchScoreForCode(int offset, int max, string code, IScore aScore, Result<SearchResult<IScore>> aResult);
		Result<SearchResult<IScore>> ScoresFromSource(int offset, int max, ISource aSource, Result<SearchResult<IScore>> aResult);
		Result<SearchResult<IScore>> GetAllScores(int offset, int max, Result<SearchResult<IScore>> aResult);
	}
	public interface IUserDataMapper : IDataMapper<IUser>
	{
		Result<IUser> RetrieveByUsername(string aUserName, Result<IUser> aResult);
		Result<SearchResult<IUser>> SearchUserForText(int offset,int max, string textSearch, IUser aUser, Result<SearchResult<IUser>> aResult);
		Result<SearchResult<IUser>> GetAllUsers(int offset, int max, Result<SearchResult<IUser>> aResult);
	}
}
