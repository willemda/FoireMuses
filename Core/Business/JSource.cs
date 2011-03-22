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


        public void Created()
        {
            base.Created();
        }

        public void Creating()
        {
            base.Creating();
        }

        public void Deleted()
        {
            base.Deleted();
        }

        public void Deleting()
        {
            base.Deleting();
        }

        public void Updated()
        {
            base.Updated();
        }

        public void Updating()
        {
            base.Updating();
        }
    }
}
