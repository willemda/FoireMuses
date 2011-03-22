using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core.Business;
using FoireMuses.Core.Utils;
using LoveSeat;
using MindTouch.Tasking;
using LoveSeat.Support;
using Newtonsoft.Json.Linq;

namespace FoireMuses.Core.Controllers
{
    class PlayController : BaseController<JPlay>, IPlayController
    {

        public Result<JPlay> Create(JPlay aDoc, Result<JPlay> aResult)
        {
            base.Create(aDoc, new Result<JPlay>()).WhenDone(
                aResult.Return,
                aResult.Throw
                );
            return aResult;
        }

        public Result<JPlay> GetById(string id, Result<JPlay> aResult)
        {
            base.GetById(id, new Result<JPlay>()).WhenDone(
                aResult.Return,
                aResult.Throw
                );
            return aResult;
        }

        public Result<JPlay> Get(JPlay aDoc, Result<JPlay> aResult)
        {
            base.Get(aDoc, new Result<JPlay>()).WhenDone(
                aResult.Return,
                aResult.Throw
                );
            return aResult;
        }

        public Result<JPlay> Update(JPlay aDoc, Result<JPlay> aResult)
        {
            base.Update(aDoc, new Result<JPlay>()).WhenDone(
                aResult.Return,
                aResult.Throw
                );
            return aResult;
        }

        public Result<JObject> Delete(JPlay aDoc, Result<JObject> aResult)
        {
            base.Delete(aDoc, new Result<JObject>()).WhenDone(
                aResult.Return,
                aResult.Throw
                );
            return aResult;
        }
    }
}
