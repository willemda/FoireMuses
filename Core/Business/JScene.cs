using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using LoveSeat;

namespace FoireMuses.Core.Business
{
    /// <summary>
    /// represent a scene(une scene) object in json
    /// </summary>
    class JScene : JDocument
    {
        public JScene ()
		{
			this.Add ("type", "scene");
		}

        public JScene(JObject jobject) : base(jobject) { }
    }
}
