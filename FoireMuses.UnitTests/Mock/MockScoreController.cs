using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Business;
using MindTouch.Tasking;
using Newtonsoft.Json.Linq;
using LoveSeat;
using FoireMuses.Core.Interfaces;

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

		public Result<IScore> Update(IScore aDoc, Result<IScore> aResult)
		{
			score = aDoc;
			aResult.Return(score);
			return aResult;
		}

		public Result<IScore> Get(IScore aDoc, Result<IScore> aResult)
		{
			aResult.Return(score);
			return aResult;
		}

		public Result<IScore> Get(string id, Result<IScore> aResult)
		{
			aResult.Return(score);
			return aResult;
		}

		public Result<bool> Delete(IScore aDoc, Result<bool> aResult)
		{
			score = null;
			aResult.Return(true);
			return aResult;
		}

		public Result<Core.SearchResult<IScore>> GetAll(int offset, int max, Result<Core.SearchResult<IScore>> aResult)
		{
			throw new NotImplementedException();
		}
	}
}
