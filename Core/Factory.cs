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
	}
}
