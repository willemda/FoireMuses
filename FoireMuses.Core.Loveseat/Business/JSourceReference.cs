using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using Newtonsoft.Json.Linq;

namespace FoireMuses.Core.Loveseat.Business
{
	public class JSourceReference : JObject, ISourceReference
	{
		public JSourceReference(){}

		public JSourceReference(JObject aJObject):base(aJObject){}

		public string SourceId
		{
			get { return this["id"].Value<string>(); }
			set { this["id"] = value; }
		}

		public int? AirNumber
		{
			get { return this["air"].Value<int?>(); }
			set { this["air"] = value; }
		}

		public string Page
		{
			get { return this["page"].Value<string>(); }
			set { this["page"] = value; }
		}

		public int? Tome
		{
			get { return this["tome"] != null ? this["tome"].Value<int?>() : default(int?); }
			set { this["tome"] = value; }
		}

		public int? Volume
		{
			get { return this["volume"] != null ? this["volume"].Value<int?>() : default(int?); }
			set { this["volume"] = value; }
		}

	}
}
