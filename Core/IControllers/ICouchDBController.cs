using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveSeat;
using MindTouch.Tasking;
using Newtonsoft.Json.Linq;

namespace Core.IControllers
{
    interface ICouchDBController
    {
        internal Result<JDocument> CreatDocument(JDocument aDocument, Result<JDocument> aResult);
        internal Result<JDocument> GetDocument(JDocument aDocument, Result<JDocument> aResult);
        internal Result<JDocument> UpdateDocument(JDocument aDocument, Result<JDocument> aResult);
        internal Result<JObject> DeleteDocument(JDocument aDocument, Result<JObject> aResult);
        internal Result<ViewResult<string, string, JDocument>> GetResultForView(string viewId, string viewName, Result<ViewResult<string, string, JDocument>> aResult);
        internal Result<ViewResult<string[], string, JDocument>> GetResultForView(string viewId, string viewName, Result<ViewResult<string[], string, JDocument>> aResult);
    }
}
