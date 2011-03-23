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

		private JScore monScore;

		public Result<LoveSeat.ViewResult<string, string, FoireMuses.Core.Business.JScore>> GetScoresFromSource(FoireMuses.Core.Business.JSource aJSource, Result<LoveSeat.ViewResult<string, string, FoireMuses.Core.Business.JScore>> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<LoveSeat.ViewResult<string[], string, FoireMuses.Core.Business.JScore>> GetScoresFromPlay(FoireMuses.Core.Business.JPlay aJPlay, Result<LoveSeat.ViewResult<string[], string, FoireMuses.Core.Business.JScore>> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<LoveSeat.ViewResult<string, string>> GetHead(int limit, Result<LoveSeat.ViewResult<string, string>> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<LoveSeat.ViewResult<string, string>> GetAll(Result<LoveSeat.ViewResult<string, string>> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<FoireMuses.Core.Business.JScore> Create(FoireMuses.Core.Business.JScore aDoc, Result<FoireMuses.Core.Business.JScore> aResult)
		{
			monScore = aDoc;
			aResult.Return(monScore);
			return aResult;
		}

		public Result<FoireMuses.Core.Business.JScore> GetById(string id, Result<FoireMuses.Core.Business.JScore> aResult)
		{
			if (monScore != null && monScore.Id == id)
			{
				aResult.Return(monScore);
			}
			else
				aResult.Throw(new Exception("NotFound"));
			return aResult;
		}

		public Result<FoireMuses.Core.Business.JScore> Get(FoireMuses.Core.Business.JScore aDoc, Result<FoireMuses.Core.Business.JScore> aResult)
		{
			if (monScore != null)
			{
				aResult.Return(monScore);
			}
			else
				aResult.Throw(new Exception("NotFound"));
			return aResult;
		}

		public Result<FoireMuses.Core.Business.JScore> Update(FoireMuses.Core.Business.JScore aDoc, Result<FoireMuses.Core.Business.JScore> aResult)
		{
			if (monScore != null)
			{
				monScore = aDoc;
				aResult.Return(monScore);
			}
			else
				aResult.Throw(new Exception("NotFound"));
			return aResult;
		}

		public Result<JObject> Delete(FoireMuses.Core.Business.JScore aDoc, Result<JObject> aResult)
		{
			if (monScore != null)
			{
				monScore = null;
				aResult.Return(aDoc);
			}
			else
				aResult.Throw(new Exception("NotFound"));
			return aResult;
		}

		public void Created()
		{
			throw new NotImplementedException();
		}

		public void Updated()
		{
			throw new NotImplementedException();
		}

		public void Deleted()
		{
			throw new NotImplementedException();
		}



		public Result<LoveSeat.ViewResult<string, string, FoireMuses.Core.Business.JScore>> GetAll(Result<LoveSeat.ViewResult<string, string, FoireMuses.Core.Business.JScore>> aResult)
		{
			throw new NotImplementedException();
		}


		public void Readed(JScore doc, Result<JScore> res)
		{
			throw new NotImplementedException();
		}


		public Result<ViewResult<string, string, JScore>> GetScoresFromPlay(JPlay aJPlay, Result<ViewResult<string, string, JScore>> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<ViewResult<string, string>> GetAll(int limit, Result<ViewResult<string, string>> aResult)
		{
			throw new NotImplementedException();
		}
	}
}
