using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace FoireMuses.Core.Controllers
{
	public class IndexController : IIndexController
	{
		private Directory directory;
		private IndexWriter writer;
		private PerFieldAnalyzerWrapper perFieldAnalyzer;
		private Analyzer whiteSpaceAnalyzer;
		private Analyzer standardAnalyzer;
		private Analyzer keywordAnalyzer;
		private INotificationManager theNotificationManager;
		private static readonly log4net.ILog theLogger = log4net.LogManager.GetLogger(typeof(IndexController));

		public IndexController(INotificationManager notif)
		{
			theLogger.Info("Creation of the IndexController");
			theNotificationManager = notif;
			directory = FSDirectory.Open(new System.IO.DirectoryInfo(System.IO.Path.Combine(Environment.CurrentDirectory,"LuceneIndex")));
			whiteSpaceAnalyzer = new WhitespaceAnalyzer();
			standardAnalyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);
			keywordAnalyzer = new KeywordAnalyzer();
			perFieldAnalyzer = new PerFieldAnalyzerWrapper(standardAnalyzer);
			perFieldAnalyzer.AddAnalyzer("CodageRythmique", whiteSpaceAnalyzer);
			perFieldAnalyzer.AddAnalyzer("CodageMelodiqueRISM", whiteSpaceAnalyzer);
			perFieldAnalyzer.AddAnalyzer("CodageParIntervalles", whiteSpaceAnalyzer);
			perFieldAnalyzer.AddAnalyzer("otype", keywordAnalyzer);
			perFieldAnalyzer.AddAnalyzer("sourceId", keywordAnalyzer);
			perFieldAnalyzer.AddAnalyzer("id", keywordAnalyzer);
			perFieldAnalyzer.AddAnalyzer("rev", keywordAnalyzer);
			writer = new IndexWriter(directory, perFieldAnalyzer, !IndexReader.IndexExists(directory), IndexWriter.MaxFieldLength.LIMITED);
			theNotificationManager.ScoreChanged += new EventHandler<EventArgs<IScore>>(theNotificationManager_ScoreChanged);
			theNotificationManager.PlayChanged += new EventHandler<EventArgs<IPlay>>(theNotificationManager_PlayChanged);
			theNotificationManager.SourceChanged += new EventHandler<EventArgs<ISource>>(theNotificationManager_SourceChanged);
			theNotificationManager.SourcePageChanged += new EventHandler<EventArgs<ISourcePage>>(theNotificationManager_SourcePageChanged);
			theNotificationManager.Start();
			theLogger.Info("IndexController created");
		}

		void theNotificationManager_SourcePageChanged(object sender, EventArgs<ISourcePage> e)
		{
			theLogger.Info("Source Page Changed - updating index");
			UpdateSourcePage(e.Item, new Result()).Wait();
		}

		void theNotificationManager_SourceChanged(object sender, EventArgs<ISource> e)
		{
			theLogger.Info("Source Changed - updating index");
			UpdateSource(e.Item, new Result()).Wait();
		}

		void theNotificationManager_PlayChanged(object sender, EventArgs<IPlay> e)
		{
			theLogger.Info("Play Changed - updating index");
			//throw new NotImplementedException();
		}

		void theNotificationManager_ScoreChanged(object sender, EventArgs<IScore> e)
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
				string titleWithoutSpaces = "";
				foreach (string part in titleParts)
				{
					titleWithoutSpaces += part;
				}
				d.AddCheck("titleWithoutSpaces", titleWithoutSpaces, Field.Store.YES, Field.Index.ANALYZED);
			}
			d.AddCheck("strophe", score.Stanza, Field.Store.NO, Field.Index.ANALYZED);
			d.AddCheck("coirault", score.Coirault, Field.Store.NO, Field.Index.ANALYZED);
			d.AddCheck("delarue", score.Delarue, Field.Store.NO, Field.Index.ANALYZED);
			string tags = "";
			if (score.Tags != null)
			{
				foreach (string tag in score.Tags)
				{
					tags += tag + " ";
				}
				d.AddCheck("tags", tags, Field.Store.YES, Field.Index.ANALYZED);
			}
			lock (writer)
			{
				writer.AddDocument(d, perFieldAnalyzer);
				writer.Commit();
			}
			aResult.Return();
			return aResult;
		}


		public Result DeleteScore(string id, Result aResult)
		{
			QueryParser queryParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "id", perFieldAnalyzer);
			Query query = queryParser.Parse(id);
			writer.DeleteDocuments(query);
			aResult.Return();
			return aResult;
		}


		public Result UpdateScore(IScore score, Result aResult)
		{
			this.DeleteScore(score.Id, new Result());
			this.AddScore(score, new Result());
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
			lock (writer)
			{
				writer.AddDocument(d, perFieldAnalyzer);
				writer.Commit();
			}
			aResult.Return();
			return aResult;
		}


		public Result DeleteSource(string id, Result aResult)
		{
			QueryParser queryParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "id", perFieldAnalyzer);
			Query query = queryParser.Parse(id);
			writer.DeleteDocuments(query);
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
			lock (writer)
			{
				writer.AddDocument(d, perFieldAnalyzer);
				writer.Commit();
			}
			aResult.Return();
			return aResult;
		}


		public Result DeleteSourcePage(string id, Result aResult)
		{
			QueryParser queryParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "id", perFieldAnalyzer);
			Query query = queryParser.Parse(id);
			writer.DeleteDocuments(query);
			aResult.Return();
			return aResult;
		}


		public Result UpdateSourcePage(ISourcePage sourcePage, Result aResult)
		{
			this.DeleteSourcePage(sourcePage.Id, new Result());
			this.AddSourcePage(sourcePage, new Result());
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
			QueryParser qp = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "title", perFieldAnalyzer);
			Query q = qp.Parse(aLuceneQuery);
			string toto = q.ToString();

			IndexReader reader = IndexReader.Open(directory, true);
			Searcher indexSearch = new IndexSearcher(reader);

			TopDocs topDocs = indexSearch.Search(q, reader.MaxDoc());
			IList<IScoreSearchResult> results = new List<IScoreSearchResult>();
			if (offset < 0 || offset > topDocs.totalHits)
				throw new Exception();
			int ToGo = (offset + max) > topDocs.totalHits ? topDocs.totalHits : (offset + max);
			for (int i = offset; i < ToGo; i++)
			{
				Document d = reader.Document(topDocs.scoreDocs[i].doc);
				ScoreSearchResult score = new ScoreSearchResult();
				score.Id = d.ExtractValue("Id");
				score.Title = d.ExtractValue("Title");
				score.Composer = d.ExtractValue("Composer");
				score.Editor = d.ExtractValue("Editor");
				score.Verses = d.ExtractValue("Verses");
				score.Music = d.ExtractValue("Music");
				results.Add(score);
			}
			SearchResult<IScoreSearchResult> searchResult = new SearchResult<IScoreSearchResult>(results, offset, max, topDocs.totalHits);
			aResult.Return(searchResult);
			return aResult;
		}

		public Result<SearchResult<IScoreSearchResult>> SearchScore(ScoreQuery query, Result<SearchResult<IScoreSearchResult>> aResult)
		{
			theLogger.Info("Searching Score");
			QueryParser qp = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "title", perFieldAnalyzer);

			StringBuilder queryString = new StringBuilder();
			queryString.Append("+otype:score ");
			if (!String.IsNullOrEmpty(query.TitleWild))
			{
				string[] titleParts = query.TitleWild.Split(new char[] { ' ' });
				string titleWithoutSpaces = "";
				foreach (string part in titleParts)
				{
					titleWithoutSpaces += part;
				}
				queryString.AppendFormat("+titleWithoutSpaces:{0} ", titleWithoutSpaces + "*");
			}
			if (!String.IsNullOrEmpty(query.Title))
			{
				queryString.AppendFormat("+title:\"{0}\" ", query.Title);
			}
			if (!String.IsNullOrEmpty(query.Composer))
			{
				queryString.AppendFormat("+composer:\"{0}\" ", query.Composer);
			}
			if (!String.IsNullOrEmpty(query.Editor))
			{
				queryString.AppendFormat("+editor:\"{0}\" ", query.Editor);
			}
			if (!String.IsNullOrEmpty(query.Verses))
			{
				queryString.AppendFormat("+verses:\"{0}\" ", query.Verses);
			}
			if (!String.IsNullOrEmpty(query.Music))
			{
				queryString.AppendFormat("codageMelodiqueRISM:\"{0}\" ", LilyToCodageMelodiqueRISM(query.Music));

				queryString.AppendFormat("codageParIntervalles:\"{0}\" ", LilyToCodageParIntervalles(query.Music));
			}
			if (!String.IsNullOrEmpty(query.IsMaster))
			{
				queryString.AppendFormat("+isMaster:\"{0}\" ", query.IsMaster);
			}

			if (!String.IsNullOrEmpty(query.MasterId))
			{
				queryString.AppendFormat("+masterId:\"{0}\"", query.MasterId);
			}

			Query q = qp.Parse(queryString.ToString());
			string toto = q.ToString();

			IndexReader reader = IndexReader.Open(directory, true);
			Searcher indexSearch = new IndexSearcher(reader);

			TopDocs topDocs = indexSearch.Search(q, reader.MaxDoc());
			IList<IScoreSearchResult> results = new List<IScoreSearchResult>();
			if (query.Offset < 0 || query.Offset > topDocs.totalHits)
				throw new Exception();
			int ToGo = (query.Offset + query.Max) > topDocs.totalHits ? topDocs.totalHits : (query.Offset + query.Max);
			if (query.Max == 0)
				ToGo = topDocs.totalHits;
			for (int i = query.Offset; i < ToGo; i++)
			{
				Document d = reader.Document(topDocs.scoreDocs[i].doc);
				ScoreSearchResult score = new ScoreSearchResult();
				score.Id = d.ExtractValue("id");
				score.Title = d.ExtractValue("title");
				score.Composer = d.ExtractValue("composer");
				score.Editor = d.ExtractValue("editor");
				score.Verses = d.ExtractValue("verses");
				score.Music = d.ExtractValue("music");
				results.Add(score);
			}
			SearchResult<IScoreSearchResult> searchResult = new SearchResult<IScoreSearchResult>(results, query.Offset, query.Max, topDocs.totalHits);
			aResult.Return(searchResult);
			reader.Close();
			indexSearch.Close();
			return aResult;
		}


		public Result<SearchResult<IScoreSearchResult>> GetAllScores(int max, int offset, Result<SearchResult<IScoreSearchResult>> aResult)
		{
			QueryParser qp = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "title", perFieldAnalyzer);
			StringBuilder queryString = new StringBuilder();
			queryString.Append("+otype:score ");
			Query q = qp.Parse(queryString.ToString());
			string toto = q.ToString();

			IndexReader reader = IndexReader.Open(directory, true);
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
				ScoreSearchResult score = new ScoreSearchResult();
				score.Id = d.ExtractValue("id");
				score.Title = d.ExtractValue("title");
				score.Composer = d.ExtractValue("composer");
				score.Editor = d.ExtractValue("editor");
				score.Verses = d.ExtractValue("verses");
				score.Music = d.ExtractValue("music");
				results.Add(score);
			}
			SearchResult<IScoreSearchResult> searchResult = new SearchResult<IScoreSearchResult>(results, offset, max, topDocs.totalHits);
			aResult.Return(searchResult);
			reader.Close();
			indexSearch.Close();
			return aResult;
		}


		public Result<SearchResult<ISourceSearchResult>> GetAllSources(int max, int offset, Result<SearchResult<ISourceSearchResult>> aResult)
		{
			QueryParser qp = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "title", perFieldAnalyzer);
			StringBuilder queryString = new StringBuilder();
			queryString.Append("+otype:source ");
			Query q = qp.Parse(queryString.ToString());
			string toto = q.ToString();

			IndexReader reader = IndexReader.Open(directory, true);
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
			reader.Close();
			indexSearch.Close();
			return aResult;
		}


		public Result<SearchResult<ISourcePageSearchResult>> GetAllPagesFromSource(string sourceId, int max, int offset, Result<SearchResult<ISourcePageSearchResult>> aResult)
		{
			QueryParser qp = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "title", perFieldAnalyzer);
			StringBuilder queryString = new StringBuilder();
			queryString.AppendFormat("+otype:sourcePage +sourceId:{0}", sourceId);
			Query q = qp.Parse(queryString.ToString());
			string toto = q.ToString();

			IndexReader reader = IndexReader.Open(directory, true);
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
			reader.Close();
			indexSearch.Close();
			return aResult;
		}


		private string[] pitches = new string[] { "c", "ces", "cis", "d", "des", "dis", "e", "f", "fes", "fis", "g", "ges", "gis", "a", "aes", "ais", "b" };
		private int[] pitchesValue = new int[] { 0, 1, 1, 2, 3, 3, 4, 5, 6, 6, 7, 8, 8, 9, 10, 10, 11 };



		public string LilyToCodageParIntervalles(string lily)
		{
			string cpi = "";
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
				for (int i = 0; i < pitches.Length; i++)
				{
					if (pitches[i] == maNote)
					{
						valeur += pitchesValue[i];
						if (lastNoteValue == -1)
						{
							lastNoteValue = valeur;
						}
						else
						{
							cpi += " ";
							cpi += valeur - lastNoteValue;
							lastNoteValue = valeur;
						}
						break;
					}
				}
			}
			return cpi;
		}

		public string LilyToCodageMelodiqueRISM(string lily)
		{
			string cpi = "0";
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
				for (int i = 0; i < pitches.Length; i++)
				{
					if (pitches[i] == maNote)
					{
						valeur += pitchesValue[i];
						if (lastNoteValue == -1)
						{
							lastNoteValue = valeur;
						}
						else
						{
							cpi += " ";
							cpi += valeur - lastNoteValue;
						}
						break;
					}
				}
			}
			return cpi;
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
