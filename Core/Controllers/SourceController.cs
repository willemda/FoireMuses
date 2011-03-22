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

        public override void Readed(JSource source, Result<JSource> res)
        {
            JToken type;
            source.TryGetValue("otype", out type);
            if (type.Value<string>() == "source")
                res.Return(source);
            else
                res.Throw(new Exception("Bad type"));
        }


    }
}
