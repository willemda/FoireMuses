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
    using Newtonsoft.Json.Linq;
    using FoireMuses.Core.Business;

    public partial class Services
    {

        [DreamFeature("GET:scores", "Get all scores")]
        [DreamFeatureParam("limit","int?","the number of document given by the output")]
        public Yield GetScores(DreamContext context, DreamMessage request, Result<DreamMessage> response)
        {   
            Result<ViewResult<string, string>> res = new Result<ViewResult<string, string>>();
            int limit = context.GetParam<int>("limit", 0);
            yield return Context.Current.Instance.ScoreController.GetHead(limit, res);


            if (!res.HasException)
            {
                string json = ResultToJson(res.Value);
                response.Return(DreamMessage.Ok(MimeType.JSON, json));
            }
            else
                response.Return(DreamMessage.BadRequest("Todo"));
            
        }

        [DreamFeature("GET:scores/{id}", "Get the score given by the id number")]
        public Yield GetScoreById(DreamContext context, DreamMessage request, Result<DreamMessage> response)
        {
            Result<JScore> res = new Result<JScore>();
            yield return Context.Current.Instance.ScoreController.GetById(context.GetParam("id"),res).Catch();

            if (!res.HasException)
                response.Return(DreamMessage.Ok(MimeType.JSON, res.Value.ToString()));
            else
                response.Return(DreamMessage.BadRequest("Impossible d'effectuer la requête."));
            yield break;
        }



        [DreamFeature("POST:scores", "Create new score")]
        public Yield CreateScore(DreamContext context, DreamMessage request, Result<DreamMessage> response)
        {
            JObject aObject = JObject.Parse(request.ToText());
            Result<JScore> res = new Result<JScore>();
            yield return Context.Current.Instance.ScoreController.Create(new JScore(aObject), res);
            if (!res.HasException)
                response.Return(DreamMessage.Ok(MimeType.JSON, res.Value.ToString()));
            else
                response.Return(DreamMessage.BadRequest("Todo"));

            yield break;
        }

        [DreamFeature("PUT:scores", "Update the score")]
        public Yield UpdateScore(DreamContext context, DreamMessage request, Result<DreamMessage> response)
        {
            JObject aObject = JObject.Parse(request.ToText());
            Result<JScore> res = new Result<JScore>();
            yield return Context.Current.Instance.ScoreController.Update(new JScore(aObject), res);
            if (!res.HasException)
                response.Return(DreamMessage.Ok(MimeType.JSON, res.Value.ToString()));
            else
                response.Return(DreamMessage.BadRequest("Todo"));

            yield break;
        }


        // TODO: delete methods not allowed!
        public Yield DeleteScore(DreamContext context, DreamMessage request, Result<DreamMessage> response)
        {
            JObject aObject = JObject.Parse(request.ToText());
            Result<JObject> res = new Result<JObject>();
            yield return Context.Current.Instance.ScoreController.Delete(new JScore(aObject), res);
            if (!res.HasException)
                response.Return(DreamMessage.Ok(MimeType.JSON, res.Value.ToString()));
            else
                response.Return(DreamMessage.BadRequest("Todo"));

            yield break;
        }

        private XDoc ResultToXml(ViewResult<string, string> result)
        {
            XDoc xdoc = new XDoc("scores");
            foreach (ViewResultRow<string,string> row in result.Rows)
            {
                xdoc.Start("score");
                xdoc.Attr("id", row.Key);
                xdoc.Attr("title", row.Value);
                xdoc.End();
            }
            return xdoc;
        }

        private string ResultToJson(ViewResult<string, string> result)
        {
            string json = "";
            foreach (ViewResultRow<string,string> r in result.Rows)
            {
                json += "Id: " + r.Id + "\tKey: " + r.Key + "\n";
            }
            return json;
        }


    }

   
}
