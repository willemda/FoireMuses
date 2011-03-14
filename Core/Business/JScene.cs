using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using LoveSeat;
using FoireMuses.Core.Interfaces;

namespace FoireMuses.Core.Business
{
    /// <summary>
    /// represent a scene(une scene) object in json
    /// </summary>
    class JScene : Document
    {
        public JScene ()
		{
			this.Add ("type", "scene");
		}

        public JScene(JObject jobject) : base(jobject) { }

    }
}
