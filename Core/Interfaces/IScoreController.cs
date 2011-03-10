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
		Result<JScore> CreateDocument (JDocument aDocument, Result<JScore> aResult);
		Result<JScore> GetDocument (JDocument aDocument, Result<JScore> aResult);
		Result<JScore> UpdateDocument (JDocument aDocument, Result<JScore> aResult);
		Result<JObject> DeleteDocument (JDocument aDocument, Result<JObject> aResult);
		//Result<ViewResult<string, string, JScore>> GetResultForView (string viewId, string viewName, Result<ViewResult<string, string, JScore>> aResult);
		//Result<ViewResult<string[], string, JScore>> GetResultForView (string viewId, string viewName, Result<ViewResult<string[], string, JScore>> aResult);
    }
}
