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
    using MindTouch.Dream;

	public class PlayController : IPlayController
	{

		private static readonly log4net.ILog theLogger = log4net.LogManager.GetLogger(typeof(ScoreController));

		private readonly IPlayDataMapper thePlayDataMapper;

		public PlayController(IPlayDataMapper aController)
		{
			thePlayDataMapper = aController;
		}


        private Yield CreateHelper(IPlay aDoc, Result<IPlay> aResult)
        {
            Result<bool> resultExists = new Result<bool>();
            yield return Context.Current.Instance.SourceController.Exists(aDoc.SourceId, resultExists);
            if (resultExists.HasValue && resultExists.Value)
            {
                Result<IPlay> resultCreate = new Result<IPlay>();
                yield return thePlayDataMapper.Create(aDoc, resultCreate);
                aResult.Return(resultCreate.Value);
            }
            else
                aResult.Throw(new DreamNotFoundException("Source not found"));
        }
		public Result<IPlay> Create(IPlay aDoc, Result<IPlay> aResult)
		{
			if (Context.Current.User == null)
				throw new UnauthorizedAccessException();

            Coroutine.Invoke(CreateHelper, aDoc, new Result<IPlay>()).WhenDone(
                aResult.Return,
                aResult.Throw
                );
            return aResult;
		}

        private Yield UpdateHelper(string id, string rev, IPlay aDoc, Result<IPlay> aResult)
        {
            Result<IPlay> validPlayResult = new Result<IPlay>();
            yield return thePlayDataMapper.Retrieve(aDoc.Id, validPlayResult);
            if (validPlayResult.Value == null)
                aResult.Throw(new DreamNotFoundException("Play not found"));
            CheckAuthorization(validPlayResult.Value);
            //if we reach there we have the update rights.

            //check if source exist
            Result<bool> resultExists = new Result<bool>();
			yield return Context.Current.Instance.SourceController.Exists(aDoc.SourceId, resultExists);
            if (resultExists.HasValue && resultExists.Value)
            {
                Result<IPlay> playResult = new Result<IPlay>();
                yield return thePlayDataMapper.Update(id, rev, aDoc, new Result<IPlay>());
                //finally return the IScore updated
                aResult.Return(playResult.Value);
                yield break;
            }else
                aResult.Throw(new DreamNotFoundException("Source not found"));
        }

        public void CheckAuthorization(IPlay aDoc)
        {
            if (Context.Current.User == null || (!IsCreator(aDoc) && !IsCollaborator(aDoc)))
                throw new UnauthorizedAccessException();
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

        public Result<IPlay> Update(string id, string rev, IPlay aDoc, Result<IPlay> aResult)
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
