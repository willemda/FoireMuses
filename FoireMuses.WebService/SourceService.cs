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
	public partial class Services
	{

		[DreamFeature("GET:sources", "Get all sources")]
		[DreamFeatureParam("max", "int?", "the number of document given by the output")]
		[DreamFeatureParam("offset", "int?", "skip the offset first results")]
		public Yield GetSources(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			Result<SearchResult<ISource>> result = new Result<SearchResult<ISource>>();
			int limit = context.GetParam("max", 20);
			int offset = context.GetParam("offset", 0);

			yield return Context.Current.Instance.SourceController.GetAll(offset, limit, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Factory.ResultToJson(result.Value)));
		}

		[DreamFeature("GET:sources/{id}", "Get the source given by the id number")]
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
			ISource source;
			if (request.ContentType == MimeType.XML)
				source = Context.Current.Instance.SourceController.FromXml(XDocFactory.From(request.ToStream(), MimeType.XML));
			else
				source = Context.Current.Instance.SourceController.FromJson(request.ToText());
			Result<ISource> result = new Result<ISource>();
			yield return Context.Current.Instance.SourceController.Create(source, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.SourceController.ToJson(result.Value)));
		}

		[DreamFeature("PUT:sources/{id}", "Update the source")]
		[DreamFeatureParam("{id}", "String", "Source id")]
		[DreamFeatureParam("{rev}", "String", "Source revision id")]
		public Yield UpdateSource(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			ISource source = Context.Current.Instance.SourceController.FromJson(request.ToText());
			Result<ISource> result = new Result<ISource>();
			yield return Context.Current.Instance.SourceController.Update(context.GetParam("id"), context.GetParam("rev"), source, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.SourceController.ToJson(result.Value)));
		}


		[DreamFeature("DELETE:sources/{id}", "Delete a source")]
		[DreamFeatureParam("{id}", "String", "source id")]
		[DreamFeatureParam("{rev}", "String", "source revision id")]
		public Yield DeleteSource(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			Result<bool> result = new Result<bool>();
			yield return Context.Current.Instance.SourceController.Delete(context.GetParam("id"), context.GetParam("rev"), result);

			response.Return(DreamMessage.Ok(MimeType.JSON, result.Value.ToString()));
		}
	}
}
