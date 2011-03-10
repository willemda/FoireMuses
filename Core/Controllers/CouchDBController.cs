using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveSeat;
using MindTouch.Tasking;
using Newtonsoft.Json.Linq;
using Core.IControllers;

namespace Core.Controllers
{
    class CouchDBController : ICouchDBController
    {
        private CouchDatabase theDatabase { get; private set; }

        Result<JDocument> CreateDocument(JDocument aDocument, Result<JDocument> aResult)
        {

            if (aResult == null)
            {
                throw new ArgumentNullException();
            }

            Result<JDocument> res = new Result<JDocument>();
            
            theDatabase.CreateDocument(aDocument, res).WhenDone(
                res.Return,
                res.Throw
                );
            return res;
        }

        Result<JDocument> UpdateDocument(JDocument aDocument, Result<JDocument> aResult)
        {

            if (aResult == null)
            {
                throw new ArgumentNullException();
            }

            Result<JDocument> res = new Result<JDocument>();

            theDatabase.UpdateDocument(aDocument, res).WhenDone(
                res.Return,
                res.Throw
                );
            return res;
        }

        Result<JDocument> GetDocument(string id, Result<JDocument> aResult)
        {

            if (aResult == null)
            {
                throw new ArgumentNullException();
            }

            Result<JDocument> res = new Result<JDocument>();

            theDatabase.GetDocument(id, res).WhenDone(
                res.Return,
                res.Throw
                );
            return res;
        }

        Result<JObject> DeleteDocument(JDocument aDocument, Result<JObject> aResult)
        {

            if (aResult == null)
            {
                throw new ArgumentNullException();
            }

            Result<JObject> res = new Result<JObject>();

            theDatabase.DeleteDocument(aDocument,res)
                .WhenDone(
                res.Return,
                res.Throw
                );
            return res;
        }



        Result<ViewResult<string, string, JDocument>> getViewDocument(string aViewId, string aViewName, Result<ViewResult<string, string, JDocument>> aResult)
        {

            if (aResult == null)
            {
                throw new ArgumentNullException();
            }

            var aRes = new Result<ViewResult<string, string, JDocument>>();

            theDatabase.GetView(aViewId, aViewName, aRes).WhenDone(
                aRes.Return,
                aRes.Throw
                );
            return aRes;
        }

        Result<ViewResult<string[], string, JDocument>> getViewDocument(string aViewId, string aViewName, Result<ViewResult<string[], string, JDocument>> aResult)
        {

            if (aResult == null)
            {
                throw new ArgumentNullException();
            }

            var aRes = new Result<ViewResult<string[], string, JDocument>>();

            theDatabase.GetView(aViewId, aViewName, aRes).WhenDone(
                aRes.Return,
                aRes.Throw
                );
            return aRes;
        }
    }
}
