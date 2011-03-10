using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveSeat;
using MindTouch.Tasking;
using Newtonsoft.Json.Linq;

namespace Core.Interfaces
{
	internal interface ICouchDBController
	{
		CouchDatabase MyCouchDatabase { get;}
		
		/*Result<JDocument> CreateDocument (JDocument aDocument, Result<JDocument> aResult);
		Result<JDocument> GetDocument (JDocument aDocument, Result<JDocument> aResult);
		Result<JDocument> UpdateDocument (JDocument aDocument, Result<JDocument> aResult);
		Result<JObject> DeleteDocument (JDocument aDocument, Result<JObject> aResult);
		Result<ViewResult<string, string, JDocument>> GetResultForView (string viewId, string viewName, Result<ViewResult<string, string, JDocument>> aResult);
		Result<ViewResult<string[], string, JDocument>> GetResultForView (string viewId, string viewName, Result<ViewResult<string[], string, JDocument>> aResult);*/
	}
}
