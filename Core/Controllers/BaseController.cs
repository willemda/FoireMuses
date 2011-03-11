using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core.Utils;
using MindTouch.Tasking;
using LoveSeat;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Business;

namespace FoireMuses.Core.Controllers
{
    public class BaseController<T> : IBaseController<T> where T : Document
    {

        public Result<T> Create(T aDoc, Result<T> aResult)
        {
            try
            {
                ArgCheck.NotNull("aDoc", aDoc);
                ArgCheck.NotNull("aResult", aResult);
                aDoc.Validate();

                Context.Current.Instance.CouchDbController.CouchDatabase.CreateDocument(aDoc, new Result<T>()).WhenDone(
                     aResult.Return,
                     aResult.Throw
                     );
            }
            catch (Exception e)
            {
                aResult.Throw(e);
            }
            return aResult;
        }

        public Result<T> Update(T aDoc, Result<T> aResult)
        {
            try
            {
                ArgCheck.NotNull("aResult", aResult);
                ArgCheck.NotNull("aDoc", aDoc);
                aDoc.Validate();

                Context.Current.Instance.CouchDbController.CouchDatabase.UpdateDocument(aDoc, new Result<T>()).WhenDone(
                    aResult.Return,
                    aResult.Throw
                    );
            }
            catch (Exception e)
            {
                aResult.Throw(e);
            }
            return aResult;
        }

        public Result<T> Get(string id, Result<T> aResult)
        {
            try
            {
                ArgCheck.NotNull("aResult", aResult);
                ArgCheck.NotNullNorEmpty("id", id);

                Context.Current.Instance.CouchDbController.CouchDatabase.GetDocument(id, new Result<T>()).WhenDone(
                    aResult.Return,
                    aResult.Throw
                    );
            }
            catch (Exception e)
            {
                aResult.Throw(e);
            }

            return aResult;
        }

        public Result<T> Get(T aDoc, Result<T> aResult)
        {
            try
            {
                ArgCheck.NotNull("aResult", aResult);
                ArgCheck.NotNull("aDoc", aDoc);

                Context.Current.Instance.CouchDbController.CouchDatabase.GetDocument(aDoc.Id, new Result<T>()).WhenDone(
                    aResult.Return,
                    aResult.Throw
                    );
            }
            catch (Exception e)
            {
                aResult.Throw(e);
            }

            return aResult;
        }

        public Result<JObject> Delete(T aDoc, Result<JObject> aResult)
        {
            try
            {
                ArgCheck.NotNull("aResult", aResult);
                ArgCheck.NotNull("aDoc", aDoc);

                Context.Current.Instance.CouchDbController.CouchDatabase.DeleteDocument(aDoc, new Result<JObject>())
                    .WhenDone(
                    aResult.Return,
                    aResult.Throw
                    );
            }
            catch (Exception e)
            {
                aResult.Throw(e);
            }
            return aResult;
        }



        string VIEW_ALL_ID = "alldoc";
        string VIEW_ALL_NAME = "all";

        public Result<ViewResult<string,string,T>> GetAll(Result<ViewResult<string,string,T>> aResult){
            try
            {
                ArgCheck.NotNull("aResult", aResult);

                Context.Current.Instance.CouchDbController.CouchDatabase.GetView
                (
                    VIEW_ALL_ID,
                    VIEW_ALL_NAME,
                    new Result<ViewResult<string, string, T>>()
                ).WhenDone(
                        aResult.Return,
                        aResult.Throw
                    );
            }
            catch (Exception e)
            {
                aResult.Throw(e);
            }
			return aResult;
        }

    }
}
