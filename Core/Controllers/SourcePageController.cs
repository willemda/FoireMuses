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
	using System.IO;

	public class SourcePageController : ISourcePageController
	{

		private static readonly log4net.ILog theLogger = log4net.LogManager.GetLogger(typeof(ScoreController));

		private readonly ISourcePageDataMapper theSourcePageDataMapper;

		public SourcePageController(ISourcePageDataMapper aController)
		{
			theSourcePageDataMapper = aController;
		}


		public Result<bool> AddAttachment(string id, Stream file, string fileName, Result<bool> aResult)
		{
			theSourcePageDataMapper.AddAttachment(id, file, fileName, new Result<bool>()).WhenDone(
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

		public Result<bool> AddFascimile(string id, Stream file, Result<bool> aResult)
		{
			this.Retrieve(id, new Result<ISourcePage>()).WhenDone(
				a => {
					this.AddAttachment(id, file, "$fascimile_" + a.PageNumber, new Result<bool>()).WhenDone(
						aResult.Return,
						aResult.Throw
						);
				},
				aResult.Throw
				);
			return aResult;
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

			//Check that the sources aren't null and does exists
			yield return Coroutine.Invoke(CheckSource, aDoc, new Result());

			//Insert a the score and return
			Result<ISourcePage> resultCreate = new Result<ISourcePage>();
			yield return theSourcePageDataMapper.Create(aDoc, resultCreate);
			aResult.Return(resultCreate.Value);
		}

		public Result<ISourcePage> Insert(ISourcePage aDoc, Result<ISourcePage> aResult)
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

			//Check if the source in the SourcePage exist and are not null.
			Coroutine.Invoke(CheckSource, aDoc, new Result());

			//Update and return the updated SourcePage.
			Result<ISourcePage> SourcePageResult = new Result<ISourcePage>();
			yield return theSourcePageDataMapper.Update(id, rev, aDoc, SourcePageResult);
			aResult.Return(SourcePageResult.Value);
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

		public Result<SearchResult<ISourcePage>> GetPagesFromSource(
			string aSourceId, 
			int anOffset, 
			int aMax, 
			Result<SearchResult<ISourcePage>> aResult)
		{
			ArgCheck.NotNullNorEmpty("sourceId", aSourceId);
			ArgCheck.SuperiorOrEqualsTo0("offset", anOffset);
			ArgCheck.SuperiorOrEqualsTo0("max", aMax);

			theSourcePageDataMapper.GetPagesFromSource(anOffset, aMax, aSourceId, aResult);

			return aResult;
		}
		public ISourcePage CreateNew()
		{
			return theSourcePageDataMapper.CreateNew();
		}

	}
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              