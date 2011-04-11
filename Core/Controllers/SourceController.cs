using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core.Utils;
using MindTouch.Tasking;
using Newtonsoft.Json.Linq;

namespace FoireMuses.Core.Controllers
{
	using Yield = System.Collections.Generic.IEnumerator<MindTouch.Tasking.IYield>;

	public class SourceController : ISourceController
	{
		private static readonly log4net.ILog theLogger = log4net.LogManager.GetLogger(typeof(ScoreController));

		private readonly ISourceDataMapper theSourceDataMapper;

		public SourceController(ISourceDataMapper aController)
		{
			theSourceDataMapper = aController;
		}

		public Yield CreateHelper(ISource aDoc, Result<ISource> aResult)
		{
			//Check if user is set as we need to know the creator.
			if (Context.Current.User == null)
			{
				aResult.Throw(new UnauthorizedAccessException());
				yield break;
			}

			//Create a the source and return
			Result<ISource> resultCreate = new Result<ISource>();
			yield return theSourceDataMapper.Create(aDoc, resultCreate);
			aResult.Return(resultCreate.Value);
		}

		public ISource CreateNew()
		{
			return theSourceDataMapper.CreateNew();
		}

		public Result<ISource> Create(ISource aDoc, Result<ISource> aResult)
		{
			Coroutine.Invoke(CreateHelper,aDoc, new Result<ISource>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		private Yield UpdateHelper(string id, string rev, ISource aDoc, Result<ISource> aResult)
		{
			//Check if a source with this id exists.
			Result<ISource> validSourceResult = new Result<ISource>();
			yield return theSourceDataMapper.Retrieve(aDoc.Id, validSourceResult);
			if (validSourceResult.Value == null)
			{
				aResult.Throw(new ArgumentException());
				yield break;
			}

			//Check if the current user has the update rights.
			if (!HasAuthorization(validSourceResult.Value))
			{
				aResult.Throw(new UnauthorizedAccessException());
				yield break;
			}

			//Update and return the updated source.
			Result<ISource> sourceResult = new Result<ISource>();
			yield return theSourceDataMapper.Update(id, rev, aDoc, sourceResult);
			aResult.Return(sourceResult.Value);
		}

		public string ToJson(SearchResult<ISource> aSearchResult)
		{
			return theSourceDataMapper.ToJson(aSearchResult);
		}

		public bool HasAuthorization(ISource aDoc)
		{
			//Check if user isn't set, or if he isn't creator or collaborator.
			if (Context.Current.User == null || (!IsCreator(aDoc) && !IsCollaborator(aDoc)))
				return false;
			return true;
		}

		private bool IsCreator(ISource aDoc)
		{
			return aDoc.CreatorId == Context.Current.User.Id;
		}

		private bool IsCollaborator(ISource aDoc)
		{
			IUser current = Context.Current.User;

			foreach (string collab in aDoc.CollaboratorsId)
			{
				if (current.Groups.Contains(collab) || collab == current.Id)
					return true;
			}
			return false;
		}

		public Result<ISource> Update(string id,string rev,ISource aDoc, Result<ISource> aResult)
		{
			Coroutine.Invoke(UpdateHelper,id, rev, aDoc, new Result<ISource>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<ISource> Retrieve(string id, Result<ISource> aResult)
		{
			theSourceDataMapper.Retrieve(id, new Result<ISource>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<bool> Delete(string id, string rev, Result<bool> aResult)
		{
			theSourceDataMapper.Delete(id, rev, new Result<bool>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<SearchResult<ISource>> GetAll(int offset, int max, Result<SearchResult<ISource>> aResult)
		{
			theSourceDataMapper.GetAll(offset, max, new Result<SearchResult<ISource>>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public ISource FromJson(string aJson)
		{
			return theSourceDataMapper.FromJson(aJson);
		}

		public string ToJson(ISource aJson)
		{
			return theSourceDataMapper.ToJson(aJson);
		}


		public ISource FromXml(MindTouch.Xml.XDoc aXml)
		{
			throw new NotImplementedException();
		}

		public MindTouch.Xml.XDoc ToXml(ISource anObject)
		{
			throw new NotImplementedException();
		}


		public Result<bool> Exists(string id, Result<bool> aResult)
		{
			this.Retrieve(id, new Result<ISource>()).WhenDone(
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
	}
}
