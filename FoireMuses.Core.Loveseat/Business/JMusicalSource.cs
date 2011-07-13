using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using Newtonsoft.Json.Linq;

namespace FoireMuses.Core.Loveseat.Business
{
	public class JMusicalSource : JObject, IMusicalSource
	{

		public JMusicalSource()
		{

		}

		public JMusicalSource(JObject jobject)
			: base(jobject)
		{
		}

		public string SourceId
		{
			get { return this["id"].Value<string>(); }
			set { this["id"] = value; }
		}

		public int? AirNumber
		{
			get
			{
				return this["air"].Value<int?>();
			}
			set
			{
				this["air"] = value;
			}
		}

		public string Page
		{
			get
			{
				return this["page"].Value<string>();
			}
			set
			{
				this["page"] = value;
			}
		}

		public int? Tome
		{
			get
			{
				return this["tome"].Value<int?>();
			}
			set
			{
				this["tome"] = value;
			}
		}

		public int? Volume
		{
			get
			{
				return this["volume"].Value<int?>();
			}
			set
			{
				this["volume"] = value;
			}
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
