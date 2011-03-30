using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveSeat;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Interfaces;
using LoveSeat.Interfaces;
using FoireMuses.Core.Loveseat.Business;

namespace FoireMuses.Core.Business
{
	/// <summary>
	/// represent a Source(une source) object in json
	/// </summary>
	public class JSource : Document, ISource
	{
		public JSource()
		{
			this.Add("type", "source");
		}

		public JSource(JObject jobject)
			: base(jobject)
		{
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

		public string Name
		{
			get { return this["name"].Value<string>(); }
			set { this["name"] = value; }
		}

		public string Publisher
		{
			get { return this["publisher"].Value<string>(); }
			set { this["publisher"] = value; }
		}

		public string FreeZone
		{
			get { return this["free"].Value<string>(); }
			set { this["free"] = value; }
		}

		public string Cote
		{
			get { return this["cote"].Value<string>(); }
			set { this["cote"] = value; }
		}

		public string Abbreviation
		{
			get { return this["abbr"].Value<string>(); }
			set { this["abbr"] = value; }
		}

		public bool ApproxDate
		{
			get { return this["approx"].Value<bool>(); }
			set { this["approx"] = value; }
		}

		public bool IsMusicalSource
		{
			get { return this["musicalSource"].Value<bool>(); }
			set { this["musicalSource"] = value; }
		}

		public int? DateFrom
		{
			get { return this["dateFrom"].Value<int>(); }
			set { this["dateFrom"] = value; }
		}

		public int? DateTo
		{
			get { return this["dateTo"].Value<int>(); }
			set { this["dateTo"] = value; }
		}

		public override void Created()
		{
			base.Created();
		}

		public override void Creating()
		{
			base.Creating();
		}

		public override void Deleted()
		{
			base.Deleted();
		}

		public override void Deleting()
		{
			base.Deleting();
		}

		public override void Updated()
		{
			base.Updated();
		}

		public override void Updating()
		{
			base.Updating();
		}
	}
}
