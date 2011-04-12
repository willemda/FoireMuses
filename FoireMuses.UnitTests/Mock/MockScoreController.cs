﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public Result<IScore> AttachMusicXml(IScore aScore, MindTouch.Xml.XDoc xdoc, bool overwriteMusicXmlValues, Result<IScore> aResult)
        {
            if (!overwriteMusicXmlValues)
            {
                IScore themusicxmlScore = FromXml(xdoc);
                aScore.CodageMelodiqueRISM = themusicxmlScore.CodageMelodiqueRISM;
                aScore.CodageParIntervalle = themusicxmlScore.CodageParIntervalle;
                aScore.Title = themusicxmlScore.Title;
                aScore.Composer = themusicxmlScore.Composer;
                aScore.Verses = themusicxmlScore.Verses;
            }
            score = aScore;
            attachment = new MemoryStream(xdoc.ToBytes());
            aResult.Return(score);
            return aResult;
        }


        public IScore CreateNew()
        {
            throw new NotImplementedException();
        }
    }
}
