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
using FoireMuses.Core.Exceptions;

namespace FoireMuses.Core.Controllers
{
    /// <summary>
    /// Generic class for CRUD operations on Business objects
    /// Create and update method calls beforeCreate, afterCreate, beforeUpdate, afterUpdate on business object (validation is easyer this way)
    /// </summary>
    /// <typeparam name="T">T param is business object class that is or inherits Document</typeparam>
    public class BaseController<T> : IBaseController<T> where T : Document
    {
        public Result CheckAuthorization(T aDoc, Result aResult)
        {
            if (Context.Current.User==null||(!IsCreator(aDoc) && !IsCollaborator(aDoc)))
                throw new UnauthorizedAccessException();
            return aResult;
        }

        private bool IsCreator(T aDoc)
        {
            JUser current = Context.Current.User;
            Result<T> res = new Result<T>();
            this.Get(aDoc, res).Wait();
            if (!res.HasException && res.Value != null)
            {
                JToken creatorId;
                if (res.Value.TryGetValue("creatorId", out creatorId) )
                {
                    if (creatorId.Value<string>() == current.Id)
                        return true;
                    else
                        return false;
                }
                throw new InternalError();
            }
            throw new NoResultException();
        }

        private bool IsCollaborator(T aDoc)
        {
            JUser current = Context.Current.User;
            JToken groupsId;
            JArray groups = new JArray();
            if (current.TryGetValue("groupsId", out groupsId)) // get the groups of the current user
            {
                groups = groupsId.Value<JArray>();
            }
            Result<T> res = new Result<T>();
            this.Get(aDoc, res).Wait();
            if (!res.HasException && res.Value != null)
            {
                  JToken collaboratorsId;
                  //get the id of the collaborators of the doc, either groups or users
                  if (res.Value.TryGetValue("collaboratorsId", out collaboratorsId)) 
                  {
                      bool IsTrue = false;
                      foreach (string collabId in collaboratorsId.Values<string>())
                      {
                          // if we are directly collab or part of a collab group
                          if (collabId == current.Id || groups.Contains(collabId)) 
                          {
                              IsTrue = true;
                              break;
                          }
                      }
                    return IsTrue;
                  }
                  return false;
            }
            throw new NoResultException();
        }



        public virtual Result<T> Create(T aDoc, Result<T> aResult)
        {
            try
            {
                ArgCheck.NotNull("aDoc", aDoc);
                ArgCheck.NotNull("aResult", aResult);

                Context.Current.Instance.CouchDbController.CouchDatabase.CreateDocument<T>(aDoc, new Result<T>()).WhenDone(
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

        public virtual Result<T> Update(T aDoc, Result<T> aResult)
        {
            try
            {
                ArgCheck.NotNull("aResult", aResult);
                ArgCheck.NotNull("aDoc", aDoc);

                Context.Current.Instance.CouchDbController.CouchDatabase.UpdateDocument<T>(aDoc, new Result<T>()).WhenDone(
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

       // public abstract Result<T> GetById(string id, Result<T> aResult);


        public virtual Result<T> Get(T aDoc, Result<T> aResult)
        {
            try
            {
                ArgCheck.NotNull("aResult", aResult);
                ArgCheck.NotNull("aDoc", aDoc);

                Context.Current.Instance.CouchDbController.CouchDatabase.GetDocument<T>(aDoc.Id, new Result<T>()).WhenDone(
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

        public virtual Result<T> GetById(string id, Result<T> aResult)
        {
            try
            {
                ArgCheck.NotNull("aResult", aResult);
                ArgCheck.NotNull("id", id);

                Context.Current.Instance.CouchDbController.CouchDatabase.GetDocument<T>(id, new Result<T>()).WhenDone(
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

        public virtual Result<JObject> Delete(T aDoc, Result<JObject> aResult)
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



    }
}
