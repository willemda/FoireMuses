using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using LoveSeat;
using MindTouch.Tasking;
using MindTouch.Dream;
using FoireMuses.Core.Controllers;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core;
using MindTouch.Xml;
using Newtonsoft.Json.Linq;

namespace FoireMuses.WebService
{

	using Yield = System.Collections.Generic.IEnumerator<IYield>;

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

			yield return Context.Current.Instance.ScoreController.Get(id, result);

			response.Return(result.Value == null
								? DreamMessage.NotFound("No Score found for id " + id)
								: DreamMessage.Ok(MimeType.JSON, Factory.ResultToJson(result.Value)));
		}

		[DreamFeature("POST:scores", "Create new score")]
		public Yield CreateScore(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			IScore score = Factory.IScoreFromJson(request.ToText());
			Result<IScore> result = new Result<IScore>();
			yield return Context.Current.Instance.ScoreController.Create(score, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Factory.ResultToJson(result.Value)));
		}

		[DreamFeature("PUT:scores", "Update the score")]
		public Yield UpdateScore(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			IScore score = Factory.IScoreFromJson(request.ToText());
			Result<IScore> result = new Result<IScore>();
			yield return Context.Current.Instance.ScoreController.Update(score, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Factory.ResultToJson(result.Value)));
		}


		// TODO: delete methods not allowed!
		public Yield DeleteScore(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			IScore score= Factory.IScoreFromJson(request.ToText());
			Result<bool> result = new Result<bool>();
			yield return Context.Current.Instance.ScoreController.Delete(score, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, result.Value.ToString()));
		}

	}


}
