using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Business;
using FoireMuses.Core.Interfaces;
using MindTouch.Tasking;
using FoireMuses.Core.Utils;
using LoveSeat;
using LoveSeat.Support;
using Newtonsoft.Json.Linq;

namespace FoireMuses.Core.Controllers
{
    public class UserController : BaseController<JUser>, IUserController
    {

        public Result<JUser> GetByUsername(string username, Result<JUser> aResult)
        {
            try
            {
                ArgCheck.NotNull("aResult", aResult);
                ArgCheck.NotNull("username", username);

                ViewOptions voptions = new ViewOptions();
                KeyOptions koptions = new KeyOptions();
                koptions.Add(username);
                voptions.Key = koptions;

                Context.Current.Instance.CouchDbController.CouchDatabase.GetView
                (
                    CouchViews.VIEW_USERS,
                    CouchViews.VIEW_USERS_BY_USERNAME,
                    voptions,
                    new Result<ViewResult<string, string, JUser>>()
                ).WhenDone(
                a => { aResult.Return(a.Rows.First().Doc); }, // TODO MOCHE CHANGER CA
                        aResult.Throw
                    );
            }
            catch (Exception e)
            {
                aResult.Throw(e);
            }
            return aResult;
        }

        public override Result<JUser> Create(JUser aDoc, Result<JUser> aResult)
        {
            base.Create(aDoc, new Result<JUser>()).WhenDone(
                aResult.Return,
                aResult.Throw
                );
            return aResult;
        }

        public override Result<JUser> GetById(string id, Result<JUser> aResult)
        {
            base.GetById(id, new Result<JUser>()).WhenDone(
                aResult.Return,
                aResult.Throw
                );
            return aResult;
        }

        public override Result<JUser> Get(JUser aDoc, Result<JUser> aResult)
        {
            base.Get(aDoc, new Result<JUser>()).WhenDone(
                aResult.Return,
                aResult.Throw
                );
            return aResult;
        }

        public override Result<JUser> Update(JUser aDoc, Result<JUser> aResult)
        {
            base.Update(aDoc, new Result<JUser>()).WhenDone(
                aResult.Return,
                aResult.Throw
                );
            return aResult;
        }

        public override Result<JObject> Delete(JUser aDoc, Result<JObject> aResult)
        {
            base.Delete(aDoc, new Result<JObject>()).WhenDone(
                aResult.Return,
                aResult.Throw
                );
            return aResult;
        }
    }
}
