using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using LoveSeat;
using FoireMuses.Core.Interfaces;

namespace FoireMuses.Core.Business
{
    /// <summary>
    /// represent a Act(un acte) object in json
    /// </summary>
    public class JAct : Document
    {
        public JAct ()
		{
			this.Add ("type", "act");
		}

        public JAct(JObject jobject) : base(jobject) { }


        
    }
}
