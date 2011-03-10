using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Core.Business
{
    public class JScore
    {
		JObject Doc{get;set;}
		
		public JScore ()
		{
			Doc.Add ("type", "score");
		}
    }
}
