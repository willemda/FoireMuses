using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Tasking;
using FoireMuses.Core.Business;
using LoveSeat;
using Newtonsoft.Json.Linq;

namespace FoireMuses.Core.Interfaces
{
    public interface IScoreController
    {
		Result<JScore> CreateDocument (JScore aJScore, Result<JScore> aResult);
		Result<JScore> GetDocument (string id, Result<JScore> aResult);
        Result<JScore> GetDocument(JScore aJScore, Result<JScore> aResult);
		Result<JScore> UpdateDocument (JScore aJScore, Result<JScore> aResult);
		Result<JObject> DeleteDocument (JScore aJScore, Result<JObject> aResult);
		Result<ViewResult<string, string, JScore>> GetScoresFromSource (JSource aJSource, Result<ViewResult<string, string, JScore>> aResult);
		Result<ViewResult<string[], string, JScore>> GetScoresFromPlay (JPlay aJPlay,  Result<ViewResult<string[], string, JScore>> aResult);
    }
}
