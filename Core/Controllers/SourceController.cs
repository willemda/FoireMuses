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

namespace FoireMuses.Core.Controllers
{
	using Yield = System.Collections.Generic.IEnumerator<MindTouch.Tasking.IYield>;
	using System.IO;
	using System.Text.RegularExpressions;

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

			//Insert a the source and return
			Result<ISource> resultCreate = new Result<ISource>();
			yield return theSourceDataMapper.Create(aDoc, resultCreate);
			aResult.Return(resultCreate.Value);
		}

		public ISource CreateNew()
		{
			return theSourceDataMapper.CreateNew();
		}

		public Result<ISource> Insert(ISource aDoc, Result<ISource> aResult)
		{
			Coroutine.Invoke(CreateHelper, aDoc, new Result<ISource>()).WhenDone(
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

		public Result<ISource> Update(string id, string rev, ISource aDoc, Result<ISource> aResult)
		{
			Coroutine.Invoke(UpdateHelper, id, rev, aDoc, new Result<ISource>()).WhenDone(
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


		public Result<bool> AddAttachment(string id, Stream file, string fileName, Result<bool> aResult)
		{
			theSourceDataMapper.AddAttachment(id, file, fileName, new Result<bool>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}


		public Result<bool> BulkFascimile(string sourceId, Stream file, Result<bool> aResult)
		{
			Stream valid = new MemoryStream();
			file.CopyTo(valid, file.Length);
			valid.Position = 0;
			using (ZipInputStream zis = new ZipInputStream(valid))
			{
				ZipEntry entry;
				while ((entry = zis.GetNextEntry()) != null)
				{
					if (!entry.IsFile)
					{
						aResult.Return(false);
						return aResult;
					}
					string reg = @"\$facsimile_(\d+)\.jpg";
					Match match = Regex.Match(entry.Name, reg);
					if (!match.Success)
					{
						aResult.Return(false);
						return aResult;
					}
				}
			}
			file.Position = 0;
			using (ZipInputStream zis = new ZipInputStream(file))
			{
				ZipEntry entry;
				while ((entry = zis.GetNextEntry()) != null)
				{
					Match match = Regex.Match(entry.Name, @"\$facsimile_(\d+)\.jpg");
					string number;
					number = match.Groups[1].Value;
					ISourcePage page = Context.Current.Instance.SourcePageController.CreateNew();
					page.PageNumber = int.Parse(number);
					page.DisplayPageNumber = int.Parse(number);
					page.SourceId = sourceId;
					page = Context.Current.Instance.SourcePageController.Insert(page, new Result<ISourcePage>()).Wait();
					Stream fasc = new MemoryStream();
					zis.CopyTo(fasc, zis.Length);
					fasc.Position = 0;
					Context.Current.Instance.SourcePageController.AddFascimile(page.Id, fasc, new Result<bool>()).Wait();
				}
			}
			aResult.Return(true);
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

		/*
		public Yield AddCollaboratorHelper(string sourceId, string userId, Result<ISource> aResult)
		{
			if (!Context.Current.User.IsAdmin)
			{
				aResult.Throw(new UnauthorizedAccessException("You must be admin to change the rights"));
				yield break;
			}

			Result<IUser> resultUser = new Result<IUser>();
			yield return Context.Current.Instance.UserController.Retrieve(userId, resultUser);
			if (resultUser.Value == null)
			{
				aResult.Throw(new ArgumentException());
				yield break;
			}

			Result<ISource> result = new Result<ISource>();
			yield return this.Retrieve(sourceId, result);
			if (result.Value == null)
			{
				aResult.Throw(new ArgumentException());
				yield break;
			}
			ISource source = result.Value;
			IList<string> listeCollab = source.CollaboratorsId;
			listeCollab.Add(userId);
			source.CollaboratorsId = listeCollab;

			Result<ISource> resultUpdated = new Result<ISource>();
			yield return this.Update(source.Id, source.Rev, source, resultUpdated);
			aResult.Return(resultUpdated.Value);
		}


		public Result<ISource> AddCollaborator(string sourceId, string userId, Result<ISource> aResult)
		{
			Coroutine.Invoke(AddCollaboratorHelper, sourceId, userId, new Result<ISource>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}
		 */
	}
}
