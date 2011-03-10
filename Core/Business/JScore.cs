using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using LoveSeat;

namespace FoireMuses.Core.Business
{
    /// <summary>
    /// represent a Score(un air) object in json
    /// </summary>
    public class JScore : JDocument
    {
		
		public JScore ()
		{
			this.Add ("type", "score");
		}

        public JScore(JObject jobject) : base(jobject) { }
    }
}
