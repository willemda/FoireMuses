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
    /// represent a Play(une pièce) object in json
    /// </summary>
    public class JPlay : JDocument, IAuditableDocument
    {
       public JPlay ()
		{
			this.Add ("type", "play");
		}

       public JPlay(JObject jobject) : base(jobject) {
           JToken type;
           if (this.TryGetValue("otype", out type))
           {
               if (type.Value<string>() != "play")
                   throw new Exception("Bad object type");
           }
           else
           {
               this.Add("otype", "play");
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
