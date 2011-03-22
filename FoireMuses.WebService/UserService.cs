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
        [DreamFeature("POST:users","create a user")]
        public Yield CreateUser(DreamContext context, DreamMessage request, Result<DreamMessage> response)
        {
            JObject aObject = JObject.Parse(request.ToText());
            Result<JUser> res = new Result<JUser>();
            JUser user = new JUser();
           
            yield return Context.Current.Instance.UserController.Create(new JUser(aObject), res);
            if (!res.HasException)
                response.Return(DreamMessage.Ok(MimeType.JSON, res.Value.ToString()));
            else
                response.Return(DreamMessage.BadRequest("Todo"));

            yield break;
        }

        [DreamFeature("GET:users/username/{username}", "get the user that has the given username")]
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

        [DreamFeature("GET:users/{id}", "get the user that has the given id")]
        public Yield GetUserById(DreamContext context, DreamMessage request, Result<DreamMessage> response)
        {
            string theId = context.GetParam("id");
            Result<JUser> res = new Result<JUser>();
            yield return Context.Current.Instance.UserController.GetById(theId, res);
            if (!res.HasException)
                response.Return(DreamMessage.Ok(MimeType.JSON, res.Value.ToString()));
            else
                response.Return(DreamMessage.BadRequest("Todo"));

            yield break;
        }

        [DreamFeature("PUT:users", "update a user")]
        public Yield UpdateUser(DreamContext context, DreamMessage request, Result<DreamMessage> response)
        {
            JObject aObject = JObject.Parse(request.ToText());
            Result<JUser> res = new Result<JUser>();
            yield return Context.Current.Instance.UserController.Update(new JUser(aObject), res);
            if (!res.HasException)
                response.Return(DreamMessage.Ok(MimeType.JSON, res.Value.ToString()));
            else
                response.Return(DreamMessage.BadRequest("Todo"));

            yield break;
        }
    }
}
