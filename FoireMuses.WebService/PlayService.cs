using System.Collections.Generic;
using FoireMuses.Core;
using FoireMuses.Core.Interfaces;
using MindTouch.Dream;
using MindTouch.Tasking;
using MindTouch.Xml;

namespace FoireMuses.WebService
{
	using Yield = IEnumerator<IYield>;

	public partial class Services
	{
		[DreamFeature("GET:plays", "All plays")]
		[DreamFeatureParam("max", "int?", "maximum rows")]
		[DreamFeatureParam("offset", "int?", "offset to start at")]
		public Yield GetPlays(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			Result<SearchResult<IPlay>> result = new Result<SearchResult<IPlay>>();
			int limit = context.GetParam("max", 20);
			int offset = context.GetParam("offset", 0);

			yield return Context.Current.Instance.PlayController.GetAll(offset, limit, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.PlayController.ToJson(result.Value)));
		}

		[DreamFeature("GET:plays/source/{id}", "All plays that are from this source")]
		[DreamFeatureParam("max", "int?", "maximum rows")]
		[DreamFeatureParam("offset", "int?", "offset to start at")]
		public Yield GetPlaysFromSource(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			Result<SearchResult<IPlay>> result = new Result<SearchResult<IPlay>>();
			int limit = context.GetParam("max", 20);
			int offset = context.GetParam("offset", 0);
			string id = context.GetParam("id");

			yield return Context.Current.Instance.PlayController.GetPlaysFromSource(offset, limit, id, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.PlayController.ToJson(result.Value)));
		}

		[DreamFeature("GET:plays/{id}", "The play that have this id")]
		public Yield GetPlay(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			Result<IPlay> result = new Result<IPlay>();
			string id = context.GetParam<string>("id");

			yield return Context.Current.Instance.PlayController.Retrieve(id, result);

			response.Return(result.Value == null
								? DreamMessage.NotFound("No Score found for id " + id)
								: DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.PlayController.ToJson(result.Value)));
		}

		[DreamFeature("POST:plays", "Insert a play")]
		public Yield CreatePlay(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			IPlay play  = Context.Current.Instance.PlayController.FromJson(request.ToText());
			Result<IPlay> result = new Result<IPlay>();
			yield return Context.Current.Instance.PlayController.Insert(play, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.PlayController.ToJson(result.Value)));
		}

		[DreamFeature("PUT:plays/{id}", "Update a play")]
		[DreamFeatureParam("{id}", "String", "Play id")]
		[DreamFeatureParam("{rev}", "String", "Play revision id")]
		public Yield UpdatePlay(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			string playId = context.GetParam("id");
			string playRev = context.GetParam("rev");

			IPlay play = Context.Current.Instance.PlayController.FromJson(request.ToText());

			Result<IPlay> result = new Result<IPlay>();

			yield return Context.Current.Instance.PlayController.Update(playId,playRev, play, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.PlayController.ToJson(result.Value)));
		}

		[DreamFeature("DELETE:plays/{id}", "Delete a play")]
		[DreamFeatureParam("{id}", "String", "play id")]
		[DreamFeatureParam("{rev}", "String", "play revision id")]
		public Yield DeletePlay(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			Result<bool> result = new Result<bool>();
			yield return Context.Current.Instance.ScoreController.Delete(context.GetParam("id"), context.GetParam("rev"), result);

			response.Return(DreamMessage.Ok(MimeType.JSON, result.Value.ToString()));
		}
	}
}