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
    public class JPlay : Document
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
