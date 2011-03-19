using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveSeat;
using MindTouch.Tasking;
using MindTouch.Dream;
using FoireMuses.Core.Controllers;
using FoireMuses.Core.Interfaces;


namespace FoireMuses.WebService
{
    using Yield = System.Collections.Generic.IEnumerator<IYield>;
    using MindTouch.Xml;
    using FoireMuses.Core;
    using FoireMuses.Core.Business;




    [DreamService("Foire Muses Web service", "Willem Danny -> Under GPL3 LICENCE",
        Info = "foire muses",
        SID = new string[] { "http://foiremuses.org/service" }
    )]
    public class ScoreService : DreamService
    {

        private static readonly log4net.ILog theLogger = log4net.LogManager.GetLogger(typeof(ScoreService));

        private Context _Context;   

        protected override Yield Start(XDoc config, Result result) {
            Result res;

            yield return res = Coroutine.Invoke(base.Start, config, new Result());

            res.Confirm();

            if(_Context == null){
                _Context = new Context(new Instance());
                _Context.User = null;
            }
            result.Return();
        }


        [DreamFeature("GET:scores", "Get all scores")]
        [DreamFeatureParam("limit","int?","the number of document given by the output")]
        public Yield GetScores(DreamContext context, DreamMessage request, Result<DreamMessage> response)
        {
            Context ctx = _Context.Clone() as Context;
            ctx.AttachToCurrentTaskEnv();
            Result<ViewResult<string, string>> res = new Result<ViewResult<string, string>>();
            int limit = context.GetParam<int>("limit", 0);
            yield return ctx.Instance.ScoreController.GetHead(limit, res);
            //theLogger.Debug("Hello");
            ResultToXml(response, res.Value);
            
        }

        private void ResultToXml(Result<DreamMessage> response, ViewResult<string, string> result)
        {
            //theLogger.Debug("ToXML");
            XDoc xdoc = new XDoc("scores");
            foreach (ViewResultRow<string,string> row in result.Rows)
            {
                xdoc.Start("score");
                xdoc.Attr("id", row.Key);
                xdoc.Attr("title", row.Value);
                xdoc.End();
            }
            response.Return(DreamMessage.Ok(xdoc));
        }


    }

   
}
