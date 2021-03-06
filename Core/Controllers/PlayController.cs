﻿using System;
using System.IO;
using System.Linq;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core.Utils;
using MindTouch.Dream;
using MindTouch.Tasking;

namespace FoireMuses.Core.Controllers
{
	using Yield = System.Collections.Generic.IEnumerator<IYield>;

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

		private Yield CreateHelper(IPlay aDoc, Result<IPlay> aResult)
		{
			//Check if user is set as we need to know the creator.
			if (Context.Current.User == null)
			{
				aResult.Throw(new UnauthorizedAccessException());
				yield break;
			}

			//Check that the sources aren't null and does exists
			yield return Coroutine.Invoke(CheckSource, aDoc, new Result());

			//Insert a the score and return
			Result<IPlay> resultCreate = new Result<IPlay>();
			yield return thePlayDataMapper.Create(aDoc, resultCreate);
			aResult.Return(resultCreate.Value);
		}

		public Result<IPlay> Insert(IPlay aDoc, Result<IPlay> aResult)
		{
			ArgCheck.NotNull("aDoc", aDoc);
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

		private bool HasAuthorization(IPlay aDoc)
		{
			//Check if user isn't set, or if he isn't creator or collaborator.
			if (Context.Current.User == null || (!IsCreator(aDoc) && !IsCollaborator(aDoc)))
				return false;
			return true;
		}

		private bool IsCreator(IPlay aDoc)
		{
			return true;// aDoc.CreatorId == Context.Current.User.Id;
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
			ArgCheck.NotNull("aDoc", aDoc);
			ArgCheck.NotNullNorEmpty("id", id);
			ArgCheck.NotNullNorEmpty("rev", rev);
			Coroutine.Invoke(UpdateHelper, id, rev, aDoc, new Result<IPlay>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IPlay> Retrieve(string id, Result<IPlay> aResult)
		{
			ArgCheck.NotNullNorEmpty("id", id);
			thePlayDataMapper.Retrieve(id, new Result<IPlay>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<bool> Delete(string id, string rev, Result<bool> aResult)
		{
			ArgCheck.NotNullNorEmpty("id", id);
			ArgCheck.NotNullNorEmpty("rev", rev);

			thePlayDataMapper.Delete(id, rev, new Result<bool>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<SearchResult<IPlay>> GetAll(int offset, int max, Result<SearchResult<IPlay>> aResult)
		{
			ArgCheck.SuperiorOrEqualsTo0("offset",offset);
			ArgCheck.SuperiorOrEqualsTo0("max", max);

			thePlayDataMapper.GetAll(offset, max, new Result<SearchResult<IPlay>>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<bool> AddAttachment(string aDocumentId, Stream aStream, long anAttachmentLength, string aFileName, Result<bool> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<Stream> GetAttachment(string aDocumentId, string aFileName, Result<Stream> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<bool> DeleteAttachment(string aDocumentId, string aFileName, Result<bool> aResult)
		{
			throw new NotImplementedException();
		}

		public IPlay FromJson(string aJson)
		{
			ArgCheck.NotNullNorEmpty("aJson",aJson);
			return thePlayDataMapper.FromJson(aJson);
		}

		public string ToJson(IPlay anObject)
		{
			ArgCheck.NotNull("anObject", anObject);
			return thePlayDataMapper.ToJson(anObject);
		}

		public string ToJson(SearchResult<IPlay> aSearchResult)
		{
			ArgCheck.NotNull("aSearchResult", aSearchResult);
			return thePlayDataMapper.ToJson(aSearchResult);
		}

		public Result<bool> Exists(string id, Result<bool> aResult)
		{
			ArgCheck.NotNullNorEmpty("id", id);

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

		private Yield GetPlaysFromSourceHelper(int offset, int max, string aSourceId, Result<SearchResult<IPlay>> aResult)
		{
			//Check if the play exists
			Result<bool> resultExists = new Result<bool>();
			yield return Context.Current.Instance.SourceController.Exists(aSourceId, resultExists);
			if (resultExists.Value)
			{
				//Get the plays that have this source id as textual or musical source.
				Result<SearchResult<IPlay>> resultSearch = new Result<SearchResult<IPlay>>();
				yield return thePlayDataMapper.GetPlaysFromSource(offset, max, aSourceId, resultSearch);
				aResult.Return(resultSearch.Value);
			}
			else
				aResult.Throw(new DreamNotFoundException("Source not found"));
		}

		public Result<SearchResult<IPlay>> GetPlaysFromSource(int offset, int max, string sourceId, Result<SearchResult<IPlay>> aResult)
		{
			ArgCheck.NotNullNorEmpty("sourceId", sourceId);
			ArgCheck.SuperiorOrEqualsTo0("offset", offset);
			ArgCheck.SuperiorOrEqualsTo0("max", max);

			Coroutine.Invoke(GetPlaysFromSourceHelper, offset, max, sourceId, new Result<SearchResult<IPlay>>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public IPlay CreateNew()
		{
			return thePlayDataMapper.CreateNew();
		}
	}
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              