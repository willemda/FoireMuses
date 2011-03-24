﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Business;
using MindTouch.Tasking;

namespace FoireMuses.Core.Interfaces
{
    public interface IUserController : IBaseController<IUser>
    {
        Result<IUser> GetByUsername(string username, Result<IUser> aResult);
    }
}
