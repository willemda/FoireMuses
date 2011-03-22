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

        public override void Readed(JPlay play, Result<JPlay> res)
        {
            JToken type;
            play.TryGetValue("otype", out type);
            if (type.Value<string>() == "play")
                res.Return(play);
            else
                res.Throw(new Exception("Bad type"));
        }
    }
}
