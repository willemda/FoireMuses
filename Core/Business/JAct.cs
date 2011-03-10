using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using LoveSeat;

namespace FoireMuses.Core.Business
{
    /// <summary>
    /// represent a Act(un acte) object in json
    /// </summary>
    public class JAct : JDocument
    {
        public JAct ()
		{
			this.Add ("type", "act");
		}

        public JAct(JObject jobject) : base(jobject) { }
    }
}
