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
    using FoireMuses.Core;
    using MindTouch.Xml;

    public partial class ScoreService
    {

        [DreamFeature("GET:scores", "Get all scores")]
        [DreamFeatureParam("limit","int?","the number of document given by the output")]
        public Yield GetScores(DreamContext context, DreamMessage request, Result<DreamMessage> response)
        {   
            Result<ViewResult<string, string>> res = new Result<ViewResult<string, string>>();
            int limit = context.GetParam<int>("limit", 0);
            yield return Context.Current.Instance.ScoreController.GetHead(limit, res);
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
