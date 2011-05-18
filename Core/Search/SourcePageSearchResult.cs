using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Helpers;

namespace FoireMuses.Core
{
	public class SourcePageSearchResult : ISourcePageSearchResult
	{
		public string Id { get; set; }
		public string PageNumber { get; set; }
		public string DisplayPageNumber { get; set; }
		public string SourceId { get; set; }
		public string ToJson()
		{
			JObject jobject = new JObject();
			jobject.AddCheck("_id", Id);
			jobject.AddCheck("pageNumber", PageNumber);
			jobject.AddCheck("displayPageNumber", DisplayPageNumber);
			return jobject.ToString();
		}
	}
}
