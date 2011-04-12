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
		[DreamFeature("GET:scores", "Get all scores")]
		[DreamFeatureParam("max", "int?", "the number of document given by the output")]
		[DreamFeatureParam("offset", "int?", "skip the offset first results")]
		public Yield GetScores(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			Result<SearchResult<IScore>> result = new Result<SearchResult<IScore>>();
			int limit = context.GetParam("max", 20);
			int offset = context.GetParam("offset", 0);

			yield return Context.Current.Instance.ScoreController.GetAll(offset,limit, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.ScoreController.ToJson(result.Value)));
		}

		[DreamFeature("GET:scores/source/{id}", "Get all scores from this source")]
		[DreamFeatureParam("max", "int?", "the number of document given by the output")]
		[DreamFeatureParam("offset", "int?", "skip the offset first results")]
		public Yield GetScoresFromSource(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			Result<SearchResult<IScore>> result = new Result<SearchResult<IScore>>();
			int limit = context.GetParam("max", 20);
			int offset = context.GetParam("offset", 0);
			string id = context.GetParam("id");

			yield return Context.Current.Instance.ScoreController.GetScoresFromSource(offset, limit, id, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.ScoreController.ToJson(result.Value)));
		}

		[DreamFeature("GET:scores/{id}", "Get the score given by the id number")]
		public Yield GetScore(DreamContext context, DreamMessage request, Result<DreamMessage> response)
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
			IScore score;
			if (request.ContentType == MimeType.XML)
				score = Context.Current.Instance.ScoreController.FromXml(XDocFactory.From(request.ToStream(), MimeType.XML));
			else
				score = Context.Current.Instance.ScoreController.FromJson(request.ToText());
			Result<IScore> result = new Result<IScore>();
			yield return Context.Current.Instance.ScoreController.Create(score, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.ScoreController.ToJson(result.Value)));
		}

		[DreamFeature("PUT:scores", "Update the score")]
		[DreamFeatureParam("{id}", "String", "Score id")]
		[DreamFeatureParam("{rev}", "String", "Score revision id")]
		public Yield UpdateScore(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			IScore score = Context.Current.Instance.ScoreController.FromJson(request.ToText());
			Result<IScore> result = new Result<IScore>();
            if(context.GetParam("id") == null || context.GetParam("rev") == null)
                response.Return(DreamMessage.BadRequest("not id or rev specified"));
			yield return Context.Current.Instance.ScoreController.Update(context.GetParam("id"), context.GetParam("rev"), score, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.ScoreController.ToJson(result.Value)));
		}

		[DreamFeature("POST:scores/xml", "Create a score with music xml")]
		public Yield CreateScoreWithMusicXml(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			IScore score = Context.Current.Instance.ScoreController.CreateNew();
			Result<IScore> result = new Result<IScore>();
			yield return Context.Current.Instance.ScoreController.AttachMusicXml(score, request.ToDocument(), false, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.ScoreController.ToJson(result.Value)));
		}

        [DreamFeature("PUT:scores/xml", "Edit an existing score with music xml")]
        [DreamFeatureParam("{id}", "String", "Score id")]
        [DreamFeatureParam("{rev}", "String", "Score rev id")]
        [DreamFeatureParam("{overwrite}", "bool", "overwrite xml attributes or not")]
        public Yield UpdateScoreWithMusicXml(DreamContext context, DreamMessage request, Result<DreamMessage> response)
        {
            Result<IScore> resultRetrieve = new Result<IScore>();
            yield return Context.Current.Instance.ScoreController.Retrieve(context.GetParam("id"), resultRetrieve);
            IScore score = resultRetrieve.Value;
            Result<IScore> result = new Result<IScore>();
            yield return Context.Current.Instance.ScoreController.AttachMusicXml(score, request.ToDocument(), context.GetParam("overwrite",true), result);

            response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.ScoreController.ToJson(result.Value)));
        }


		[DreamFeature("DELETE:scores/{id}", "Delete a score")]
		[DreamFeatureParam("{id}", "String", "Score id")]
		[DreamFeatureParam("{rev}", "String", "Score revision id")]
		public Yield DeleteScore(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			Result<bool> result = new Result<bool>();
			yield return Context.Current.Instance.ScoreController.Delete(context.GetParam("id"),context.GetParam("rev"), result);

			response.Return(DreamMessage.Ok(MimeType.JSON, result.Value.ToString()));
		}

	}


}