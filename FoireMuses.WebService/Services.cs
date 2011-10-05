using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Dream;
using MindTouch.Xml;
using FoireMuses.Core;
using MindTouch.Tasking;
using Autofac;
using FoireMuses.Core.Interfaces;
using MindTouch.Web;

namespace FoireMuses.WebService
{
	using Yield = IEnumerator<IYield>;

	[DreamService("Foiremuses", "Foiremuses",
		Info = "Foiremuses service",
		SID = new string[] { "http://foiremuses.org/service" }
	)]
	public partial class Services : DreamService
	{
		private static readonly log4net.ILog theLogger = log4net.LogManager.GetLogger(typeof(Services));

		private InstanceFactory theFactory;
		private  const string FOIREMUSES_IMPERSONATE_HEADER = "FoireMusesImpersonate";

		[DreamFeature("GET:info", "Information about the service")]
		public Yield GetInfo(DreamContext aContext, DreamMessage aRequest, Result<DreamMessage> aResponse)
		{
			XDoc xmlInfo = new XDoc("info");
			xmlInfo.Elem("User", Context.Current.User);
			xmlInfo.Elem("About", "Foiremuses web service (c) 2011 Vincent DARON / Danny WILLEM");
			aResponse.Return(DreamMessage.Ok(MimeType.XML, xmlInfo));
			yield break;
		}

		public override DreamFeatureStage[] Prologues
		{
			get
			{
				return new DreamFeatureStage[] { 
					new DreamFeatureStage("set-context", this.SetContext, DreamAccess.Public)
				};
			}
		}

		protected override Yield Start(XDoc aConfig, IContainer aContainer, Result aResult)
		{
			Result res;
			yield return res = Coroutine.Invoke(base.Start, aConfig, new Result());
			res.Confirm();

			theFactory = new InstanceFactory(aContainer, aConfig);
			aResult.Return();
		}

		private Yield SetContext(DreamContext aContext, DreamMessage aRequest, Result<DreamMessage> aResponse)
		{
			Context ctx = new Context(theFactory.GetInstance(aContext, aRequest));

			//TODO: fix this, find real auth
			string username, password;
			if (!HttpUtil.GetAuthentication(aContext.Uri, aRequest.Headers, out username, out password))
			{
				aResponse.Return(DreamMessage.AccessDenied("foiremuses api", "no auth"));
				yield break;
			}

			Result<IUser> user;
			yield return user = ctx.Instance.UserController.Retrieve(username, new Result<IUser>());
			if (user.Value == null)
			{
				aResponse.Return(DreamMessage.AccessDenied("foiremuses api", "no auth"));
				yield break;
			}
			if (user.Value.IsAdmin)
			{
				if (aRequest.Headers[FOIREMUSES_IMPERSONATE_HEADER] != null)
				{
					//act like real user
					ctx.User = ctx.Instance.UserController.Retrieve(aRequest.Headers[FOIREMUSES_IMPERSONATE_HEADER], new Result<IUser>()).Wait();
				}
				else
				{
					ctx.User = user.Value;
				}
			}
			else
			{
				ctx.User = user.Value;
			}
			ctx.AttachToCurrentTaskEnv();
			//create context and attach
			aResponse.Return(aRequest);
			yield break;
		}
	}
}
