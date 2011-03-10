﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveSeat;
using Newtonsoft.Json.Linq;

namespace FoireMuses.Core.Business
{
    /// <summary>
    /// represent a Play(une pièce) object in json
    /// </summary>
    public class JPlay : JDocument
    {
       public JPlay ()
		{
			this.Add ("type", "play");
		}

       public JPlay(JObject jobject) : base(jobject) { }
    }
}
