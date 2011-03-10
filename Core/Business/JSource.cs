using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveSeat;
using Newtonsoft.Json.Linq;

namespace FoireMuses.Core.Business
{
    /// <summary>
    /// represent a Source(une source) object in json
    /// </summary>
    public class JSource : JDocument
    {
        public JSource ()
		{
			this.Add ("type", "source");
		}

        public JSource(JObject jobject) : base(jobject) { }
    }
}
