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

namespace FoireMuses.Core.Controllers
{
    class PlayController : BaseController<JPlay>, IPlayController
    {

        string VIEW_PLAY = "plays";
        string VIEW_PLAY_HEAD = "head";

        public override Result<JPlay> GetById(string id, Result<JPlay> aResult)
        {
            try
            {
                ArgCheck.NotNull("aResult", aResult);
                ArgCheck.NotNullNorEmpty("id", id);

                ViewOptions voptions = new ViewOptions();
                KeyOptions koptions = new KeyOptions();
                koptions.Add(id);
                voptions.Key = koptions;
                voptions.Limit = 1;

                Context.Current.Instance.CouchDbController.CouchDatabase.GetView(
                    VIEW_PLAY,
                    VIEW_PLAY_HEAD,
                    voptions,
                    new Result<ViewResult<string, string, JPlay>>()
                ).WhenDone(
                    a => aResult.Return(a.Rows.GetEnumerator().Current.Doc),
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
    }
}
