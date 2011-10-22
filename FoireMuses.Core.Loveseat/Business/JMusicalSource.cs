using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using Newtonsoft.Json.Linq;

namespace FoireMuses.Core.Loveseat.Business
{
	public class JMusicalSource : JSourceReference, IMusicalSource
	{
		public JMusicalSource()
		{

		}

		public JMusicalSource(JObject jobject)
			: base(jobject)
		{
		}

		public bool IsSuggested
		{
			get
			{
				return this["isSuggested"].Value<bool>();
			}
			set
			{
				this["isSuggested"] = value;
			}
		}
	}
}
