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
		private INotificationManager theNotificationManager;
		private static readonly log4net.ILog theLogger = log4net.LogManager.GetLogger(typeof(IndexController));

		public IndexController(INotificationManager notif)
		{
			theLogger.Info("Creation of the IndexController");
			theNotificationManager = notif;
			directory = FSDirectory.Open(new System.IO.DirectoryInfo(Environment.CurrentDirectory + "\\LuceneIndex"));
			whiteSpaceAnalyzer = new WhitespaceAnalyzer();
			standardAnalyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);
			perFieldAnalyzer = new PerFieldAnalyzerWrapper(standardAnalyzer);
			perFieldAnalyzer.AddAnalyzer("CodageRythmique", whiteSpaceAnalyzer);
			perFieldAnalyzer.AddAnalyzer("CodageMelodiqueRISM", whiteSpaceAnalyzer);
			perFieldAnalyzer.AddAnalyzer("CodageParIntervalles", whiteSpaceAnalyzer);
			writer = new IndexWriter(directory, perFieldAnalyzer, true, IndexWriter.MaxFieldLength.LIMITED);
			theNotificationManager.ScoreChanged += new EventHandler<EventArgs<IScore>>(theNotificationManager_ScoreChanged);
			theNotificationManager.PlayChanged += new EventHandler<EventArgs<IPlay>>(theNotificationManager_PlayChanged);
			theNotificationManager.SourceChanged += new EventHandler<EventArgs<ISource>>(theNotificationManager_SourceChanged);
			theNotificationManager.Start();
		}

		void theNotificationManager_SourceChanged(object sender, EventArgs<ISource> e)
		{
			theLogger.Info("Source Changed - updating index");
			//throw new NotImplementedException();
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
			d.AddCheck("Id", score.Id, Field.Store.YES, Field.Index.NOT_ANALYZED);
			d.AddCheck("Rev", score.Rev, Field.Store.YES, Field.Index.NOT_ANALYZED);
			d.AddCheck("IsMaster", score.IsMaster, Field.Store.NO, Field.Index.ANALYZED);
			d.AddCheck("CodageMelodiqueRISM", score.CodageMelodiqueRISM, Field.Store.NO, Field.Index.ANALYZED);
			d.AddCheck("CodageParIntervalles", score.CodageParIntervalles, Field.Store.NO, Field.Index.ANALYZED);
			d.AddCheck("CodageRythmique", score.CodageRythmique, Field.Store.NO, Field.Index.ANALYZED);
			d.AddCheck("CodageMelodiqueRISM", score.Code1, Field.Store.NO, Field.Index.ANALYZED);
			d.AddCheck("CodageParIntervalles", score.Code2, Field.Store.NO, Field.Index.ANALYZED);
			d.AddCheck("Composer", score.Composer, Field.Store.YES, Field.Index.ANALYZED);
			d.AddCheck("Editor", score.Editor, Field.Store.YES, Field.Index.ANALYZED);
			d.AddCheck("Comments", score.Comments, Field.Store.NO, Field.Index.ANALYZED);
			d.AddCheck("CreatorId", score.CreatorId, Field.Store.YES, Field.Index.NOT_ANALYZED);
			d.AddCheck("Type", score.ScoreType, Field.Store.YES, Field.Index.ANALYZED);
			d.AddCheck("Verses", score.Verses, Field.Store.YES, Field.Index.ANALYZED);
			d.AddCheck("Title", score.Title, Field.Store.YES, Field.Index.ANALYZED);
			if (score.Title != null)
			{
				string[] titleParts = score.Title.Split(new char[] { ' ' });
				string titleWithoutSpaces = "";
				foreach (string part in titleParts)
				{
					titleWithoutSpaces += part;
				}
				d.AddCheck("TitleWithoutSpaces", titleWithoutSpaces, Field.Store.YES, Field.Index.ANALYZED);
			}
			d.AddCheck("Strophe", score.Stanza, Field.Store.NO, Field.Index.ANALYZED);
			d.AddCheck("Coirault", score.Coirault, Field.Store.NO, Field.Index.ANALYZED);
			d.AddCheck("Delarue", score.Delarue, Field.Store.NO, Field.Index.ANALYZED);
			string tags = "";
			if (score.Tags != null)
			{
				foreach (string tag in score.Tags)
				{
					tags += tag + " ";
				}
				d.AddCheck("Tags", tags, Field.Store.YES, Field.Index.ANALYZED);
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
			QueryParser queryParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "Id", perFieldAnalyzer);
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

		public Result<SearchResult<IScoreSearchResult>> SearchScore(ScoreQuery query, Result<SearchResult<IScoreSearchResult>> aResult)
		{
			theLogger.Info("Searching Score");
			QueryParser qp = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "Title", perFieldAnalyzer);

			StringBuilder queryString = new StringBuilder();
			if (!String.IsNullOrEmpty(query.TitleWild))
			{
				string[] titleParts = query.TitleWild.Split(new char[] { ' ' });
				string titleWithoutSpaces = "";
				foreach (string part in titleParts)
				{
					titleWithoutSpaces += part;
				}
				queryString.AppendFormat("+TitleWithoutSpaces:{0} ", titleWithoutSpaces + "*");
			}
			if (!String.IsNullOrEmpty(query.Title))
			{
				queryString.AppendFormat("+Title:\"{0}\" ", query.Title);
			}
			if (!String.IsNullOrEmpty(query.Composer))
			{
				queryString.AppendFormat("+Composer:\"{0}\" ", query.Composer);
			}
			if (!String.IsNullOrEmpty(query.Editor))
			{
				queryString.AppendFormat("+Editor:\"{0}\" ", query.Editor);
			}
			if (!String.IsNullOrEmpty(query.Verses))
			{
				queryString.AppendFormat("+Verses:\"{0}\" ", query.Verses);
			}
			if (!String.IsNullOrEmpty(query.Music))
			{
				queryString.AppendFormat("CodageMelodiqueRISM:\"{0}\" ", LilyToCodageMelodiqueRISM(query.Music));

				queryString.AppendFormat("CodageParIntervalles:\"{0}\" ", LilyToCodageParIntervalles(query.Music));
			}
			if (!String.IsNullOrEmpty(query.IsMaster))
			{
				queryString.AppendFormat("+IsMaster:\"{0}\"", query.IsMaster);
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
			for (int i = query.Offset; i < ToGo; i++)
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
			SearchResult<IScoreSearchResult> searchResult = new SearchResult<IScoreSearchResult>(results, query.Offset, query.Max, topDocs.totalHits);
			aResult.Return(searchResult);
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

		public static string ExtractValue(this Document doc, string fieldName)
		{
			if (doc.GetField(fieldName) != null)
				return doc.GetField(fieldName).StringValue();
			return null;
		}
	}
}
