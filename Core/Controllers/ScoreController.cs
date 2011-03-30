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
using MindTouch.Xml;
using MindTouch.Dream;

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

		private Yield GetScoresFromSourceHelper(int offset, int max, string aSourceId, Result<SearchResult<IScore>> aResult)
		{
			Result<bool> resultExists = new Result<bool>();
			yield return Context.Current.Instance.SourceController.Exists(aSourceId, resultExists);
			if (resultExists.HasValue && resultExists.Value)
			{
				Result<SearchResult<IScore>> resultSearch = new Result<SearchResult<IScore>>();
				yield return theScoreDataMapper.ScoresFromSource(offset, max, aSourceId, resultSearch);
				if (resultSearch.HasValue)
					aResult.Return(resultSearch.Value);
			} else
				aResult.Throw(new DreamNotFoundException("Source not found"));
		}

		public Result<SearchResult<IScore>> GetScoresFromSource(int offset, int max, string aSourceId, Result<SearchResult<IScore>> aResult)
		{
			ArgCheck.NotNull("aSourceId", aSourceId);
			Coroutine.Invoke(GetScoresFromSourceHelper, offset, max, aSourceId, new Result<SearchResult<IScore>>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}


		public Yield CreateHelper(IScore aDoc, Result<IScore> aResult)
		{
			Result<bool> resultCheckSource = new Result<bool>();
			CheckSources(aDoc, resultCheckSource);
			Result<IScore> resultCreate = new Result<IScore>();
			yield return theScoreDataMapper.Create(aDoc, resultCreate);
		}

		private Yield CheckSources(IScore aScore, Result<bool> aResult)
		{
			if (aScore.TextualSource != null)
			{
				if (aScore.TextualSource.SourceId == null)
				{
					aResult.Throw(new ArgumentException());
					yield break;
				}
				Result<bool> validSourceResult = new Result<bool>();
				yield return Context.Current.Instance.SourceController.Exists(aScore.TextualSource.SourceId, validSourceResult);
				if (!validSourceResult.Value)
				{
					aResult.Throw(new ArgumentException());
					yield break;
				}
			}

			if (aScore.MusicalSource != null)
			{
				if (aScore.MusicalSource.SourceId == null)
				{
					aResult.Throw(new ArgumentException());
					yield break;
				}
				Result<bool> validSourceResult = new Result<bool>();
				yield return Context.Current.Instance.SourceController.Exists(aScore.MusicalSource.SourceId, validSourceResult);
				if (!validSourceResult.Value)
				{
					aResult.Throw(new ArgumentException());
					yield break;
				}
			}
			aResult.Return(true);
		}

		public Result<IScore> Create(IScore aDoc, Result<IScore> aResult)
		{
			if (Context.Current.User == null)
				throw new UnauthorizedAccessException();
			if (aDoc.TextualSource == null && aDoc.MusicalSource == null)
				throw new ArgumentException();

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

		private Yield UpdateHelper(string id, string rev, IScore aDoc, Result<IScore> aResult)
		{
			Result<IScore> validScoreResult = new Result<IScore>();
			yield return theScoreDataMapper.Retrieve(aDoc.Id, validScoreResult);
			if (validScoreResult.Value == null)
				aResult.Throw(new ArgumentException());
			CheckAuthorization(validScoreResult.Value);
			//if we reach there we have the update rights.

			//check if source textuelle relationship exist

			

			Result<IScore> scoreResult = new Result<IScore>();
			yield return theScoreDataMapper.Update(id,rev,aDoc, new Result<IScore>());
			//finally return the IScore updated
			aResult.Return(scoreResult.Value);
			yield break;
		}
		public void CheckAuthorization(IScore aDoc)
		{
			if (Context.Current.User == null || (!IsCreator(aDoc) && !IsCollaborator(aDoc)))
				throw new UnauthorizedAccessException();
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
			Coroutine.Invoke(UpdateHelper, id, rev, aDoc, new Result<IScore>()).WhenDone(
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

		public IScore FromXml(XDoc aXml)
		{
			return theScoreDataMapper.FromXml(aXml);
		}

		public XDoc ToXml(IScore aScore)
		{
			return theScoreDataMapper.ToXml(aScore);
		}
	

		public Result<bool>  Exists(string id, Result<bool> aResult)
		{
 			this.Retrieve(id, new Result<IScore>()).WhenDone(
				a=>{
					if(a != null)
						aResult.Return(true);
					else
						aResult.Return(false);
				},
				aResult.Throw
				);
			return aResult;
		}
	}
}