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

namespace FoireMuses.Core.Controllers
{
    public class UserController : BaseController<JUser>, IUserController
    {

        string VIEW_USERS = "users";
        string VIEW_USERS_BY_USERNAME = "byusername";

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
                    VIEW_USERS,
                    VIEW_USERS_BY_USERNAME,
                    voptions,
                    new Result<ViewResult<string, string, JUser>>()
                ).WhenDone(
                        a => { aResult.Return(a.Rows.First().Doc); },
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
