using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using LoveSeat;
using FoireMuses.Core.Interfaces;

namespace FoireMuses.Core.Business
{
    class JTag : Document
    {
         public JTag ()
		{
			this.Add ("type", "scene");
		}

         public JTag(JObject jobject) : base(jobject) { }

    }
}
