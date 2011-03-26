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
		[DreamFeature("GET:users","get the users")]
		[DreamFeatureParam("max","int?","limit the result to max rows")]
		[DreamFeatureParam("offset", "int?", "skip the offset first results")]
		public Yield GetUsers(DreamContext context, DreamMessage request, Result<DreamMessage> response){
			Result<SearchResult<IUser>> result = new Result<SearchResult<IUser>>();
			int limit = context.GetParam("limit", 100);
			int offset = context.GetParam("offset", 0);

			yield return Context.Current.Instance.UserController.GetAll(offset, limit, result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Factory.ResultToJson(result.Value)));
		}

		[DreamFeature("POST:users", "create a user")]
		public Yield CreateUser(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			Result<IUser> result = new Result<IUser>();
			yield return Context.Current.Instance.UserController.Create(Factory.IUserFromJson(request.ToText()), result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Factory.ResultToJson(result.Value)));
		}

		[DreamFeature("GET:users/username/{username}", "get the user that has the given username")]
		public Yield GetUserByUsername(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			string username = context.GetParam("username");
			Result<IUser> result = new Result<IUser>();
			yield return Context.Current.Instance.UserController.GetByUsername(username, result);

			response.Return(result.Value == null
								? DreamMessage.NotFound("No User found for username " + username)
								: DreamMessage.Ok(MimeType.JSON, Factory.ResultToJson(result.Value)));
		}

		[DreamFeature("GET:users/{id}", "get the user that has the given id")]
		public Yield GetUserById(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			string id = context.GetParam("id");
			Result<IUser> result = new Result<IUser>();
			yield return Context.Current.Instance.UserController.Get(id, result);

			response.Return(result.Value == null
								? DreamMessage.NotFound("No User found for id " + id)
								: DreamMessage.Ok(MimeType.JSON, Factory.ResultToJson(result.Value)));
		}

		[DreamFeature("PUT:users", "update a user")]
		public Yield UpdateUser(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{

			Result<IUser> result = new Result<IUser>();

			yield return Context.Current.Instance.UserController.Update(Factory.IUserFromJson(request.ToText()), result);

			response.Return(DreamMessage.Ok(MimeType.JSON, Factory.ResultToJson(result.Value)));
		}
	}
}
