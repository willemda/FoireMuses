using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core.Utils;
using MindTouch.Tasking;
using Newtonsoft.Json.Linq;
using ICSharpCode.SharpZipLib.Zip;
using MindTouch.IO;
using System.IO;
using System.Text.RegularExpressions;

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

		#region BaseController
		public ISource CreateNew()
		{
			return theSourceDataMapper.CreateNew();
		}
		public Result<ISource> Insert(ISource aDoc, Result<ISource> aResult)
		{
			theSourceDataMapper.Create(aDoc, aResult);
			return aResult;
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
		public Result<ISource> Update(string aSourceId, string rev, ISource aDoc, Result<ISource> aResult)
		{
			ArgCheck.NotNullNorEmpty("aSourceId", aSourceId);
			ArgCheck.NotNull("aSource", aDoc);

			Coroutine.Invoke(UpdateHelper, aSourceId, rev, aDoc, aResult);
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
		public Result<bool> AddAttachment(string id, Stream file,long anAttachmentLength, string fileName, Result<bool> aResult)
		{
			theSourceDataMapper.AddAttachment(id, file, anAttachmentLength, fileName, new Result<bool>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}
		public Result<Stream> GetAttachment(string aDocumentId, string aFileName, Result<Stream> aResult)
		{
			throw new NotImplementedException();
		}
		public Result<bool> DeleteAttachment(string aDocumentId, string aFileName, Result<bool> aResult)
		{
			throw new NotImplementedException();
		}
		public Result<bool> Delete(string aSourceId, string rev, Result<bool> aResult)
		{
			ArgCheck.NotNullNorEmpty("aSourceId", aSourceId);

			Coroutine.Invoke(DeleteHelper, aSourceId, rev, aResult);
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
		#endregion

		#region Private Methods
		private Yield UpdateHelper(string id, string rev, ISource aDoc, Result<ISource> aResult)
		{
			//Check if a source with this id exists.
			Result<ISource> validSourceResult = new Result<ISource>();
			yield return theSourceDataMapper.Retrieve(id, validSourceResult);
			if (validSourceResult.Value == null)
			{
				aResult.Throw(new ArgumentException(String.Format("Source not found for id '{0}'",id)));
				yield break;
			}

			//Update and return the updated source.
			Result<ISource> sourceResult = new Result<ISource>();
			yield return theSourceDataMapper.Update(id, rev ?? validSourceResult.Value.Rev , aDoc, sourceResult);
			aResult.Return(sourceResult.Value);
		}
		private Yield DeleteHelper(string id, string rev, Result<bool> aResult)
		{
			//Check if a source with this id exists.
			Result<ISource> validSourceResult = new Result<ISource>();
			yield return theSourceDataMapper.Retrieve(id, validSourceResult);
			if (validSourceResult.Value == null)
			{
				aResult.Throw(new ArgumentException(String.Format("Source not found for id '{0}'", id)));
				yield break;
			}

			yield return theSourceDataMapper.Delete(id, rev ?? validSourceResult.Value.Rev, aResult);
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
		#endregion
	}
}
