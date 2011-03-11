using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveSeat;
using Newtonsoft.Json.Linq;

namespace FoireMuses.Core.Business
{
    class JIntervene : JDocument
    {
         public JIntervene ()
		{
			this.Add ("type", "scene");
		}

         public JIntervene(JObject jobject) : base(jobject) { }
    }
}
