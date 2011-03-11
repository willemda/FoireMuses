using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using LoveSeat;

namespace FoireMuses.Core.Business
{
    class JTag : JDocument
    {
         public JTag ()
		{
			this.Add ("type", "scene");
		}

         public JTag(JObject jobject) : base(jobject) { }
    }
}
