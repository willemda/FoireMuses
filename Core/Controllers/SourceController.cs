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

namespace FoireMuses.Core.Controllers
{
    class SourceController : BaseController<JSource>, ISourceController 
    {


        string VIEW_SOURCES = "sources";
        string VIEW_SOURCES_HEAD = "head";

        public override Result<JSource> GetById(string id, Result<JSource> aResult)
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
                    VIEW_SOURCES,
                    VIEW_SOURCES_HEAD,
                    voptions,
                    new Result<ViewResult<string, string, JSource>>()
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
