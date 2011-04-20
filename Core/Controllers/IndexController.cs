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

namespace FoireMuses.Core.Controllers
{
	public class IndexController : IIndexController
	{
		private Directory directory; 
		private IndexWriter writer;
		private PerFieldAnalyzerWrapper perFieldAnalyzer;
		private Analyzer keywordAnalyzer;
		private Analyzer standardAnalyzer;

		public IndexController()
		{
			directory = FSDirectory.Open(new System.IO.DirectoryInfo(Environment.CurrentDirectory + "\\LuceneIndex"));
			keywordAnalyzer = new KeywordAnalyzer();
			standardAnalyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);
			perFieldAnalyzer = new PerFieldAnalyzerWrapper(standardAnalyzer);
			perFieldAnalyzer.AddAnalyzer("codageRythmique", keywordAnalyzer);
			perFieldAnalyzer.AddAnalyzer("codageMelodiqueRISM", keywordAnalyzer);
			perFieldAnalyzer.AddAnalyzer("codageParIntervalles", keywordAnalyzer);
			writer = new IndexWriter(directory, perFieldAnalyzer, true, IndexWriter.MaxFieldLength.LIMITED);
		}


		public Result AddScore(IScore score, Result aResult){
			Document d = new Document();
			d.AddCheck("Id",score.Id,Field.Store.YES, Field.Index.NOT_ANALYZED);
			d.AddCheck("Rev", score.Rev, Field.Store.YES, Field.Index.NOT_ANALYZED);
			d.AddCheck("Composer", score.Composer, Field.Store.YES, Field.Index.ANALYZED);
			d.AddCheck("Editor", score.Editor, Field.Store.YES, Field.Index.ANALYZED);
			d.AddCheck("Comments", score.Comments, Field.Store.NO, Field.Index.ANALYZED);
			d.AddCheck("CreatorId", score.CreatorId, Field.Store.YES, Field.Index.NOT_ANALYZED);
			d.AddCheck("Type", score.ScoreType, Field.Store.YES, Field.Index.ANALYZED);
			d.AddCheck("Verses", score.Verses, Field.Store.YES, Field.Index.ANALYZED);
			d.AddCheck("Title", score.Title, Field.Store.YES, Field.Index.ANALYZED);
			d.AddCheck("Strophe", score.Stanza, Field.Store.NO, Field.Index.ANALYZED);
			d.AddCheck("Coirault", score.Coirault, Field.Store.NO, Field.Index.ANALYZED);
			d.AddCheck("Delarue", score.Delarue, Field.Store.NO, Field.Index.ANALYZED);
			string tags="";
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
				writer.Flush();
			}
			aResult.Return();
			return aResult;
		}


		public Result DeleteScore(IScore score, Result aResult)
		{
			QueryParser queryParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "Id",perFieldAnalyzer);
			Query query = queryParser.Parse(score.Id);
			writer.DeleteDocuments(query);
			aResult.Return();
			return aResult;
		}


		public Result UpdateScore(IScore score, Result aResult)
		{
			this.DeleteScore(score, new Result());
			this.AddScore(score, new Result());
			aResult.Return();
			return aResult;
		}

		public Result<SearchResult<ScoreSearchResult>> SearchScore(ScoreQuery query, Result<SearchResult<ScoreSearchResult>> aResult)
		{
			BooleanQuery q = new Lucene.Net.Search.BooleanQuery();
			if (query.Title != null)
			{
				q.Add(new TermQuery(new Term("Title", query.Title)), BooleanClause.Occur.MUST);
			}
			if (query.Composer != null)
			{
				q.Add(new TermQuery(new Term("Composer", query.Composer)), BooleanClause.Occur.MUST);
			}
			if (query.Editor != null)
			{
				q.Add(new TermQuery(new Term("Editor", query.Title)), BooleanClause.Occur.MUST);
			}
			if (query.Verses != null)
			{
				q.Add(new TermQuery(new Term("Verses", query.Verses)), BooleanClause.Occur.MUST);
			}
			if (query.Music != null)
			{
				//convertir et ajouter comme terme de recherche
			}

			IndexReader reader = IndexReader.Open(directory,true);
			Searcher indexSearch = new IndexSearcher(reader);
			
			TopDocs topDocs = indexSearch.Search(q, reader.MaxDoc());
			IList<ScoreSearchResult> results = new List<ScoreSearchResult>();
			if(query.Offset < 0 || query.Offset > topDocs.totalHits)
				throw new Exception();
			int ToGo = (query.Offset + query.Max) > topDocs.totalHits? topDocs.totalHits:(query.Offset + query.Max);
			for(int i = query.Offset; i<ToGo;i++){
				Document d = reader.Document(topDocs.scoreDocs[i].doc);
				ScoreSearchResult score = new ScoreSearchResult();
				score.Title = d.ExtractValue("Title");
				score.Composer = d.ExtractValue("Composer");
				score.Editor = d.ExtractValue("Editor");
				score.Verses = d.ExtractValue("Verses");
				score.Music = d.ExtractValue("Music");
				results.Add(score);
			}
			SearchResult<ScoreSearchResult> searchResult = new SearchResult<ScoreSearchResult>(results,query.Offset,query.Max,topDocs.totalHits);
			aResult.Return(searchResult);
			return aResult;
		}
	}

	public static class DocumentHelper
	{
		public static void AddCheck(this Document doc, string fieldName, string fieldValue, Field.Store fieldStore, Field.Index fieldIndex)
		{
			if (fieldValue != null)
				doc.Add(new Field(fieldName, fieldValue, fieldStore, fieldIndex));
		}

		public static string ExtractValue(this Document doc, string fieldName)
		{
			if (doc.GetField(fieldName) != null)
				return doc.GetField(fieldName).StringValue();
			return null;
		}
	}
}
