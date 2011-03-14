using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveSeat;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Interfaces;

namespace FoireMuses.Core.Business
{
    class JIntervene : Document
    {
         public JIntervene ()
		{
			this.Add ("type", "scene");
		}

         public JIntervene(JObject jobject) : base(jobject) { }


    }
}
