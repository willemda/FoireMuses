using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Utils;
using Lucene.Net.Store;
using Lucene.Net.Index;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis;
using Lucene.Net.Search;
using MindTouch.Tasking;
using Lucene.Net.Documents;
using FoireMuses.Core.Interfaces;
using Lucene.Net.QueryParsers;
using FoireMuses.Core.Querys;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace FoireMuses.Core.Controllers
{
	public class IndexController : IIndexController
	{
		private static readonly log4net.ILog theLogger = log4net.LogManager.GetLogger(typeof(IndexController));
		private static readonly string[] thePitches = new[] { "c", "ces", "cis", "d", "des", "dis", "e", "f", "fes", "fis", "g", "ges", "gis", "a", "aes", "ais", "b" };
		private static readonly int[] thePitchesValue = new[] { 0, 1, 1, 2, 3, 3, 4, 5, 6, 6, 7, 8, 8, 9, 10, 10, 11 };

		private readonly Directory theDirectory;
		private readonly IndexWriter theWriter;
		private readonly PerFieldAnalyzerWrapper thePerFieldAnalyzer;
		private readonly INotificationManager theNotificationManager;
		private readonly IScoreDataMapper theScoreDataMapper;

		public Instance Instance { get; set; }

		public IndexController(INotificationManager aNotificationManager, IScoreDataMapper aScoreDataMapper)
		{
			theLogger.Info("Creation of the IndexController");
			theNotificationManager = aNotificationManager;
			theScoreDataMapper = aScoreDataMapper;

			theDirectory = FSDirectory.Open(new System.IO.DirectoryInfo(System.IO.Path.Combine(Environment.CurrentDirectory,"LuceneIndex")));
			Analyzer whiteSpaceAnalyzer = new WhitespaceAnalyzer();
			Analyzer standardAnalyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);
			Analyzer keywordAnalyzer = new KeywordAnalyzer();
			thePerFieldAnalyzer = new PerFieldAnalyzerWrapper(standardAnalyzer);
			thePerFieldAnalyzer.AddAnalyzer("codageRythmique", whiteSpaceAnalyzer);
			thePerFieldAnalyzer.AddAnalyzer("codageMelodiqueRISM", whiteSpaceAnalyzer);
			thePerFieldAnalyzer.AddAnalyzer("codageParIntervalles", whiteSpaceAnalyzer);
			thePerFieldAnalyzer.AddAnalyzer("otype", keywordAnalyzer);
			thePerFieldAnalyzer.AddAnalyzer("sourceId", keywordAnalyzer);
			thePerFieldAnalyzer.AddAnalyzer("id", keywordAnalyzer);
			thePerFieldAnalyzer.AddAnalyzer("rev", keywordAnalyzer);
			theWriter = new IndexWriter(theDirectory, thePerFieldAnalyzer, !IndexReader.IndexExists(theDirectory), IndexWriter.MaxFieldLength.LIMITED);
			theNotificationManager.ScoreChanged += theNotificationManager_ScoreChanged;
			theNotificationManager.PlayChanged += theNotificationManager_PlayChanged;
			theNotificationManager.SourceChanged += theNotificationManager_SourceChanged;
			theNotificationManager.SourcePageChanged += theNotificationManager_SourcePageChanged;
			theNotificationManager.Start();
			theLogger.Info("IndexController created");
		}

		private void theNotificationManager_SourcePageChanged(object sender, EventArgs<ISourcePage> e)
		{
			theLogger.Info("Source Page Changed - updating index");
			UpdateSourcePage(e.Item, new Result()).Wait();
		}

		private void theNotificationManager_SourceChanged(object sender, EventArgs<ISource> e)
		{
			theLogger.Info("Source Changed - updating index");
			UpdateSource(e.Item, new Result()).Wait();
		}

		private void theNotificationManager_PlayChanged(object sender, EventArgs<IPlay> e)
		{
			theLogger.Info("Play Changed - updating index");
			//throw new NotImplementedException();
		}

		private void theNotificationManager_ScoreChanged(object sender, EventArgs<IScore> e)
		{
			theLogger.Info("Score Changed - updating index");
			UpdateScore(e.Item, new Result()).Wait();
		}

		public Result AddScore(IScore score, Result aResult)
		{
			Document d = new Document();
			d.AddCheck("otype", "score", Field.Store.NO, Field.Index.NOT_ANALYZED);
			d.AddCheck("id", score.Id, Field.Store.YES, Field.Index.NOT_ANALYZED);
			d.AddCheck("rev", score.Rev, Field.Store.YES, Field.Index.NOT_ANALYZED);
			d.AddCheck("isMaster", score.IsMaster, Field.Store.NO, Field.Index.ANALYZED);
			d.AddCheck("masterId", score.MasterId, Field.Store.NO, Field.Index.NOT_ANALYZED);
			d.AddCheck("codageMelodiqueRISM", score.CodageMelodiqueRISM, Field.Store.NO, Field.Index.ANALYZED);
			d.AddCheck("codageParIntervalles", score.CodageParIntervalles, Field.Store.NO, Field.Index.ANALYZED);
			d.AddCheck("codageRythmique", score.CodageRythmique, Field.Store.NO, Field.Index.ANALYZED);
			d.AddCheck("codageMelodiqueRISM", score.Code1, Field.Store.NO, Field.Index.ANALYZED);
			d.AddCheck("codageParIntervalles", score.Code2, Field.Store.NO, Field.Index.ANALYZED);
			d.AddCheck("composer", score.Composer, Field.Store.YES, Field.Index.ANALYZED);
			d.AddCheck("editor", score.Editor, Field.Store.YES, Field.Index.ANALYZED);
			d.AddCheck("comments", score.Comments, Field.Store.NO, Field.Index.ANALYZED);
			d.AddCheck("creatorId", score.CreatorId, Field.Store.YES, Field.Index.NOT_ANALYZED);
			d.AddCheck("type", score.ScoreType, Field.Store.YES, Field.Index.ANALYZED);
			d.AddCheck("verses", score.Verses, Field.Store.YES, Field.Index.ANALYZED);
			d.AddCheck("title", score.Title, Field.Store.YES, Field.Index.ANALYZED);
			if (score.Title != null)
			{
				string[] titleParts = score.Title.Split(new char[] { ' ' });
				string titleWithoutSpaces = titleParts.Aggregate("", (current, part) => current + part);
				d.AddCheck("titleWithoutSpaces", titleWithoutSpaces, Field.Store.YES, Field.Index.ANALYZED);
			}
			d.AddCheck("strophe", score.Stanza, Field.Store.NO, Field.Index.ANALYZED);
			d.AddCheck("coirault", score.Coirault, Field.Store.NO, Field.Index.ANALYZED);
			d.AddCheck("delarue", score.Delarue, Field.Store.NO, Field.Index.ANALYZED);

			d.AddCheck("musicalSourceReference", GetMusicalSourceText(score),Field.Store.YES,Field.Index.NO);
			d.AddCheck("textualSourceReference", GetTextualSourceText(score), Field.Store.YES, Field.Index.NO);

			string tags = "";
			if (score.Tags != null)
			{
				tags = score.Tags.Aggregate(tags, (current, tag) => current + (tag + " "));
				d.AddCheck("tags", tags, Field.Store.YES, Field.Index.ANALYZED);
			}
			lock (theWriter)
			{
				theWriter.AddDocument(d, thePerFieldAnalyzer);
				theWriter.Commit();
			}
			aResult.Return();
			return aResult;
		}

		public Result DeleteScore(string id, Result aResult)
		{
			QueryParser queryParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "id", thePerFieldAnalyzer);
			Query query = queryParser.Parse(id);
			theWriter.DeleteDocuments(query);
			aResult.Return();
			return aResult;
		}

		public Result UpdateScore(IScore score, Result aResult)
		{
			DeleteScore(score.Id, new Result());
			AddScore(score, new Result());
			aResult.Return();
			return aResult;
		}

		public Result AddSource(ISource source, Result aResult)
		{
			Document d = new Document();
			d.AddCheck("otype", "source", Field.Store.NO, Field.Index.NOT_ANALYZED);
			d.AddCheck("id", source.Id, Field.Store.YES, Field.Index.NOT_ANALYZED);
			d.AddCheck("rev", source.Rev, Field.Store.YES, Field.Index.NOT_ANALYZED);
			d.AddCheck("name", source.Name, Field.Store.YES, Field.Index.ANALYZED);
			d.AddCheck("publisher", source.Publisher, Field.Store.YES, Field.Index.ANALYZED);
			d.AddCheck("dateFrom", source.DateFrom, Field.Store.YES, Field.Index.ANALYZED);
			d.AddCheck("dateTo", source.DateTo, Field.Store.YES, Field.Index.ANALYZED);
			d.AddCheck("abbreviation", source.Abbreviation, Field.Store.NO, Field.Index.ANALYZED);
			d.AddCheck("free", source.FreeZone, Field.Store.NO, Field.Index.ANALYZED);
			d.AddCheck("creatorId", source.CreatorId, Field.Store.YES, Field.Index.NOT_ANALYZED);
			string tags = "";
			if (source.Tags != null)
			{
				foreach (string tag in source.Tags)
				{
					tags += tag + " ";
				}
				d.AddCheck("tags", tags, Field.Store.YES, Field.Index.ANALYZED);
			}
			lock (theWriter)
			{
				theWriter.AddDocument(d, thePerFieldAnalyzer);
				theWriter.Commit();
			}
			aResult.Return();
			return aResult;
		}

		public Result DeleteSource(string id, Result aResult)
		{
			QueryParser queryParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "id", thePerFieldAnalyzer);
			Query query = queryParser.Parse(id);
			theWriter.DeleteDocuments(query);
			aResult.Return();
			return aResult;
		}

		public Result UpdateSource(ISource source, Result aResult)
		{
			this.DeleteSource(source.Id, new Result());
			this.AddSource(source, new Result());
			aResult.Return();
			return aResult;
		}

		public Result AddSourcePage(ISourcePage source, Result aResult)
		{
			Document d = new Document();
			d.AddCheck("otype", "sourcePage", Field.Store.NO, Field.Index.NOT_ANALYZED);
			d.AddCheck("id", source.Id, Field.Store.YES, Field.Index.NOT_ANALYZED);
			d.AddCheck("rev", source.Rev, Field.Store.YES, Field.Index.NOT_ANALYZED);
			d.AddCheck("pageNumber", source.PageNumber, Field.Store.YES, Field.Index.ANALYZED);
			d.AddCheck("displayPageNumber", source.DisplayPageNumber, Field.Store.YES, Field.Index.ANALYZED);
			d.AddCheck("textContent", source.TextContent, Field.Store.NO, Field.Index.ANALYZED);
			d.AddCheck("sourceId", source.SourceId, Field.Store.YES, Field.Index.ANALYZED);
			d.AddCheck("creatorId", source.CreatorId, Field.Store.YES, Field.Index.NOT_ANALYZED);
			lock (theWriter)
			{
				theWriter.AddDocument(d, thePerFieldAnalyzer);
				theWriter.Commit();
			}
			aResult.Return();
			return aResult;
		}

		public Result DeleteSourcePage(string id, Result aResult)
		{
			QueryParser queryParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "id", thePerFieldAnalyzer);
			Query query = queryParser.Parse(id);
			theWriter.DeleteDocuments(query);
			aResult.Return();
			return aResult;
		}

		public Result UpdateSourcePage(ISourcePage aSourcePage, Result aResult)
		{
			this.DeleteSourcePage(aSourcePage.Id, new Result());
			this.AddSourcePage(aSourcePage, new Result());
			aResult.Return();
			return aResult;
		}

		public string ToJson<T>(SearchResult<T> aSearchResult) where T : ISearchResultItem
		{
			JObject jo = new JObject();
			jo.Add("offset", aSearchResult.Offset);
			jo.Add("max", aSearchResult.Max);
			jo.Add("total_rows", aSearchResult.TotalCount);
			JArray ja = new JArray();
			foreach (ISearchResultItem row in aSearchResult)
			{
				ja.Add(JObject.Parse(row.ToJson()));
			}
			jo.Add("rows", ja);
			return jo.ToString();
		}

		public Result<SearchResult<IScoreSearchResult>> ScoreSearch(int max, int offset, string aLuceneQuery, Result<SearchResult<IScoreSearchResult>> aResult)
		{
			QueryParser qp = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "title", thePerFieldAnalyzer);
			Query q = qp.Parse(aLuceneQuery);
			string toto = q.ToString();

			IndexReader reader = IndexReader.Open(theDirectory, true);
			Searcher indexSearch = new IndexSearcher(reader);

			TopDocs topDocs = indexSearch.Search(q, reader.MaxDoc());
			IList<IScoreSearchResult> results = new List<IScoreSearchResult>();
			if (offset < 0 || offset > topDocs.totalHits)
				throw new Exception();
			int ToGo = (offset + max) > topDocs.totalHits ? topDocs.totalHits : (offset + max);
			for (int i = offset; i < ToGo; i++)
			{
				Document d = reader.Document(topDocs.scoreDocs[i].doc);
				ScoreSearchResult score = GetScoreSearchResultFromDocument(d);
				results.Add(score);
			}
			SearchResult<IScoreSearchResult> searchResult = new SearchResult<IScoreSearchResult>(results, offset, max, topDocs.totalHits);
			aResult.Return(searchResult);
			return aResult;
		}

		private static ScoreSearchResult GetScoreSearchResultFromDocument(Document d)
		{
			ScoreSearchResult score = new ScoreSearchResult();
			score.Id = d.ExtractValue("id");
			score.Title = d.ExtractValue("title");
			score.Composer = d.ExtractValue("composer");
			score.Editor = d.ExtractValue("editor");
			score.Verses = d.ExtractValue("verses");

			string json = d.ExtractValue("musicalSourceReference");
			score.MusicalSourceReference = String.IsNullOrEmpty(json) ? null : JObject.Parse(json);

			json = d.ExtractValue("textualSourceReference");
			score.TextualSourceReference = String.IsNullOrEmpty(json) ? null : JObject.Parse(json);

			return score;
		}

		public Result<SearchResult<IScoreSearchResult>> SearchScore(ScoreQuery aQuery, Result<SearchResult<IScoreSearchResult>> aResult)
		{
			theLogger.Info("Searching Score");
			QueryParser qp = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "title", thePerFieldAnalyzer);

			StringBuilder queryString = new StringBuilder();
			queryString.Append("+otype:score ");
			if (!String.IsNullOrEmpty(aQuery.TitleWild))
			{
				string[] titleParts = aQuery.TitleWild.Split(new[] { ' ' });
				string titleWithoutSpaces = titleParts.Aggregate(String.Empty, (aCurrent, aPart) => aCurrent + aPart);
				queryString.AppendFormat("+titleWithoutSpaces:{0} ", titleWithoutSpaces + "*");
			}
			if (!String.IsNullOrEmpty(aQuery.Title))
			{
				queryString.AppendFormat("+title:\"{0}\" ", aQuery.Title);
			}
			if (!String.IsNullOrEmpty(aQuery.Composer))
			{
				queryString.AppendFormat("+composer:\"{0}\" ", aQuery.Composer);
			}
			if (!String.IsNullOrEmpty(aQuery.Editor))
			{
				queryString.AppendFormat("+editor:\"{0}\" ", aQuery.Editor);
			}
			if (!String.IsNullOrEmpty(aQuery.Verses))
			{
				queryString.AppendFormat("+verses:\"{0}\" ", aQuery.Verses);
			}
			if (!String.IsNullOrEmpty(aQuery.Music))
			{
				queryString.AppendFormat("+(codageMelodiqueRISM:\"{0}\" or ", LilyToCodageMelodiqueRISM(aQuery.Music));

				queryString.AppendFormat("codageParIntervalles:\"{0}\" ) ", LilyToCodageParIntervalles(aQuery.Music));
			}
			if (!String.IsNullOrEmpty(aQuery.IsMaster))
			{
				queryString.AppendFormat("+isMaster:\"{0}\" ", aQuery.IsMaster);
			}

			if (!String.IsNullOrEmpty(aQuery.MasterId))
			{
				queryString.AppendFormat("+masterId:\"{0}\"", aQuery.MasterId);
			}

			Query q = qp.Parse(queryString.ToString());
			string toto = q.ToString();

			IndexReader reader = IndexReader.Open(theDirectory, true);
			Searcher indexSearch = new IndexSearcher(reader);

			TopDocs topDocs = indexSearch.Search(q, reader.MaxDoc());
			IList<IScoreSearchResult> results = new List<IScoreSearchResult>();
			if (aQuery.Offset < 0 || aQuery.Offset > topDocs.totalHits)
				throw new Exception();
			int ToGo = (aQuery.Offset + aQuery.Max) > topDocs.totalHits ? topDocs.totalHits : (aQuery.Offset + aQuery.Max);
			if (aQuery.Max == 0)
				ToGo = topDocs.totalHits;
			for (int i = aQuery.Offset; i < ToGo; i++)
			{
				Document d = reader.Document(topDocs.scoreDocs[i].doc);
				ScoreSearchResult score = GetScoreSearchResultFromDocument(d);
				results.Add(score);
			}
			SearchResult<IScoreSearchResult> searchResult = new SearchResult<IScoreSearchResult>(results, aQuery.Offset, aQuery.Max, topDocs.totalHits);
			aResult.Return(searchResult);
			reader.Close();
			indexSearch.Close();
			return aResult;
		}

		public Result<SearchResult<IScoreSearchResult>> GetAllScores(int max, int offset, Result<SearchResult<IScoreSearchResult>> aResult)
		{
			QueryParser qp = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "title", thePerFieldAnalyzer);
			StringBuilder queryString = new StringBuilder();
			queryString.Append("+otype:score ");
			Query q = qp.Parse(queryString.ToString());
			string toto = q.ToString();

			IndexReader reader = IndexReader.Open(theDirectory, true);
			if (reader.NumDocs() == 0)
			{
				aResult.Return(new SearchResult<IScoreSearchResult>(new List<IScoreSearchResult>(), 0, 0, 0));
			}
			else
			{
				Searcher indexSearch = new IndexSearcher(reader);
				TopDocs topDocs = indexSearch.Search(q, reader.MaxDoc());
				IList<IScoreSearchResult> results = new List<IScoreSearchResult>();
				if (offset < 0 || offset > topDocs.totalHits)
					throw new Exception();
				int ToGo = (offset + max) > topDocs.totalHits ? topDocs.totalHits : (offset + max);
				if (max == 0)
					ToGo = topDocs.totalHits;
				for (int i = offset; i < ToGo; i++)
				{
					Document d = reader.Document(topDocs.scoreDocs[i].doc);
					ScoreSearchResult score = GetScoreSearchResultFromDocument(d);
					results.Add(score);
				}
				SearchResult<IScoreSearchResult> searchResult = new SearchResult<IScoreSearchResult>(results, offset, max, topDocs.totalHits);
				aResult.Return(searchResult);
				indexSearch.Close();
			}
			reader.Close();
			return aResult;
		}

		public Result<SearchResult<ISourceSearchResult>> GetAllSources(int max, int offset, Result<SearchResult<ISourceSearchResult>> aResult)
		{
			QueryParser qp = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "title", thePerFieldAnalyzer);
			StringBuilder queryString = new StringBuilder();
			queryString.Append("+otype:source ");
			Query q = qp.Parse(queryString.ToString());
			string toto = q.ToString();

			IndexReader reader = IndexReader.Open(theDirectory, true);
			if (reader.NumDocs() == 0)
			{
				aResult.Return(new SearchResult<ISourceSearchResult>(new List<ISourceSearchResult>(), 0, 0, 0));
			}else{
				Searcher indexSearch = new IndexSearcher(reader);

				TopDocs topDocs = indexSearch.Search(q, reader.MaxDoc());
				IList<ISourceSearchResult> results = new List<ISourceSearchResult>();
				if (offset < 0 || offset > topDocs.totalHits)
					throw new Exception();
				int ToGo = (offset + max) > topDocs.totalHits ? topDocs.totalHits : (offset + max);
				if (max == 0)
					ToGo = topDocs.totalHits;
				for (int i = offset; i < ToGo; i++)
				{
					Document d = reader.Document(topDocs.scoreDocs[i].doc);
					SourceSearchResult source = new SourceSearchResult();
					source.Id = d.ExtractValue("id");
					source.Name = d.ExtractValue("name");
					source.Publisher = d.ExtractValue("publisher");
					source.DateFrom = d.ExtractValue("dateFrom");
					source.DateTo = d.ExtractValue("dateTo");
					results.Add(source);
				}
				SearchResult<ISourceSearchResult> searchResult = new SearchResult<ISourceSearchResult>(results, offset, max, topDocs.totalHits);
				aResult.Return(searchResult);
				indexSearch.Close();
			}
			reader.Close();
			return aResult;
		}

		public Result<SearchResult<ISourcePageSearchResult>> GetAllPagesFromSource(string sourceId, int max, int offset, Result<SearchResult<ISourcePageSearchResult>> aResult)
		{
			QueryParser qp = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "title", thePerFieldAnalyzer);
			StringBuilder queryString = new StringBuilder();
			queryString.AppendFormat("+otype:sourcePage +sourceId:{0}", sourceId);
			Query q = qp.Parse(queryString.ToString());
			string toto = q.ToString();

			IndexReader reader = IndexReader.Open(theDirectory, true);
			if (reader.NumDocs() == 0)
			{
				aResult.Return(new SearchResult<ISourcePageSearchResult>(new List<ISourcePageSearchResult>(), 0, 0, 0));
			}
			else
			{
				Searcher indexSearch = new IndexSearcher(reader);

				TopDocs topDocs = indexSearch.Search(q, reader.MaxDoc());
				IList<ISourcePageSearchResult> results = new List<ISourcePageSearchResult>();
				if (offset < 0 || offset > topDocs.totalHits)
					throw new Exception();
				int ToGo = (offset + max) > topDocs.totalHits ? topDocs.totalHits : (offset + max);
				if (max == 0)
					ToGo = topDocs.totalHits;
				for (int i = offset; i < ToGo; i++)
				{
					Document d = reader.Document(topDocs.scoreDocs[i].doc);
					SourcePageSearchResult sourcePage = new SourcePageSearchResult();
					sourcePage.Id = d.ExtractValue("id");
					sourcePage.PageNumber = d.ExtractValue("pageNumber");
					sourcePage.DisplayPageNumber = d.ExtractValue("displayPageNumber");
					sourcePage.SourceId = d.ExtractValue("sourceId");
					results.Add(sourcePage);
				}
				SearchResult<ISourcePageSearchResult> searchResult = new SearchResult<ISourcePageSearchResult>(results, offset, max, topDocs.totalHits);
				aResult.Return(searchResult);
				indexSearch.Close();
			}
			reader.Close();
			return aResult;
		}

		public string LilyToCodageParIntervalles(string lily)
		{
			StringBuilder result = new StringBuilder();
			string[] notes = lily.Split(new char[] { ' ' });
			int lastNoteValue = -1;
			foreach (string note in notes)
			{
				int valeur = 0;
				string maNote = note;
				Match m = Regex.Match(maNote, @"\w+'*");
				if (!m.Success)
					break;
				maNote = m.Value;
				while (maNote.Substring(maNote.Length - 1) == "'")
				{
					valeur += 12;
					maNote = maNote.Substring(0, maNote.Length - 1);
				}
				if (maNote == "r")
					break;
				for (int i = 0; i < thePitches.Length; i++)
				{
					if (thePitches[i] == maNote)
					{
						valeur += thePitchesValue[i];
						if (lastNoteValue == -1)
						{
							lastNoteValue = valeur;
						}
						else
						{
							result.AppendFormat(" {0}", valeur - lastNoteValue);
							lastNoteValue = valeur;
						}
						break;
					}
				}
			}
			return result.ToString();
		}

		public string LilyToCodageMelodiqueRISM(string lily)
		{
			StringBuilder result = new StringBuilder("0");
			string[] notes = lily.Split(new char[] { ' ' });
			int lastNoteValue = -1;
			foreach (string note in notes)
			{
				int valeur = 0;
				string maNote = note;
				Match m = Regex.Match(maNote, @"\w+'*");
				if (!m.Success)
					break;
				maNote = m.Value;
				while (maNote.Substring(maNote.Length - 1) == "'")
				{
					valeur += 12;
					maNote = maNote.Substring(0, maNote.Length - 1);
				}
				if (maNote == "r")
					break;
				for (int i = 0; i < thePitches.Length; i++)
				{
					if (thePitches[i] == maNote)
					{
						valeur += thePitchesValue[i];
						if (lastNoteValue == -1)
						{
							lastNoteValue = valeur;
						}
						else
						{
							result.AppendFormat(" {0}", valeur - lastNoteValue);
						}
						break;
					}
				}
			}
			return result.ToString();
		}

		private string GetMusicalSourceText(IScore aScore)
		{
			if (aScore.MusicalSource != null)
			{
				JObject json = JObject.Parse(theScoreDataMapper.ToJson(aScore));
				return json["musicalSource"].ToString();
			}
			return null;
		}
		private string GetTextualSourceText(IScore aScore)
		{
			if (aScore.MusicalSource != null)
			{
				JObject json = JObject.Parse(theScoreDataMapper.ToJson(aScore));
				return json["textualSource"].ToString();
			}
			return null;
		}
		private static JObject GetMusicalSourceText(IMusicalSource aMusicalSourceReference, ISource aSource)
		{
			if (aMusicalSourceReference == null)
				throw new ArgumentNullException("aMusicalSourceReference");
			
			JObject result = new JObject();
			if (aSource != null)
			{
				result = GetSourceReferenceText(aMusicalSourceReference, aSource);
				if (aMusicalSourceReference.IsSuggested && aMusicalSourceReference.IsSuggested)
				{
					result.Add("suggested",true);
				}
			}
			return result;
		}
		private static JObject GetTextualSourceText(ITextualSource aTextualSource, ISource aSource, IPlay aPlay)
		{
			if (aTextualSource == null)
				throw new ArgumentNullException("aTextualSource");

			JObject result = new JObject();
			if (aSource != null)
			{
				result = GetSourceReferenceText(aTextualSource, aSource);

				if (aPlay != null)
				{
					result.Add("playTitle",aPlay.Title);
					if (aTextualSource.ActNumber.HasValue)
					{
						result.Add("act",aTextualSource.ActNumber.Value);
					}
					if (aTextualSource.SceneNumber.HasValue)
					{
						result.Add("scene", aTextualSource.SceneNumber.Value);
					}
				}
			}

			return result;
		}
		private static JObject GetSourceReferenceText(ISourceReference aSourceReference, ISource aSource)
		{
			if (aSourceReference == null)
				throw new ArgumentNullException("aSourceReference");

			JObject result = new JObject();

			if (aSource != null)
			{
				result.Add("title",aSource.Name);

				if (aSource.DateFrom.HasValue)
				{
					result.Add("dateFrom",aSource.DateFrom.Value);
				}
				if (!String.IsNullOrEmpty(aSourceReference.Page))
				{
					result.Add("page", aSourceReference.Page);
				}
				if (aSourceReference.AirNumber.HasValue)
				{
					result.Add("air",aSourceReference.AirNumber.Value);
				}
				if (aSourceReference.Tome.HasValue)
				{
					result.Add("tome", aSourceReference.Tome.Value);
				}
				if (aSourceReference.Volume.HasValue)
				{
					result.Add("volume", aSourceReference.Volume.Value);
				}
			}

			return result;
		}
	}

	public static class DocumentHelper
	{
		public static void AddCheck(this Document doc, string fieldName, string fieldValue, Field.Store fieldStore, Field.Index fieldIndex)
		{
			if (fieldValue != null)
				doc.Add(new Field(fieldName, fieldValue, fieldStore, fieldIndex));
		}

		public static void AddCheck(this Document doc, string fieldName, bool fieldValue, Field.Store fieldStore, Field.Index fieldIndex)
		{
			if (fieldValue != null)
				doc.Add(new Field(fieldName, fieldValue.ToString(), fieldStore, fieldIndex));
		}

		public static void AddCheck(this Document doc, string fieldName, int? fieldValue, Field.Store fieldStore, Field.Index fieldIndex)
		{
			if (fieldValue != null && fieldValue.Value!=null)
				doc.Add(new Field(fieldName, fieldValue.Value.ToString(), fieldStore, fieldIndex));
		}

		public static string ExtractValue(this Document doc, string fieldName)
		{
			if (doc.GetField(fieldName) != null)
				return doc.GetField(fieldName).StringValue();
			return null;
		}
	}
}
