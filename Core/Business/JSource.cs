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
    public class JSource : JDocument, IAuditableDocument
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
