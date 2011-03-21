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

    public partial class Services
    {
        /*[DreamFeature("POST:auth","create a user")]
        public Yield CreateUser(DreamContext context, DreamMessage request, Result<DreamMessage> response)
        {
            string jUser = request.ToText();
            JObject aObject = JObject.Parse(request.ToText());
            
        }*/
    }
}
