//  
//  ScoreController.cs
//  
//  Author:
//       danny <${AuthorEmail}>
// 
//  Copyright (c) 2011 danny
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using MindTouch.Tasking;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core.Utils;
using FoireMuses.Core.Exceptions;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace FoireMuses.Core.Controllers
{
	using Yield = System.Collections.Generic.IEnumerator<MindTouch.Tasking.IYield>;

	public class ScoreController : IScoreController
	{
		private static readonly log4net.ILog theLogger = log4net.LogManager.GetLogger(typeof(ScoreController));

		private readonly IScoreDataMapper theScoreDataMapper;

		public ScoreController(IScoreDataMapper aController)
		{
			theScoreDataMapper = aController;
		}

		public Result<SearchResult<IScore>> GetScoresFromSource(int offset, int max, ISource aJSource, Result<SearchResult<IScore>> aResult)
		{
			theScoreDataMapper.ScoresFromSource(offset, max, aJSource, new Result<SearchResult<IScore>>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IScore> Create(IScore aDoc, Result<IScore> aResult)
		{
			theScoreDataMapper.Create(aDoc, new Result<IScore>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IScore> Retrieve(string id, Result<IScore> aResult)
		{
			theScoreDataMapper.Retrieve(id, new Result<IScore>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		private Yield UpdateHelper(IScore aDoc, Result<IScore> aResult)
		{
			yield return CheckAuthorization(aDoc, new Result());
			//if we reach there we have the update rights.
			Result<IScore> scoreResult = new Result<IScore>();
			yield return theScoreDataMapper.Update("","",aDoc, new Result<IScore>());
			//finally return the IScore updated
			aResult.Return(scoreResult.Value);
			yield break;
		}
		public Result CheckAuthorization(IScore aDoc, Result aResult)
		{
			if (Context.Current.User == null || (!IsCreator(aDoc) && !IsCollaborator(aDoc)))
				throw new UnauthorizedAccessException();
			return aResult;
		}

		private bool IsCreator(IScore aDoc)
		{
			return aDoc.CreatorId == Context.Current.User.Id;
		}

		private bool IsCollaborator(IScore aDoc)
		{
			IUser current = Context.Current.User;
			
			foreach (string collab in aDoc.CollaboratorsId)
			{
				if (current.Groups.Contains(collab) || collab == current.Id)
					return true;
			}
			return false;
		}

		public Result<IScore> Update(string id,string rev,IScore aDoc, Result<IScore> aResult)
		{
			Coroutine.Invoke(UpdateHelper, aDoc, new Result<IScore>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<bool> Delete(string id, string rev, Result<bool> aResult)
		{
			theScoreDataMapper.Delete(id,rev, new Result<bool>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<SearchResult<IScore>> GetAll(int offset, int max, Result<SearchResult<IScore>> aResult)
		{
			theScoreDataMapper.GetAllScores(offset, max, new Result<SearchResult<IScore>>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public IScore FromJson(string aJson)
		{
			return theScoreDataMapper.FromJson(aJson);
		}
		public string ToJson(IScore aScore)
		{
			return theScoreDataMapper.ToJson(aScore);
		}
	}
}

