using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using LoveSeat;
using FoireMuses.Core.Interfaces;
using LoveSeat.Interfaces;

namespace FoireMuses.Core.Business
{
    /// <summary>
    /// represent a Score(un air) object in json
    /// </summary>
    public class JScore :Document
    {
		
		public JScore ()
		{
			this.Add ("type", "score");
		}

        public JScore(JObject jobject) : base(jobject) {
            JToken type;
            if (this.TryGetValue("otype", out type))
            {
                if (type.Value<string>() != "score")
                    throw new Exception("Bad object type");
            }
            else
            {
                this.Add("otype", "score");
            }
        }


        public override void Created()
        {
            base.Created();
        }

        public override void Creating()
        {
            base.Creating();
        }

        public override void Deleted()
        {
            base.Deleted();
        }

        public override void Deleting()
        {
            base.Deleting();
        }

        public override void Updated()
        {
            base.Updated();
        }

        public override void Updating()
        {
            base.Updating();
        }
    }
}
