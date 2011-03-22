using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core.Business;
using LoveSeat;
using LoveSeat.Support;
using FoireMuses.Core.Utils;
using MindTouch.Tasking;
using Newtonsoft.Json.Linq;

namespace FoireMuses.Core.Controllers
{
    class SourceController : BaseController<JSource>, ISourceController
    {

        new public Result<JSource> Create(JSource aDoc, Result<JSource> aResult)
        {
            base.Create(aDoc, new Result<JSource>()).WhenDone(
                aResult.Return,
                aResult.Throw
                );
            return aResult;
        }

        new public Result<JSource> GetById(string id, Result<JSource> aResult)
        {
            base.GetById(id, new Result<JSource>()).WhenDone(
                aResult.Return,
                aResult.Throw
                );
            return aResult;
        }

        new public Result<JSource> Get(JSource aDoc, Result<JSource> aResult)
        {
            base.Get(aDoc, new Result<JSource>()).WhenDone(
                aResult.Return,
                aResult.Throw
                );
            return aResult;
        }

        new public Result<JSource> Update(JSource aDoc, Result<JSource> aResult)
        {
            base.Update(aDoc, new Result<JSource>()).WhenDone(
                aResult.Return,
                aResult.Throw
                );
            return aResult;
        }

        new public Result<JObject> Delete(JSource aDoc, Result<JObject> aResult)
        {
            base.Delete(aDoc, new Result<JObject>()).WhenDone(
                aResult.Return,
                aResult.Throw
                );
            return aResult;
        }


    }
}
