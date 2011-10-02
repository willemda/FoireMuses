using System;
using System.IO;
using System.Text.RegularExpressions;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core.Utils;
using ICSharpCode.SharpZipLib.Zip;
using MindTouch.IO;
using MindTouch.Tasking;

namespace FoireMuses.Core.Controllers
{
	using Yield = System.Collections.Generic.IEnumerator<MindTouch.Tasking.IYield>;

	public class SourcePageController : ISourcePageController
	{
		private static readonly log4net.ILog theLogger = log4net.LogManager.GetLogger(typeof(ScoreController));

		private readonly ISourcePageDataMapper theSourcePageDataMapper;

		public SourcePageController(ISourcePageDataMapper aController)
		{
			theSourcePageDataMapper = aController;
		}

		#region IBaseController
		public ISourcePage CreateNew()
		{
			return theSourcePageDataMapper.CreateNew();
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
		public Result<ISourcePage> Update(string aSourcePageId, string aSourcePageRev, ISourcePage aDoc, Result<ISourcePage> aResult)
		{
			ArgCheck.NotNull("aDoc", aDoc);
			ArgCheck.NotNullNorEmpty("id", aSourcePageId);
			ArgCheck.NotNullNorEmpty("rev", aSourcePageRev);

			Coroutine.Invoke(UpdateHelper, aSourcePageId, aSourcePageRev, aDoc, new Result<ISourcePage>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}
		public Result<ISourcePage> Retrieve(string aSourcePageId, Result<ISourcePage> aResult)
		{
			ArgCheck.NotNullNorEmpty("id", aSourcePageId);
			theSourcePageDataMapper.Retrieve(aSourcePageId, new Result<ISourcePage>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}
		public Result<bool> Delete(string aSourcePageId, string aSourcePageRev, Result<bool> aResult)
		{
			ArgCheck.NotNullNorEmpty("id", aSourcePageId);
			ArgCheck.NotNullNorEmpty("rev", aSourcePageRev);

			theSourcePageDataMapper.Delete(aSourcePageId, aSourcePageRev, new Result<bool>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}
		public Result<bool> AddAttachment(string aSourcePageId, Stream aFile, long anAttachmentLength, string aFileName, Result<bool> aResult)
		{
			theSourcePageDataMapper.AddAttachment(aSourcePageId, aFile, anAttachmentLength, aFileName, new Result<bool>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}
		public Result<Stream> GetAttachment(string aSourcePageId, string aFileName, Result<Stream> aResult)
		{
			throw new NotImplementedException();
		}
		public Result<bool> DeleteAttachment(string aSourcePageId, string aFileName, Result<bool> aResult)
		{
			throw new NotImplementedException();
		}
		public Result<SearchResult<ISourcePage>> GetAll(int anOffset, int aMaxResults, Result<SearchResult<ISourcePage>> aResult)
		{
			ArgCheck.SuperiorOrEqualsTo0("offset", anOffset);
			ArgCheck.SuperiorOrEqualsTo0("max", aMaxResults);

			theSourcePageDataMapper.GetAll(anOffset, aMaxResults, new Result<SearchResult<ISourcePage>>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}
		public ISourcePage FromJson(string aJson)
		{
			ArgCheck.NotNullNorEmpty("aJson", aJson);
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
		public Result<bool> Exists(string aSourcePageId, Result<bool> aResult)
		{
			ArgCheck.NotNullNorEmpty("id", aSourcePageId);

			this.Retrieve(aSourcePageId, new Result<ISourcePage>()).WhenDone(
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

		#region ISourcePageController
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
		public Result<bool> AddFascimile(string aSourcePageId, Stream aFile, long anAttachmentLength, Result<bool> aResult)
		{
			this.Retrieve(aSourcePageId, new Result<ISourcePage>()).WhenDone(
				a =>
				{
					this.AddAttachment(aSourcePageId, aFile,anAttachmentLength,  "$fascimile_" + a.PageNumber, new Result<bool>()).WhenDone(
						aResult.Return,
						aResult.Throw
						);
				},
				aResult.Throw
				);
			return aResult;
		}
		public Result<bool> BulkImportSourcePages(string aSourcePageId, Stream file, Result<bool> aResult)
		{
			bool valid = TestFacsimileZipStream(file);

			if (!valid)
			{
				aResult.Return(false);
				return aResult;
			}

			file.Position = 0;
			using (ZipInputStream zis = new ZipInputStream(file))
			{
				ZipEntry entry;
				while ((entry = zis.GetNextEntry()) != null)
				{
					if (!entry.IsFile)
						continue;

					Match match = Regex.Match(entry.Name, @"\$facsimile_(\d+)\.jpg");
					string number;
					number = match.Groups[1].Value;
					ISourcePage page = Context.Current.Instance.SourcePageController.CreateNew();
					page.PageNumber = int.Parse(number);
					page.DisplayPageNumber = int.Parse(number);
					page.SourceId = aSourcePageId;
					page = Context.Current.Instance.SourcePageController.Insert(page, new Result<ISourcePage>()).Wait();
					using (Stream fasc = new MemoryStream())
					{
						zis.CopyTo(fasc, zis.Length);
						fasc.Position = 0;
						Context.Current.Instance.SourcePageController.AddFascimile(page.Id, fasc,fasc.Length, new Result<bool>()).Wait();
					}
				}
			}
			aResult.Return(true);
			return aResult;
		} 
		#endregion

		#region Private Methods
		private static bool TestFacsimileZipStream(Stream file)
		{
			bool valid = false;
			using (Stream testStream = new MemoryStream())
			{
				file.CopyTo(testStream, file.Length);
				testStream.Position = 0;
				using (ZipInputStream zis = new ZipInputStream(testStream))
				{
					ZipEntry entry;
					while ((entry = zis.GetNextEntry()) != null)
					{
						if (entry.IsFile)
						{
							string reg = @"\$facsimile_(\d+)\.jpg";
							Match match = Regex.Match(entry.Name, reg);

							valid = match.Success;
							if (!valid)
								break;
						}
					}
				}
			}
			return valid;
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
		#endregion
	}
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              