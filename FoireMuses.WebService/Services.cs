using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Dream;

namespace FoireMuses.WebService
{


    using Yield = System.Collections.Generic.IEnumerator<MindTouch.Tasking.IYield>;
    using MindTouch.Xml;
    using FoireMuses.Core;
    using FoireMuses.Core.Business;
    using MindTouch.Tasking;

    [DreamService("Foire Muses", "Foire Muses",
        Info = "foire muses",
        SID = new string[] { "http://foiremuses.org/service" }
    )]
    public partial class Services : DreamService
    {

        private static Instance theInstance = new Instance();

        private static readonly log4net.ILog theLogger = log4net.LogManager.GetLogger(typeof(ScoreService));

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


        private Yield SetContext(DreamContext context, DreamMessage request, Result<DreamMessage> response)
        {
            Context ctx = new Context(theInstance);
            ctx.User = context.User;
            //create context and attach
            ctx.AttachToCurrentTaskEnv();
            response.Return(request);
            yield break;
            //verification if the user if authenticated or not..
        }
    }
}
