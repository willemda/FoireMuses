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
    public interface IScoreController : IBaseController<JScore>
    {
		Result<ViewResult<string, string, JScore>> GetScoresFromSource (JSource aJSource, Result<ViewResult<string, string, JScore>> aResult);
		Result<ViewResult<string[], string, JScore>> GetScoresFromPlay (JPlay aJPlay,  Result<ViewResult<string[], string, JScore>> aResult);
        Result<ViewResult<string, string>> GetHead(int limit, Result<ViewResult<string, string>> aResult);
        Result<ViewResult<string, string>> GetHead(Result<ViewResult<string, string>> aResult);
    }
}
