using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Dream;
using MindTouch.Xml;
using MindTouch.Tasking;
using Newtonsoft.Json.Linq;
using FoireMuses.Core;
using FoireMuses.Core.Interfaces;

namespace FoireMuses.WebService
{
	using Yield = System.Collections.Generic.IEnumerator<MindTouch.Tasking.IYield>;


	public partial class Services
	{
		[DreamFeature("GET:users", "get the users")]
		[DreamFeatureParam("max", "int?", "limit the result to max rows")]
		[DreamFeatureParam("offset", "int?", "skip the offset first results")]
		public Yield GetUsers(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			Result<SearchResult<IUser>> result = new Result<SearchResult<IUser>>();
			int limit = context.GetParam("max", 20);
			int offset = context.GetParam("offset", 0);

			yield return Context.Current.Instance.UserController.GetAll(offset, limit, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.UserController.ToJson(result.Value)));
		}

		[DreamFeature("POST:users", "create a user")]
		public Yield CreateUser(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			Result<IUser> result = new Result<IUser>();

			IUser user = Context.Current.Instance.UserController.FromJson(request.ToText());

			yield return Context.Current.Instance.UserController.Insert(user, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.UserController.ToJson(result.Value)));
		}

		[DreamFeature("GET:users/{id}", "get the user that has the given id")]
		public Yield GetUser(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			string id = context.GetParam("id");
			Result<IUser> result = new Result<IUser>();
			yield return Context.Current.Instance.UserController.Retrieve(id, result);

			response.Return(result.Value == null
								? DreamMessage.NotFound("No User found for id " + id)
								: DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.UserController.ToJson((result.Value))));
		}

		[DreamFeature("PUT:users/{id}", "update a user")]
		[DreamFeatureParam("{id}", "String", "User id")]
		[DreamFeatureParam("{rev}", "String", "User revision id")]
		public Yield UpdateUser(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{

			Result<IUser> result = new Result<IUser>();
			yield return Context.Current.Instance.UserController.Update(context.GetParam("id"), context.GetParam("rev"), Context.Current.Instance.UserController.FromJson(request.ToText()), result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Context.Current.Instance.UserController.ToJson(result.Value)));
		}

		[DreamFeature("DELETE:users/{id}", "Delete a source")]
		[DreamFeatureParam("{id}", "String", "source id")]
		[DreamFeatureParam("{rev}", "String", "source revision id")]
		public Yield DeleteUser(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			Result<bool> result = new Result<bool>();
			yield return Context.Current.Instance.UserController.Delete(context.GetParam("id"), context.GetParam("rev"), result);

			response.Return(DreamMessage.Ok(MimeType.JSON, result.Value.ToString()));
		}
	}
}