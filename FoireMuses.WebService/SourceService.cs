using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Dream;
using MindTouch.Tasking;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core;

namespace FoireMuses.WebService
{
	using Yield = IEnumerator<IYield>;
	using MindTouch.Xml;
	using System.Collections;
	using System.IO;
	public partial class Services
	{
		#region Sources Methods
		[DreamFeature("GET:sources", "Get all sources")]
		[DreamFeatureParam("max", "int?", "the number of document given by the output")]
		[DreamFeatureParam("offset", "int?", "skip the offset first results")]
		public Yield GetSources(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			Result<SearchResult<ISourceSearchResult>> result = new Result<SearchResult<ISourceSearchResult>>();
			int limit = context.GetParam("max", 20);
			int offset = context.GetParam("offset", 0);

			yield return Context.Current.Instance.IndexController.GetAllSources(limit, offset, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.IndexController.ToJson(result.Value)));
		}

		[DreamFeature("GET:sources/{id}", "Get the source given by the id number")]
		[DreamFeatureParam("{id}", "String", "source id")]
		public Yield GetSource(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			Result<ISource> result = new Result<ISource>();
			string id = context.GetParam<string>("id");

			yield return Context.Current.Instance.SourceController.Retrieve(id, result);

			response.Return(result.Value == null
								? DreamMessage.NotFound("No Source found for id " + id)
								: DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.SourceController.ToJson(result.Value)));
		}

		[DreamFeature("POST:sources", "Create new source")]
		public Yield CreateSource(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			ISource source = Context.Current.Instance.SourceController.FromJson(request.ToText());
			Result<ISource> result = new Result<ISource>();
			yield return Context.Current.Instance.SourceController.Insert(source, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.SourceController.ToJson(result.Value)));
		}

		[DreamFeature("PUT:sources/{id}", "Update the source")]
		[DreamFeatureParam("{rev}", "String", "Source revision id, if not specified, use the last source rev")]
		public Yield UpdateSource(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			ISource source = Context.Current.Instance.SourceController.FromJson(request.ToText());
			Result<ISource> result = new Result<ISource>();
			yield return Context.Current.Instance.SourceController.Update(context.GetParam("id"), context.GetParam("rev", null), source, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.SourceController.ToJson(result.Value)));
		}

		[DreamFeature("DELETE:sources/{id}", "Delete a source and associated pages")]
		[DreamFeatureParam("{id}", "String", "source id")]
		[DreamFeatureParam("{rev}", "String", "source revision id")]
		public Yield DeleteSource(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			Result<bool> result = new Result<bool>();
			yield return Context.Current.Instance.SourceController.Delete(context.GetParam("id"), context.GetParam("rev",null), result);

			response.Return(DreamMessage.Ok(MimeType.JSON, result.Value.ToString()));
		} 
		#endregion

		#region Source Pages Methods

		[DreamFeature("GET:sources/{id}/pages", "Get all the source pages")]
		[DreamFeatureParam("max", "int", "the max result")]
		[DreamFeatureParam("offset", "int", "the result to start with")]
		public Yield GetPagesFromSource(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			Result<SearchResult<ISourcePage>> result;
			yield return result = Context.Current.Instance.SourcePageController.GetPagesFromSource(
				context.GetParam("id"),
				context.GetParam("offset", 0),
				context.GetParam("max", 10),
				new Result<SearchResult<ISourcePage>>());

			response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.SourcePageController.ToJson(result.Value)));
		}

		[DreamFeature("GET:sources/{id}/pages/{sourcePageId}", "Get source page")]
		public Yield GetSourcePage(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			Result<ISourcePage> result = new Result<ISourcePage>();
			yield return Context.Current.Instance.SourcePageController.Retrieve(context.GetParam("sourcePageId"), result);
			response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.SourcePageController.ToJson(result.Value)));
		}

		[DreamFeature("POST:sources/{sourceId}/pages/{sourcePageId}/fascimile", "Add an facsimile to the source")]
		public Yield AddFascimile(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			Stream file = request.ToStream();
			Result<bool> result;
			yield return result = Context.Current.Instance.SourcePageController.AddFascimile(context.GetParam("sourcePageId"), file, request.ContentLength, new Result<bool>());
			response.Return(DreamMessage.Ok());
		}

		[DreamFeature("PUT:sources/{id}/pages/{sourcePageId}", "Edit a source page")]
		[DreamFeatureParam("{sourcePageId}", "String", "Source page id")]
		[DreamFeatureParam("{sourcePageRev}", "String", "Source page revision id")]
		public Yield UpdateSourcePage(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			ISourcePage sourcePage = Context.Current.Instance.SourcePageController.FromJson(request.ToText());
			Result<ISourcePage> result = new Result<ISourcePage>();
			yield return Context.Current.Instance.SourcePageController.Update(context.GetParam("sourcePageId"), context.GetParam("sourcePageRev"), sourcePage, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.SourcePageController.ToJson(result.Value)));
		}

		[DreamFeature("POST:sources/{id}/pages", "Create a new page")]
		public Yield CreateSourcePage(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			if (request.ContentType.Match(MimeType.JSON))
			{
				ISourcePage page = Context.Current.Instance.SourcePageController.FromJson(request.ToText());
				Result<ISourcePage> result = new Result<ISourcePage>();
				yield return Context.Current.Instance.SourcePageController.Insert(page, result);
				response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.SourcePageController.ToJson(result.Value)));
			}
			else // Bulk import based on ZIP file
			{
				Stream file = request.ToStream();
				Result<bool> result = new Result<bool>();
				yield return Context.Current.Instance.SourcePageController.BulkImportSourcePages(context.GetParam("sourceId"), file, result);

				if (result.Value)
					response.Return(DreamMessage.Ok());
				else
					response.Return(DreamMessage.BadRequest("Bad name format"));

			}
		}

		[DreamFeature("DELETE:sources/{Id}/pages/{sourcePageId}", "Delete a source page")]
		[DreamFeatureParam("{id}", "String", "source id")]
		[DreamFeatureParam("{sourcePageId}", "String", "sourcepage id")]
		[DreamFeatureParam("{rev}", "String", "source page revision id")]
		public Yield DeleteSourcePage(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			Result<bool> result = new Result<bool>();
			yield return Context.Current.Instance.SourcePageController.Delete(context.GetParam("id"), context.GetParam("rev"), result);

			response.Return(DreamMessage.Ok(MimeType.JSON, result.Value.ToString()));
		} 
		#endregion
	}
}
