using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Business;

namespace FoireMuses.Core
{
	public class Factory
	{
		public static IScore IScoreFromJson(string json)
		{
			return new JScore(JObject.Parse(json));
		}

		public static IUser IUserFromJson(string json)
		{
			return new JUser(JObject.Parse(json));
		}

		public static string ResultToJson(IScore score)
		{
			return (score as JScore).ToString();
		}

		public static string ResultToJson(IUser user)
		{
			return (user as JUser).ToString();
		}

		public static string ResultToJson(SearchResult<IUser> result)
		{
			JObject info = new JObject { { "total_rows", result.TotalCount }, { "offset", result.Offset }, { "max", result.Max } };
			JArray users = new JArray();
			foreach (IUser user in result)
			{
				users.Add(user);
			}
			info.Add("rows", users);
			return info.ToString();
		}

		public static string ResultToJson(SearchResult<IScore> result)
		{
			JObject info = new JObject{{"total_rows",result.TotalCount},{"offset",result.Offset},{"max",result.Max}};
			JArray scores = new JArray();
			foreach (IScore score in result)
			{
				scores.Add(score);
			}
			info.Add("rows", scores);
			return info.ToString();
		}
	}
}
