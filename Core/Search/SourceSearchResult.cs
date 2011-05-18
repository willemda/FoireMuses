using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Helpers;

namespace FoireMuses.Core
{
	public class SourceSearchResult : ISourceSearchResult
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Publisher { get; set; }
		public string DateFrom { get; set; }
		public string DateTo { get; set; }
		public string ToJson()
		{
			JObject jobject = new JObject();
			jobject.AddCheck("_id", Id);
			jobject.AddCheck("name", Name);
			jobject.AddCheck("publisher", Publisher);
			jobject.AddCheck("dateFrom", DateFrom);
			jobject.AddCheck("dateTo", DateTo);
			return jobject.ToString();
		}
	}
}
