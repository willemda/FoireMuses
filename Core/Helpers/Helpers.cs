using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using MindTouch.Dream;

namespace FoireMuses.Core.Helpers
{
	public static class JObjectHelpers
	{
		public static void AddCheck(this JObject jo, string fieldName, JToken fieldValue)
		{
			if (fieldValue != null)
				jo.Add(fieldName, fieldValue);
		}
	}
}
