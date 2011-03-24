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


namespace FoireMuses.WebService
{

	using Yield = System.Collections.Generic.IEnumerator<IYield>;
	using FoireMuses.Core;
	using MindTouch.Xml;
	using Newtonsoft.Json.Linq;
	using FoireMuses.Core.Business;

	public partial class Services
	{
		[DreamFeature("GET:scores", "Get all scores")]
		[DreamFeatureParam("limit", "int?", "the number of document given by the output")]
		public Yield GetScores(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			Result<SearchResult<IScore>> result = new Result<SearchResult<IScore>>();
			int limit = context.GetParam("limit", 100);

			yield return Context.Current.Instance.ScoreController.GetAll(0,limit, result);

			string json = ResultToJson(result.Value);
			response.Return(DreamMessage.Ok(MimeType.JSON, json));
		}

		[DreamFeature("GET:scores/{id}", "Get the score given by the id number")]
		public Yield GetScoreById(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			Result<IScore> result = new Result<IScore>();
			string id = context.GetParam<string>("id");

			yield return Context.Current.Instance.ScoreController.Get(id, result);

			response.Return(result.Value == null
								? DreamMessage.NotFound("No Score found for id " + id)
								: DreamMessage.Ok(MimeType.JSON, result.Value.ToString()));
		}

		[DreamFeature("POST:scores", "Create new score")]
		public Yield CreateScore(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			IScore score = Factory.IScoreFromJson(request.ToText());
			Result<IScore> result = new Result<IScore>();
			yield return Context.Current.Instance.ScoreController.Create(score, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, result.Value.ToString()));
		}

		[DreamFeature("PUT:scores", "Update the score")]
		public Yield UpdateScore(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			IScore score = Factory.IScoreFromJson(request.ToText());
			Result<IScore> result = new Result<IScore>();
			yield return Context.Current.Instance.ScoreController.Update(score, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, result.Value.ToString()));
		}


		// TODO: delete methods not allowed!
		public Yield DeleteScore(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			IScore score= Factory.IScoreFromJson(request.ToText());
			Result<bool> result = new Result<bool>();
			yield return Context.Current.Instance.ScoreController.Delete(score, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, result.Value.ToString()));
		}

		private XDoc ResultToXml(ViewResult<string, string> result)
		{
			XDoc xdoc = new XDoc("scores");
			foreach (ViewResultRow<string, string> row in result.Rows)
			{
				xdoc.Start("score");
				xdoc.Attr("id", row.Key);
				xdoc.Attr("title", row.Value);
				xdoc.End();
			}
			return xdoc;
		}

		private string ResultToJson(ViewResult<string, string> result)
		{
			string json = "";
			foreach (ViewResultRow<string, string> r in result.Rows)
			{
				json += "Id: " + r.Id + "\tKey: " + r.Key + "\n";
			}
			return json;
		}


		private string ResultToJson(SearchResult<IScore> result)
		{
			string json = "";
			foreach (IScore score in result)
			{
				json += (score as JScore).ToString()+"\n";
			}
			return json;
		}

	}


}
