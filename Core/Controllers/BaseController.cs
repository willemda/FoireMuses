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
    /// <summary>
    /// Generic class for CRUD operations on Business objects
    /// Create and update method calls beforeCreate, afterCreate, beforeUpdate, afterUpdate on business object (validation is easyer this way)
    /// </summary>
    /// <typeparam name="T">T param is business object class that is or inherits Document</typeparam>
    public abstract class BaseController<T> : IBaseController<T> where T : Document
    {

        //eventually override these methods if they are needed in the subclasses
        //exemple: in playController, after each creation, I want to credit the user who created the play 100 pts
        //called whenever a document is created
        public void Created() { }
        //called whenever a document is updated
        public void Updated() { }
        //called whenever a document is deleted
        public void Deleted() { }
        //called whenever a document is read/get
        public void Readed() { }

        public Result<T> Create(T aDoc, Result<T> aResult)
        {
            try
            {
                ArgCheck.NotNull("aDoc", aDoc);
                ArgCheck.NotNull("aResult", aResult);
                aDoc.BeforeCreate();

                Context.Current.Instance.CouchDbController.CouchDatabase.CreateDocument(aDoc, new Result<T>()).WhenDone(
                     aResult.Return,
                     aResult.Throw
                     );
                aDoc.AfterCreate();
                this.Created();
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
                aDoc.BeforeUpdate();

                Context.Current.Instance.CouchDbController.CouchDatabase.UpdateDocument(aDoc, new Result<T>()).WhenDone(
                    aResult.Return,
                    aResult.Throw
                    );
                aDoc.AfterUpdate();
                this.Updated();
            }
            catch (Exception e)
            {
                aResult.Throw(e);
            }
            return aResult;
        }

        public abstract Result<T> GetById(string id, Result<T> aResult);


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
                this.Readed();
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
                this.Deleted();
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
