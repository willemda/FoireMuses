using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Tasking;
using Newtonsoft.Json.Linq;
using LoveSeat;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core;

namespace FoireMuses.UnitTests.Mock
{
	internal class MockScoreController : IScoreController
	{
		private IScore score = null;


		public Result<Core.SearchResult<IScore>> GetScoresFromSource(int offset, int max, ISource aJSource, Result<Core.SearchResult<IScore>> aResult)
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

		public Result<bool> Delete(string id, Result<bool> aResult)
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
			throw new NotImplementedException();
		}

		public string ToJson(IScore aJson)
		{
			throw new NotImplementedException();
		}
	}
}
