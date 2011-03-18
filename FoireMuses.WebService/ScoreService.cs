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
        Info = "http://localhost:8081/host/",
        SID = new string[] { "http://localhost:8081/host/" }
    )]
    public class ScoreService : DreamService
    {
        private Context _Context;   

        protected override Yield Start(XDoc config, Result result) {
            Result res;

            yield return res = Coroutine.Invoke(base.Start, config, new Result());

            res.Confirm();

            if(_Context == null){
                _Context = new Context(new Instance());
                _Context.User = "Admin";
            }
            result.Return();
        }


        [DreamFeature("GET:scores", "Get all scores")]
        public Yield GetScores(DreamContext context, DreamMessage request, Result<DreamMessage> response)
        {
            _Context.AttachToCurrentTaskEnv();
            _Context.Instance.ScoreController.GetHead(new Result<ViewResult<string,string>>()).WhenDone(
                a=>ResultToXml(response,a),
                    e=>response.Return(DreamMessage.InternalError())
                );
           response.Return(DreamMessage.Ok());
           yield break;
        }

        private void ResultToXml(Result<DreamMessage> response, ViewResult<string, string> result)
        {
            XDoc xdoc = new XDoc("scores");
            foreach (ViewResultRow<string,string,JScore> row in result.Rows)
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
