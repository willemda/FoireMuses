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
		[DreamFeature("POST:users", "create a user")]
		public Yield CreateUser(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			JObject aObject = JObject.Parse(request.ToText());
			Result<JUser> res = new Result<JUser>();
			JUser user = new JUser();

			yield return Context.Current.Instance.UserController.Create(new JUser(aObject), res).Catch();

			response.Return(DreamMessage.Ok(MimeType.JSON, res.Value.ToString()));
		}

		[DreamFeature("GET:users/username/{username}", "get the user that has the given username")]
		public Yield GetUserByUsername(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			string username = context.GetParam("username");
			Result<JUser> result = new Result<JUser>();
			yield return Context.Current.Instance.UserController.GetByUsername(username, result);

			response.Return(result.Value == null
								? DreamMessage.NotFound("No User found for username " + username)
								: DreamMessage.Ok(MimeType.JSON, result.Value.ToString()));
		}

		[DreamFeature("GET:users/{id}", "get the user that has the given id")]
		public Yield GetUserById(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			string id = context.GetParam("id");
			Result<JUser> result = new Result<JUser>();
			yield return Context.Current.Instance.UserController.GetById(id, result);

			response.Return(result.Value == null
								? DreamMessage.NotFound("No User found for id " + id)
								: DreamMessage.Ok(MimeType.JSON, result.Value.ToString()));
		}

		[DreamFeature("PUT:users", "update a user")]
		public Yield UpdateUser(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			JObject aObject = JObject.Parse(request.ToText());
			Result<JUser> res = new Result<JUser>();
			yield return Context.Current.Instance.UserController.Update(new JUser(aObject), res);

			response.Return(DreamMessage.Ok(MimeType.JSON, res.Value.ToString()));
		}
	}
}
