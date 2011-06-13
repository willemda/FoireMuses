#region

using System.Collections.Generic;
using System.IO;
using FoireMuses.Core;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core.Querys;
using FoireMuses.Core.Utils;
using MindTouch.Dream;
using MindTouch.Tasking;

#endregion

namespace FoireMuses.WebService
{
	#region

	using Yield = IEnumerator<IYield>;

	#endregion

	public partial class Services
	{
		[DreamFeature("GET:scores", "Get all scores")]
		[DreamFeatureParam("max", "int?", "the number of document given by the output")]
		[DreamFeatureParam("offset", "int?", "skip the offset first results")]
		public Yield GetScores(DreamContext aContext, DreamMessage aRequest, Result<DreamMessage> aResponse)
		{
			theLogger.Info("GetScores");
			Result<SearchResult<IScoreSearchResult>> result = new Result<SearchResult<IScoreSearchResult>>();
			int limit = aContext.GetParam("max", 20);
			int offset = aContext.GetParam("offset", 0);

			yield return Context.Current.Instance.IndexController.GetAllScores(limit, offset, result);

			aResponse.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.IndexController.ToJson(result.Value)));
		}

		[DreamFeature("GET:scores/source/{id}", "Get all scores from this source")]
		[DreamFeatureParam("max", "int?", "the number of document given by the output")]
		[DreamFeatureParam("offset", "int?", "skip the offset first results")]
		public Yield GetScoresFromSource(DreamContext aContext, DreamMessage aRequest, Result<DreamMessage> aResponse)
		{
			theLogger.Info("GetScoresFromSource");
			Result<SearchResult<IScore>> result = new Result<SearchResult<IScore>>();
			int limit = aContext.GetParam("max", 20);
			int offset = aContext.GetParam("offset", 0);
			string id = aContext.GetParam("id");

			yield return Context.Current.Instance.ScoreController.GetScoresFromSource(id, offset, limit, result);

			aResponse.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.ScoreController.ToJson(result.Value)));
		}

		[DreamFeature("GET:scores/{id}", "Get the score given by the id number")]
		[DreamFeature("GET:scores/{id}/{fileName}", "Get the score given by the id number")]
		public Yield GetScore(DreamContext aContext, DreamMessage aRequest, Result<DreamMessage> aResponse)
		{
			theLogger.Info("GetScore");
			string id = aContext.GetParam("id");
			string fileName = aContext.GetParam("fileName", ".json").ToLower();
			string fileType = Path.GetExtension(fileName);
			switch (fileType)
			{
				case ".json":
					Result<IScore> resultJson = new Result<IScore>();
					yield return Context.Current.Instance.ScoreController.Retrieve(id, resultJson);

					aResponse.Return(resultJson.Value == null
					                 	? DreamMessage.NotFound("No Score found for id " + id)
					                 	: DreamMessage.Ok(MimeType.JSON,
					                 	                  Context.Current.Instance.ScoreController.ToJson(resultJson.Value)));
					break;
				case ".pdf":
					Result<Stream> resultPdf = new Result<Stream>();
					yield return Context.Current.Instance.ScoreController.GetConvertedScore(MimeType.PDF, id, resultPdf);
					Stream streamPdf = resultPdf.Value;
					aResponse.Return(DreamMessage.Ok(MimeType.PDF, streamPdf.Length, streamPdf));
					break;
				case ".xml":
					Result<Stream> resultXml = new Result<Stream>();
					yield return Context.Current.Instance.ScoreController.GetAttachment(id, "$musicxml.xml", resultXml);
					Stream streamXml = resultXml.Value;
					aResponse.Return(DreamMessage.Ok(MimeType.XML, streamXml.Length, streamXml));
					break;
				case ".mid":
					Result<Stream> resultMidi = new Result<Stream>();
					yield return Context.Current.Instance.ScoreController.GetConvertedScore(Constants.Midi, id, resultMidi);
					Stream streamMidi = resultMidi.Value;
					aResponse.Return(DreamMessage.Ok(Constants.Midi, streamMidi.Length, streamMidi));
					break;
			}
			yield break;
		}

		[DreamFeature("GET:scores/search", "Search for a  score")]
		[DreamFeatureParam("music", "string", "music partition to search for")]
		[DreamFeatureParam("title", "string", "the title")]
		[DreamFeatureParam("titleWild", "string", "the title wildcarded")]
		[DreamFeatureParam("composer", "string", "the composer")]
		[DreamFeatureParam("editor", "string", "the editor")]
		[DreamFeatureParam("verses", "string", "verses in the score to search for")]
		[DreamFeatureParam("isMaster", "bool?", "is master or not")]
		[DreamFeatureParam("masterId", "string", "the master's id")]
		[DreamFeatureParam("offset", "int?", "result to start with")]
		[DreamFeatureParam("max", "int?", "max results")]
		public Yield SearchScore(DreamContext aContext, DreamMessage aRequest, Result<DreamMessage> aResponse)
		{
			theLogger.Info("SearchScore");
			ScoreQuery query = new ScoreQuery
			                   	{
			                   		Composer = aContext.GetParam("composer", null),
			                   		Editor = aContext.GetParam("editor", null),
			                   		Title = aContext.GetParam("title", null),
			                   		TitleWild = aContext.GetParam("titleWild", null),
			                   		Verses = aContext.GetParam("verses", null),
			                   		Music = aContext.GetParam("music", null),
			                   		IsMaster = aContext.GetParam("isMaster", null),
			                   		MasterId = aContext.GetParam("masterId", null),
			                   		Offset = aContext.GetParam("offset", 0),
			                   		Max = aContext.GetParam("max", 20)
			                   	};
			Result<SearchResult<IScoreSearchResult>> result = new Result<SearchResult<IScoreSearchResult>>();
			yield return Context.Current.Instance.IndexController.SearchScore(query, result);
			aResponse.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.IndexController.ToJson(result.Value)));
		}

		[DreamFeature("POST:scores", "Insert new score")]
		public Yield CreateScore(DreamContext aContext, DreamMessage aRequest, Result<DreamMessage> aResponse)
		{
			IScore score = Context.Current.Instance.ScoreController.FromJson(aRequest.ToText());
			Result<IScore> result = new Result<IScore>();
			yield return Context.Current.Instance.ScoreController.Insert(score, result);

			aResponse.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.ScoreController.ToJson(result.Value)));
		}

		[DreamFeature("PUT:scores", "Update the score")]
		[DreamFeatureParam("{id}", "String", "Score id")]
		[DreamFeatureParam("{rev}", "String", "Score revision id")]
		public Yield UpdateScore(DreamContext aContext, DreamMessage aRequest, Result<DreamMessage> aResponse)
		{
			IScore score = Context.Current.Instance.ScoreController.FromJson(aRequest.ToText());
			Result<IScore> result = new Result<IScore>();
			if (aContext.GetParam("id") == null || aContext.GetParam("rev") == null)
				aResponse.Return(DreamMessage.BadRequest("not id or rev specified"));
			yield return
				Context.Current.Instance.ScoreController.Update(aContext.GetParam("id"), aContext.GetParam("rev"), score, result);

			aResponse.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.ScoreController.ToJson(result.Value)));
		}

		[DreamFeature("POST:scores/musicxml", "Insert a score with music xml")]
		public Yield CreateScoreWithMusicXml(DreamContext aContext, DreamMessage aRequest, Result<DreamMessage> aResponse)
		{
			IScore score = Context.Current.Instance.ScoreController.CreateNew();
			Result<IScore> result = new Result<IScore>();
			yield return Context.Current.Instance.ScoreController.AttachMusicXml(score, aRequest.ToDocument(), false, result);

			aResponse.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.ScoreController.ToJson(result.Value)));
		}

		[DreamFeature("GET:scores/{id}/attachments/{fileName}", "get the attached file name")]
		public Yield GetScoreAttachement(DreamContext aContext, DreamMessage aRequest, Result<DreamMessage> aResponse)
		{
			IScore score = Context.Current.Instance.ScoreController.CreateNew();
			Result<Stream> result = new Result<Stream>();
			yield return
				Context.Current.Instance.ScoreController.GetAttachment(aContext.GetParam("id"), aContext.GetParam("fileName"),
				                                                       result)
				;
			Stream stream = result.Value;
			aResponse.Return(DreamMessage.Ok(MimeType.BINARY, stream.Length, stream));
		}

		[DreamFeature("PUT:scores/musicxml", "Edit an existing score with music xml")]
		[DreamFeatureParam("{id}", "String", "Score id")]
		[DreamFeatureParam("{rev}", "String", "Score rev id")]
		[DreamFeatureParam("{overwrite}", "bool", "overwrite xml attributes or not")]
		public Yield UpdateScoreWithMusicXml(DreamContext aContext, DreamMessage aRequest, Result<DreamMessage> aResponse)
		{
			Result<IScore> resultRetrieve = new Result<IScore>();
			yield return Context.Current.Instance.ScoreController.Retrieve(aContext.GetParam("id"), resultRetrieve);
			IScore score = resultRetrieve.Value;
			Result<IScore> result = new Result<IScore>();
			yield return
				Context.Current.Instance.ScoreController.AttachMusicXml(score, aRequest.ToDocument(),
				                                                        aContext.GetParam("overwrite", true), result);

			aResponse.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.ScoreController.ToJson(result.Value)));
		}

		[DreamFeature("DELETE:scores/{id}", "Delete a score")]
		[DreamFeatureParam("{id}", "String", "Score id")]
		[DreamFeatureParam("{rev}", "String", "Score revision id")]
		public Yield DeleteScore(DreamContext aContext, DreamMessage aRequest, Result<DreamMessage> aResponse)
		{
			Result<bool> result = new Result<bool>();
			yield return
				Context.Current.Instance.ScoreController.Delete(aContext.GetParam("id"), aContext.GetParam("rev"), result)
				;

			aResponse.Return(DreamMessage.Ok(MimeType.JSON, result.Value.ToString()));
		}
	}
}