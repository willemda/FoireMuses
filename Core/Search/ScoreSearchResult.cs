using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Helpers;

namespace FoireMuses.Core
{
	public class ScoreSearchResult : IScoreSearchResult
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public string Composer { get; set; }
		public string Editor { get; set; }
		public string Verses { get; set; }
		public string Music { get; set; }
		public string ToJson()
		{
			JObject jobject = new JObject();
			jobject.AddCheck("_id", Id);
			jobject.AddCheck("title", Title);
			jobject.AddCheck("composer", Composer);
			jobject.AddCheck("editor", Editor);
			jobject.AddCheck("verses", Verses);
			jobject.AddCheck("music", Music);
			return jobject.ToString();
		}
	}
}
