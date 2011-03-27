using System.Collections.Generic;
using FoireMuses.Core;
using FoireMuses.Core.Interfaces;
using MindTouch.Dream;
using MindTouch.Tasking;

namespace FoireMuses.WebService
{
	using Yield = IEnumerator<IYield>;

	public partial class Services
	{
		[DreamFeature("GET:scores", "Get all scores")]
		[DreamFeatureParam("limit", "int?", "the number of document given by the output")]
		[DreamFeatureParam("offset", "int?", "skip the offset first results")]
		public Yield GetScores(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			Result<SearchResult<IScore>> result = new Result<SearchResult<IScore>>();
			int limit = context.GetParam("limit", 100);
			int offset = context.GetParam("offset", 0);

			yield return Context.Current.Instance.ScoreController.GetAll(offset,limit, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Factory.ResultToJson(result.Value)));
		}

		[DreamFeature("GET:scores/{id}", "Get the score given by the id number")]
		public Yield GetScoreById(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			Result<IScore> result = new Result<IScore>();
			string id = context.GetParam<string>("id");

			yield return Context.Current.Instance.ScoreController.Retrieve(id, result);

			response.Return(result.Value == null
								? DreamMessage.NotFound("No Score found for id " + id)
								: DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.ScoreController.ToJson(result.Value)));
		}

		[DreamFeature("POST:scores", "Create new score")]
		public Yield CreateScore(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			IScore score = Context.Current.Instance.ScoreController.FromJson(request.ToText());
			Result<IScore> result = new Result<IScore>();
			yield return Context.Current.Instance.ScoreController.Create(score, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.ScoreController.ToJson(result.Value)));
		}

		[DreamFeature("PUT:scores/{id}", "Update the score")]
		[DreamFeatureParam("{id}", "String", "Score id")]
		[DreamFeatureParam("{rev}", "String", "Score revision id")]
		public Yield UpdateScore(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			IScore score = Context.Current.Instance.ScoreController.FromJson(request.ToText());
			Result<IScore> result = new Result<IScore>();
			yield return Context.Current.Instance.ScoreController.Update("","",score, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.ScoreController.ToJson(result.Value)));
		}


		[DreamFeature("DELETE:scores/{id}", "Delete a score")]
		[DreamFeatureParam("{id}", "String", "Score id")]
		public Yield DeleteScore(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			Result<bool> result = new Result<bool>();
			yield return Context.Current.Instance.ScoreController.Delete(context.GetParam("id"), result);

			response.Return(DreamMessage.Ok(MimeType.JSON, result.Value.ToString()));
		}

	}


}
