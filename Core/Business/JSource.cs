using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveSeat;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Interfaces;
using LoveSeat.Interfaces;

namespace FoireMuses.Core.Business
{
    /// <summary>
    /// represent a Source(une source) object in json
    /// </summary>
    public class JSource : Document
    {
        public JSource ()
		{
			this.Add ("type", "source");
		}

        public JSource(JObject jobject) : base(jobject) {
            JToken type;
            if (this.TryGetValue("otype", out type))
            {
                if (type.Value<string>() != "source")
                    throw new Exception("Bad object type");
            }
            else
            {
                this.Add("otype", "source");
            }
        }


        new public void Created()
        {
            base.Created();
        }

        new public void Creating()
        {
            base.Creating();
        }

        new public void Deleted()
        {
            base.Deleted();
        }

        new public void Deleting()
        {
            base.Deleting();
        }

        new public void Updated()
        {
            base.Updated();
        }

        new public void Updating()
        {
            base.Updating();
        }
    }
}
