﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using MindTouch.Tasking;
using Newtonsoft.Json.Linq;

namespace FoireMuses.Core.Controllers
{
	using Yield = System.Collections.Generic.IEnumerator<MindTouch.Tasking.IYield>;
	using MindTouch.Dream;
	using FoireMuses.Core.Utils;

	public class SourcePageController : ISourcePageController
	{

		private static readonly log4net.ILog theLogger = log4net.LogManager.GetLogger(typeof(ScoreController));

		private readonly ISourcePageDataMapper theSourcePageDataMapper;

		public SourcePageController(ISourcePageDataMapper aController)
		{
			theSourcePageDataMapper = aController;
		}


		private Yield CheckSource(ISourcePage aSourcePage, Result aResult)
		{
			//can't be null, it must have a source
			if (aSourcePage.SourceId == null)
			{
				aResult.Throw(new ArgumentException());
				yield break;
			}

			// if this source exists
			Result<bool> validSourceResult = new Result<bool>();
			yield return Context.Current.Instance.SourceController.Exists(aSourcePage.SourceId, validSourceResult);
			if (!validSourceResult.Value)
			{
				aResult.Throw(new ArgumentException());
				yield break;
			}
			aResult.Return();
		}

		private Yield CreateHelper(ISourcePage aDoc, Result<ISourcePage> aResult)
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
			Result<ISourcePage> resultCreate = new Result<ISourcePage>();
			yield return theSourcePageDataMapper.Create(aDoc, resultCreate);
			aResult.Return(resultCreate.Value);
		}

		public Result<ISourcePage> Create(ISourcePage aDoc, Result<ISourcePage> aResult)
		{
			ArgCheck.NotNull("aDoc", aDoc);
			Coroutine.Invoke(CreateHelper, aDoc, new Result<ISourcePage>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		private Yield UpdateHelper(string id, string rev, ISourcePage aDoc, Result<ISourcePage> aResult)
		{
			//Check if a SourcePage with this id exists.
			Result<ISourcePage> validSourcePageResult = new Result<ISourcePage>();
			yield return theSourcePageDataMapper.Retrieve(aDoc.Id, validSourcePageResult);
			if (validSourcePageResult.Value == null)
			{
				aResult.Throw(new ArgumentException());
				yield break;
			}

			//Check if the current user has the update rights.
			if (!HasAuthorization(validSourcePageResult.Value))
			{
				aResult.Throw(new UnauthorizedAccessException());
				yield break;
			}

			//Check if the source in the SourcePage exist and are not null.
			Coroutine.Invoke(CheckSource, aDoc, new Result());

			//Update and return the updated SourcePage.
			Result<ISourcePage> SourcePageResult = new Result<ISourcePage>();
			yield return theSourcePageDataMapper.Update(id, rev, aDoc, SourcePageResult);
			aResult.Return(SourcePageResult.Value);
		}

		private bool HasAuthorization(ISourcePage aDoc)
		{
			//Check if user isn't set, or if he isn't creator or collaborator.
			if (Context.Current.User == null || (!IsCreator(aDoc) && !IsCollaborator(aDoc)))
				return false;
			return true;
		}

		private bool IsCreator(ISourcePage aDoc)
		{
			return aDoc.CreatorId == Context.Current.User.Id;
		}

		private bool IsCollaborator(ISourcePage aDoc)
		{
			IUser current = Context.Current.User;

			foreach (string collab in aDoc.CollaboratorsId)
			{
				if (current.Groups.Contains(collab) || collab == current.Id)
					return true;
			}
			return false;
		}

		public Result<ISourcePage> Update(string id,string rev, ISourcePage aDoc, Result<ISourcePage> aResult)
		{
			ArgCheck.NotNull("aDoc", aDoc);
			ArgCheck.NotNullNorEmpty("id", id);
			ArgCheck.NotNullNorEmpty("rev", rev);

			Coroutine.Invoke(UpdateHelper, id, rev, aDoc, new Result<ISourcePage>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<ISourcePage> Retrieve(string id, Result<ISourcePage> aResult)
		{
			ArgCheck.NotNullNorEmpty("id", id);
			theSourcePageDataMapper.Retrieve(id, new Result<ISourcePage>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<bool> Delete(string id, string rev, Result<bool> aResult)
		{
			ArgCheck.NotNullNorEmpty("id", id);
			ArgCheck.NotNullNorEmpty("rev", rev);

			theSourcePageDataMapper.Delete(id, rev, new Result<bool>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<SearchResult<ISourcePage>> GetAll(int offset, int max, Result<SearchResult<ISourcePage>> aResult)
		{
			ArgCheck.SuperiorOrEqualsTo0("offset",offset);
			ArgCheck.SuperiorOrEqualsTo0("max", max);

			theSourcePageDataMapper.GetAll(offset, max, new Result<SearchResult<ISourcePage>>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public ISourcePage FromJson(string aJson)
		{
			ArgCheck.NotNullNorEmpty("aJson",aJson);
			return theSourcePageDataMapper.FromJson(aJson);
		}

		public string ToJson(ISourcePage anObject)
		{
			ArgCheck.NotNull("anObject", anObject);
			return theSourcePageDataMapper.ToJson(anObject);
		}

		public string ToJson(SearchResult<ISourcePage> aSearchResult)
		{
			ArgCheck.NotNull("aSearchResult", aSearchResult);
			return theSourcePageDataMapper.ToJson(aSearchResult);
		}


		public ISourcePage FromXml(MindTouch.Xml.XDoc aXml)
		{
			throw new NotImplementedException();
		}

		public MindTouch.Xml.XDoc ToXml(ISourcePage anObject)
		{
			throw new NotImplementedException();
		}


		public Result<bool> Exists(string id, Result<bool> aResult)
		{
			ArgCheck.NotNullNorEmpty("id", id);

			this.Retrieve(id, new Result<ISourcePage>()).WhenDone(
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

		private Yield GetSourcePagesFromSourceHelper(int offset, int max, string aSourceId, Result<SearchResult<ISourcePage>> aResult)
		{
			//Check if the SourcePage exists
			Result<bool> resultExists = new Result<bool>();
			yield return Context.Current.Instance.SourceController.Exists(aSourceId, resultExists);
			if (resultExists.Value)
			{
				//Get the SourcePages that have this source id as textual or musical source.
				Result<SearchResult<ISourcePage>> resultSearch = new Result<SearchResult<ISourcePage>>();
				yield return theSourcePageDataMapper.GetPagesFromSource(offset, max, aSourceId, resultSearch);
				aResult.Return(resultSearch.Value);
			}
			else
				aResult.Throw(new DreamNotFoundException("Source not found"));
		}

		public Result<SearchResult<ISourcePage>> GetPagesFromSource(int offset, int max, string sourceId, Result<SearchResult<ISourcePage>> aResult)
		{
			ArgCheck.NotNullNorEmpty("sourceId", sourceId);
			ArgCheck.SuperiorOrEqualsTo0("offset", offset);
			ArgCheck.SuperiorOrEqualsTo0("max", max);

			Coroutine.Invoke(GetSourcePagesFromSourceHelper, offset, max, sourceId, new Result<SearchResult<ISourcePage>>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}
		public ISourcePage CreateNew()
		{
			return theSourcePageDataMapper.CreateNew();
		}

	}
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              