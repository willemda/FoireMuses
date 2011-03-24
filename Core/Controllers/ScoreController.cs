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
using LoveSeat;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core.Utils;
using FoireMuses.Core.Business;
using LoveSeat.Support;
using Yield = System.Collections.Generic.IEnumerator<MindTouch.Tasking.IYield>;
using FoireMuses.Core.Exceptions;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace FoireMuses.Core.Controllers
{

	public class ScoreController : IScoreController
	{

		private static readonly log4net.ILog theLogger = log4net.LogManager.GetLogger(typeof(ScoreController));

		public Result<SearchResult<IScore>> GetScoresFromSource(int offset, int max, ISource aJSource, Result<SearchResult<IScore>> aResult)
		{
			Context.Current.Instance.StoreController.ScoresFromSource(offset, max, aJSource, new Result<SearchResult<IScore>>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IScore> Create(IScore aDoc, Result<IScore> aResult)
		{
			Context.Current.Instance.StoreController.CreateScore((IScore)aDoc, new Result<IScore>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IScore> Get(string id, Result<IScore> aResult)
		{
			Context.Current.Instance.StoreController.GetScoreById(id, new Result<IScore>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IScore> Get(IScore aDoc, Result<IScore> aResult)
		{
			Context.Current.Instance.StoreController.GetScore(aDoc, new Result<IScore>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		private Yield UpdateHelper(IScore aDoc, Result<IScore> aResult)
		{
			yield return CheckAuthorization(aDoc, new Result());
			//if we reach there we have the update rights.
			Result<IScore> IScore = new Result<IScore>();
			yield return Context.Current.Instance.StoreController.UpdateScore(aDoc, new Result<IScore>());
			//finally return the IScore updated
			aResult.Return(IScore.Value);
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
			JUser current = Context.Current.User;
			Result<IScore> result = new Result<IScore>();
			Get(aDoc, result).Wait();
			if (result.Value != null && result.Value.CreatorId == current.Id)
				return true;
			return false;
		}

		private bool IsCollaborator(IScore aDoc)
		{
			JUser current = Context.Current.User;
			JToken groupsId;
			JArray groups = new JArray();
			if (current.TryGetValue("groupsId", out groupsId)) // get the groups of the current user
			{
				groups = groupsId.Value<JArray>();
			}
			Result<IScore> result = new Result<IScore>();
			this.Get(aDoc, result).Wait();
			if (result.Value != null)
				foreach (string collab in result.Value.CollaboratorsId)
				{
					if (groups.Contains(collab) || collab == current.Id)
						return true;
				}
			return false;
		}

		public Result<IScore> Update(IScore aDoc, Result<IScore> aResult)
		{
			Coroutine.Invoke(UpdateHelper, aDoc, new Result<IScore>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<bool> Delete(IScore aDoc, Result<bool> aResult)
		{
			Context.Current.Instance.StoreController.DeleteScore(aDoc, new Result<bool>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<SearchResult<IScore>> GetAll(int offset, int max, Result<SearchResult<IScore>> aResult)
		{
			Context.Current.Instance.StoreController.GetAllScores(offset, max, new Result<SearchResult<IScore>>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}
	}
}

