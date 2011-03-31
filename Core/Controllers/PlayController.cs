using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using MindTouch.Tasking;
using Newtonsoft.Json.Linq;

namespace FoireMuses.Core.Controllers
{
	using Yield = System.Collections.Generic.IEnumerator<MindTouch.Tasking.IYield>;

	public class PlayController : IPlayController
	{

		private static readonly log4net.ILog theLogger = log4net.LogManager.GetLogger(typeof(ScoreController));

		private readonly IPlayDataMapper thePlayDataMapper;

		public PlayController(IPlayDataMapper aController)
		{
			thePlayDataMapper = aController;
		}


		private Yield CheckSource(IPlay aPlay, Result aResult)
		{
			//can't be null, it must have a source
			if (aPlay.SourceId == null)
			{
				aResult.Throw(new ArgumentException());
				yield break;
			}

			// if this source exists
			Result<bool> validSourceResult = new Result<bool>();
			yield return Context.Current.Instance.SourceController.Exists(aPlay.SourceId, validSourceResult);
			if (!validSourceResult.Value)
			{
				aResult.Throw(new ArgumentException());
				yield break;
			}
			aResult.Return();
		}

		public Yield CreateHelper(IPlay aDoc, Result<IPlay> aResult)
		{
			//Check if user is set as we need to know the creator.
			if (Context.Current.User == null)
			{
				aResult.Throw(new UnauthorizedAccessException());
				yield break;
			}

			//Check that the sources aren't null and does exists
			yield return Coroutine.Invoke(CheckSource, aDoc, new Result());

			//Create a the score and return
			Result<IPlay> resultCreate = new Result<IPlay>();
			yield return thePlayDataMapper.Create(aDoc, resultCreate);
			aResult.Return(resultCreate.Value);
		}

		public Result<IPlay> Create(IPlay aDoc, Result<IPlay> aResult)
		{
			Coroutine.Invoke(CreateHelper, aDoc, new Result<IPlay>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		private Yield UpdateHelper(string id, string rev, IPlay aDoc, Result<IPlay> aResult)
		{
			//Check if a play with this id exists.
			Result<IPlay> validPlayResult = new Result<IPlay>();
			yield return thePlayDataMapper.Retrieve(aDoc.Id, validPlayResult);
			if (validPlayResult.Value == null)
			{
				aResult.Throw(new ArgumentException());
				yield break;
			}

			//Check if the current user has the update rights.
			if (!HasAuthorization(validPlayResult.Value))
			{
				aResult.Throw(new UnauthorizedAccessException());
				yield break;
			}

			//Check if the source in the play exist and are not null.
			Coroutine.Invoke(CheckSource, aDoc, new Result());

			//Update and return the updated play.
			Result<IPlay> playResult = new Result<IPlay>();
			yield return thePlayDataMapper.Update(id, rev, aDoc, playResult);
			aResult.Return(playResult.Value);
		}

		public bool HasAuthorization(IPlay aDoc)
		{
			//Check if user isn't set, or if he isn't creator or collaborator.
			if (Context.Current.User == null || (!IsCreator(aDoc) && !IsCollaborator(aDoc)))
				return false;
			return true;
		}

		private bool IsCreator(IPlay aDoc)
		{
			return aDoc.CreatorId == Context.Current.User.Id;
		}

		private bool IsCollaborator(IPlay aDoc)
		{
			IUser current = Context.Current.User;

			foreach (string collab in aDoc.CollaboratorsId)
			{
				if (current.Groups.Contains(collab) || collab == current.Id)
					return true;
			}
			return false;
		}

		public Result<IPlay> Update(string id,string rev, IPlay aDoc, Result<IPlay> aResult)
		{
			Coroutine.Invoke(UpdateHelper, id, rev, aDoc, new Result<IPlay>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IPlay> Retrieve(string id, Result<IPlay> aResult)
		{
			thePlayDataMapper.Retrieve(id, new Result<IPlay>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<bool> Delete(string id, string rev, Result<bool> aResult)
		{
			thePlayDataMapper.Delete(id, rev, new Result<bool>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<SearchResult<IPlay>> GetAll(int offset, int max, Result<SearchResult<IPlay>> aResult)
		{
			throw new NotImplementedException();
		}

		public IPlay FromJson(string aJson)
		{
			return thePlayDataMapper.FromJson(aJson);
		}

		public string ToJson(IPlay anObject)
		{
			return thePlayDataMapper.ToJson(anObject);
		}


		public IPlay FromXml(MindTouch.Xml.XDoc aXml)
		{
			throw new NotImplementedException();
		}

		public MindTouch.Xml.XDoc ToXml(IPlay anObject)
		{
			throw new NotImplementedException();
		}


		public Result<bool> Exists(string id, Result<bool> aResult)
		{
			this.Retrieve(id, new Result<IPlay>()).WhenDone(
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

		public Result<SearchResult<IPlay>> GetPlaysFromSource(int offset, int max, string sourceId, Result<SearchResult<IPlay>> aResult)
		{
			thePlayDataMapper.GetPlaysFromSource(offset, max, sourceId, new Result<SearchResult<IPlay>>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}
	}
}
