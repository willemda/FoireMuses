using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveSeat;
using Newtonsoft.Json.Linq;

namespace FoireMuses.Core.Business
{
    /// <summary>
    /// represent a Character(un personnage) object in json
    /// </summary>
    public class JCharacter : JDocument

    {
        public JCharacter ()
		{
			this.Add ("type", "character");
		}

        public JCharacter(JObject jobject) : base(jobject) { }
    }
}
