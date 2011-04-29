﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Dream;
using MindTouch.Xml;
using FoireMuses.Core;
using MindTouch.Tasking;
using Autofac;

namespace FoireMuses.WebService
{

	using Yield = System.Collections.Generic.IEnumerator<MindTouch.Tasking.IYield>;
	using FoireMuses.Core.Interfaces;
	using MindTouch.Web;

	[DreamService("Foire Muses", "Foire Muses",
		Info = "foire muses",
		SID = new string[] { "http://foiremuses.org/service" }
	)]
	public partial class Services : DreamService
	{

		private InstanceFactory theFactory;

		private static readonly log4net.ILog theLogger = log4net.LogManager.GetLogger(typeof(Services));

		[DreamFeature("GET:info", "Information about the service")]
		public Yield GetInfo(DreamContext context, DreamMessage request, Result<DreamMessage> response)
		{
			XDoc xmlInfo = new XDoc("info");
			xmlInfo.Elem("User", Context.Current.User);
			xmlInfo.Elem("About", "Foire muses web service @ 2011");
			response.Return(DreamMessage.Ok(MimeType.XML, xmlInfo));
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


		protected override Yield Start(XDoc config, IContainer container, Result result)
		{
			Result res;
			yield return res = Coroutine.Invoke(base.Start, config, new Result());
			res.Confirm();



			theFactory = new InstanceFactory(container, config);
			result.Return();
		}

		private Yield SetContext(DreamContext context, DreamMessage request, Result<DreamMessage> response)	
		{
			Context ctx = new Context(theFactory.GetInstance(context, request));

			//todo: fix this, find real auth
			Result<IUser> result = new Result<IUser>();
			string username, password;
			if (!HttpUtil.GetAuthentication(context.Uri, request.Headers, out username, out password))
			{
				response.Return(DreamMessage.AccessDenied("Foire muses","No auth"));
				yield break;
			}
			yield return ctx.Instance.UserController.Login(username, password, result);
			ctx.User = result.Value;
			ctx.AttachToCurrentTaskEnv();
			//create context and attach
			response.Return(request);
			yield break;
			//verification if the user if authenticated or not..
		}
	}
}
