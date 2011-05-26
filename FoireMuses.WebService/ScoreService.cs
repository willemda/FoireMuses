using System.Collections.Generic;
using FoireMuses.Core;
using FoireMuses.Core.Interfaces;
using MindTouch.Dream;
using MindTouch.Tasking;
using MindTouch.Xml;

namespace FoireMuses.WebService
{
	using Yield = IEnumerator<IYield>;
	using System.IO;
	using FoireMuses.Core.Utils;
	using FoireMuses.Core.Querys;
	using System;

	public partial class Services
	{
		[DreamFeature("GET:scores", "Get all scores")]
		[DreamFeatureParam("max", "int?", "the number of document given by the output")]
		[DreamFeatureParam("offset", "int?", "skip the offset first results")]
		public Yield GetScores(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			theLogger.Info("GetScores");
			Result<SearchResult<IScoreSearchResult>> result = new Result<SearchResult<IScoreSearchResult>>();
			int limit = context.GetParam("max", 20);
			int offset = context.GetParam("offset", 0);

			yield return Context.Current.Instance.IndexController.GetAllScores(limit, offset, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.IndexController.ToJson(result.Value)));
		}

		[DreamFeature("GET:scores/source/{id}", "Get all scores from this source")]
		[DreamFeatureParam("max", "int?", "the number of document given by the output")]
		[DreamFeatureParam("offset", "int?", "skip the offset first results")]
		public Yield GetScoresFromSource(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			theLogger.Info("GetScoresFromSource");
			Result<SearchResult<IScore>> result = new Result<SearchResult<IScore>>();
			int limit = context.GetParam("max", 20);
			int offset = context.GetParam("offset", 0);
			string id = context.GetParam("id");

			yield return Context.Current.Instance.ScoreController.GetScoresFromSource(id, offset, limit, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.ScoreController.ToJson(result.Value)));
		}

		Yield GetContentLength(XUri uriToDownload, Result<int> result)
		{
			Plug p = Plug.New(uriToDownload);
			Result<DreamMessage> res;
			yield return res = p.GetAsync();

			// return the result
			result.Return((int)res.Value.ContentLength);
			yield return result;

			// copy the stream to disk in the background
			theLogger.Debug("BEFORE 5 SECONDDDSSS");
			yield return Async.Sleep(TimeSpan.FromSeconds(5));
			theLogger.Debug("AFTER 5 SECONDDDSSS");
		}

		[DreamFeature("GET:testasync", "")]
		public Yield TestAsynch(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			Result<int> result = new Result<int>(TimeSpan.FromSeconds(5));
			yield return Coroutine.Invoke(GetContentLength, new XUri("http://www.mindtouch.com"), result);
			if (result.HasException)
			{
				theLogger.Debug("got exception");
			}
			response.Return(DreamMessage.Ok(MimeType.TEXT, result.Value.ToString()));
		}

		[DreamFeature("GET:scores/{id}", "Get the score given by the id number")]
		[DreamFeature("GET:scores/{id}/{fileName}", "Get the score given by the id number")]
		public Yield GetScore(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			theLogger.Info("GetScore");
			string id = context.GetParam("id");
			string fileName = context.GetParam("fileName", ".json").ToLower();
			string fileType = Path.GetExtension(fileName);
			switch (fileType)
			{
				case ".json":
					Result<IScore> resultJson = new Result<IScore>();
					yield return Context.Current.Instance.ScoreController.Retrieve(id, resultJson);

					response.Return(resultJson.Value == null
								? DreamMessage.NotFound("No Score found for id " + id)
								: DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.ScoreController.ToJson(resultJson.Value)));
					break;
				case ".pdf":
					Result<Stream> resultPdf = new Result<Stream>();
					yield return Context.Current.Instance.ScoreController.GetConvertedScore(MimeType.PDF, id, resultPdf);
					Stream streamPdf = resultPdf.Value;
					response.Return(DreamMessage.Ok(MimeType.PDF, streamPdf.Length, streamPdf));
					break;
				case ".xml":
					Result<Stream> resultXml = new Result<Stream>();
					yield return Context.Current.Instance.ScoreController.GetAttachedFile(id, "$musicxml.xml", resultXml);
					Stream streamXml = resultXml.Value;
					response.Return(DreamMessage.Ok(MimeType.XML, streamXml.Length, streamXml));
					break;
				case ".mid":
					Result<Stream> resultMidi = new Result<Stream>();
					yield return Context.Current.Instance.ScoreController.GetConvertedScore(Constants.Midi, id, resultMidi);
					Stream streamMidi = resultMidi.Value;
					response.Return(DreamMessage.Ok(Constants.Midi, streamMidi.Length, streamMidi));
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
		public Yield SearchScore(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			theLogger.Info("SearchScore");
			ScoreQuery query = new ScoreQuery()
			{
				Composer = context.GetParam("composer", null),
				Editor = context.GetParam("editor", null),
				Title = context.GetParam("title", null),
				TitleWild = context.GetParam("titleWild", null),
				Verses = context.GetParam("verses", null),
				Music = context.GetParam("music", null),
				IsMaster = context.GetParam("isMaster", null),
				MasterId = context.GetParam("masterId", null),
				Offset = context.GetParam<int>("offset", 0),
				Max = context.GetParam<int>("max", 20)
			};
			Result<SearchResult<IScoreSearchResult>> result = new Result<SearchResult<IScoreSearchResult>>();
			yield return Context.Current.Instance.IndexController.SearchScore(query, result);
			response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.IndexController.ToJson(result.Value)));
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
			if (context.GetParam("id") == null || context.GetParam("rev") == null)
				response.Return(DreamMessage.BadRequest("not id or rev specified"));
			yield return Context.Current.Instance.ScoreController.Update(context.GetParam("id"), context.GetParam("rev"), score, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.ScoreController.ToJson(result.Value)));
		}

		[DreamFeature("POST:scores/musicxml", "Create a score with music xml")]
		public Yield CreateScoreWithMusicXml(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			IScore score = Context.Current.Instance.ScoreController.CreateNew();
			Result<IScore> result = new Result<IScore>();
			yield return Context.Current.Instance.ScoreController.AttachMusicXml(score, request.ToDocument(), false, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.ScoreController.ToJson(result.Value)));
		}

		[DreamFeature("GET:scores/{id}/attachments/{fileName}", "get the attached file name")]
		public Yield GetScoreAttachement(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			IScore score = Context.Current.Instance.ScoreController.CreateNew();
			Result<Stream> result = new Result<Stream>();
			yield return Context.Current.Instance.ScoreController.GetAttachedFile(context.GetParam("id"), context.GetParam("fileName"), result);
			Stream stream = result.Value;
			response.Return(DreamMessage.Ok(MimeType.BINARY, stream.Length, stream));
		}

		[DreamFeature("PUT:scores/musicxml", "Edit an existing score with music xml")]
		[DreamFeatureParam("{id}", "String", "Score id")]
		[DreamFeatureParam("{rev}", "String", "Score rev id")]
		[DreamFeatureParam("{overwrite}", "bool", "overwrite xml attributes or not")]
		public Yield UpdateScoreWithMusicXml(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			Result<IScore> resultRetrieve = new Result<IScore>();
			yield return Context.Current.Instance.ScoreController.Retrieve(context.GetParam("id"), resultRetrieve);
			IScore score = resultRetrieve.Value;
			Result<IScore> result = new Result<IScore>();
			yield return Context.Current.Instance.ScoreController.AttachMusicXml(score, request.ToDocument(), context.GetParam("overwrite", true), result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.ScoreController.ToJson(result.Value)));
		}


		[DreamFeature("DELETE:scores/{id}", "Delete a score")]
		[DreamFeatureParam("{id}", "String", "Score id")]
		[DreamFeatureParam("{rev}", "String", "Score revision id")]
		public Yield DeleteScore(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			Result<bool> result = new Result<bool>();
			yield return Context.Current.Instance.ScoreController.Delete(context.GetParam("id"), context.GetParam("rev"), result);

			response.Return(DreamMessage.Ok(MimeType.JSON, result.Value.ToString()));
		}

	}


}