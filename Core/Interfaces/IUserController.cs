using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Business;
using MindTouch.Tasking;

namespace FoireMuses.Core.Interfaces
{
    public interface IUserController: IBaseController<JUser>
    {

        Result<JUser> GetByUsername(string username, Result<JUser> aResult);
    }
}
