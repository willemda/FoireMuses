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
    public class JScore :JDocument, IAuditableDocument
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


        public void Created()
        {
            throw new NotImplementedException();
        }

        public void Creating()
        {
            throw new NotImplementedException();
        }

        public void Deleted()
        {
            throw new NotImplementedException();
        }

        public void Deleting()
        {
            throw new NotImplementedException();
        }

        public void Updated()
        {
            throw new NotImplementedException();
        }

        public void Updating()
        {
            throw new NotImplementedException();
        }
    }
}
