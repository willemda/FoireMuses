using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoireMuses.WebService
{
    using Yield = System.Collections.Generic.IEnumerator<MindTouch.Tasking.IYield>;
    using MindTouch.Dream;
    using MindTouch.Xml;
    using MindTouch.Tasking;
    using FoireMuses.Core.Business;
    using Newtonsoft.Json.Linq;
    using FoireMuses.Core;

    public partial class Services
    {
        [DreamFeature("POST:user","create a user")]
        public Yield CreateUser(DreamContext context, DreamMessage request, Result<DreamMessage> response)
        {
            JObject aObject = JObject.Parse(request.ToText());
            Result<JUser> res = new Result<JUser>();
            yield return Context.Current.Instance.UserController.Create(new JUser(aObject), res);
            if (!res.HasException)
                response.Return(DreamMessage.Ok(MimeType.JSON, res.Value.ToString()));
            else
                response.Return(DreamMessage.BadRequest("Todo"));

            yield break;
        }

        [DreamFeature("GET:user/{username}", "create a user")]
        public Yield GetUserByUsername(DreamContext context, DreamMessage request, Result<DreamMessage> response)
        {
            string theUsername = context.GetParam("username");
            Result<JUser> res = new Result<JUser>();
            yield return Context.Current.Instance.UserController.GetByUsername(theUsername, res);
            if (!res.HasException)
                response.Return(DreamMessage.Ok(MimeType.JSON, res.Value.ToString()));
            else
                response.Return(DreamMessage.BadRequest("Todo"));

            yield break;
        }
    }
}
