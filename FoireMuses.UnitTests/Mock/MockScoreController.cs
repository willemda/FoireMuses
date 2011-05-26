using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Utils;
using MindTouch.Tasking;
using Newtonsoft.Json.Linq;
using LoveSeat;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core;
using System.IO;
using MusicXml;
using FoireMuses.Core.Business;
using FoireMuses.MusicXMLImport;

namespace FoireMuses.UnitTests.Mock
{
	internal class MockScoreController : IScoreController
	{
		private IScore score = null;
        private Stream attachment = null;


		public Result<Core.SearchResult<IScore>> GetScoresFromSource(int offset, int max, string aJSource, Result<Core.SearchResult<IScore>> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<IScore> Create(IScore aDoc, Result<IScore> aResult)
		{
			score = aDoc;
			aResult.Return(score);
			return aResult;
		}

		public Result<IScore> Update(string id, string rev, IScore aDoc, Result<IScore> aResult)
		{
			score = aDoc;
			aResult.Return(score);
			return aResult;
		}

		public Result<IScore> Retrieve(string id, Result<IScore> aResult)
		{
			aResult.Return(score);
			return aResult;
		}

		public Result<bool> Delete(string id, string rev, Result<bool> aResult)
		{
			score = null;
			aResult.Return(true);
			return aResult;
		}

		public Result<SearchResult<IScore>> GetAll(int offset, int max, Result<SearchResult<IScore>> aResult)
		{
			aResult.Return(new SearchResult<IScore>(new IScore[]{score},offset,max,1));
			return aResult;
		}

		public IScore FromJson(string aJson)
		{
            return new JScore(JObject.Parse(aJson));
		}

		public string ToJson(IScore aJson)
		{
            return aJson.ToString();
		}


		public IScore FromXml(MindTouch.Xml.XDoc aXml)
		{
            XScore xscore = new XScore(aXml);
            JScore js = new JScore();
            js["codageParIntervalle"] = xscore.GetCodageParIntervalle();
            js["codageMelodiqueRISM"] = xscore.GetCodageMelodiqueRISM();
            js["verses"] = xscore.GetText();
            js["title"] = xscore.MovementTitle;
            js["composer"] = xscore.Identification.Composer;
            return js;
		}

		public MindTouch.Xml.XDoc ToXml(IScore anObject)
		{
			throw new NotImplementedException();
		}


		public Result<bool> Exists(string id, Result<bool> aResult)
		{
			throw new NotImplementedException();
		}


		public string ToJson(SearchResult<IScore> aSearchResult)
		{
			throw new NotImplementedException();
		}

        public Result<bool> AddAttachment(string id, System.IO.Stream file, string fileName, Result<bool> aResult)
        {
            attachment = file;
            aResult.Return(true);
            return aResult;
        }

		public Result<IScore> AttachMusicXml(IScore aScore, MindTouch.Xml.XDoc aMusicXmlDoc, bool overwriteMusicXmlValues, Result<IScore> aResult)
        {
            if (!overwriteMusicXmlValues)
            {
				XScore musicXml = new XScore(aMusicXmlDoc);
				aScore.CodageMelodiqueRISM = musicXml.GetCodageMelodiqueRISM();
				aScore.CodageParIntervalles = musicXml.GetCodageParIntervalle();
				aScore.Title = musicXml.MovementTitle;
				aScore.Composer = musicXml.Identification.Composer;
				aScore.Verses = musicXml.GetText();
            }
            score = aScore;
			attachment = new MemoryStream(aMusicXmlDoc.ToBytes());
            aResult.Return(score);
            return aResult;
        }


        public IScore CreateNew()
        {
            throw new NotImplementedException();
        }


		public Result<Stream> GetAttachedFile(string scoreId, string fileName, Result<Stream> aResult)
		{
			throw new NotImplementedException();
		}


		public Result<Stream> GetConvertedScore(MindTouch.Dream.MimeType mimetype, string id, Result<Stream> aResult)
		{
			throw new NotImplementedException();
		}


		public Result<bool> TestAsync(string test, Result<bool> aResult)
		{
			throw new NotImplementedException();
		}
	}
}
