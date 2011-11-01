using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using MindTouch.Dream;
using FoireMuses.Core.Interfaces;

namespace FoireMuses.Core.Helpers
{
	public static class JObjectHelper
	{

		public static void AddCheck(this JObject jo, string fieldName, string fieldValue)
		{
			if (fieldValue != null)
				jo[fieldName] = fieldValue;
			else
				jo.Remove(fieldName);
		}

		public static void AddCheck(this JObject jo, string fieldName, int? fieldValue)
		{
			if (fieldValue != null)
				jo[fieldName] = fieldValue;
			else
				jo.Remove(fieldName);
		}

		public static void AddCheck(this JObject jo, string fieldName, bool? fieldValue)
		{
			if (fieldValue != null)
				jo[fieldName] = fieldValue;
			else
				jo.Remove(fieldName);
		}

		public static void AddCheck(this JObject jo, string fieldName, bool fieldValue)
		{
			jo[fieldName] = fieldValue;
		}

		public static string RetrieveStringCheck(this JObject jo, string fieldName)
		{
			if (jo[fieldName] != null)
				return jo[fieldName].Value<string>();
			return null;
		}

		public static bool? RetrieveNullableBoolCheck(this JObject jo, string fieldName)
		{
			if (jo[fieldName] != null)
				return jo[fieldName].Value<bool?>();
			return null;
		}

		public static bool RetrieveBoolCheck(this JObject jo, string fieldName)
		{
			if (jo[fieldName] != null)
				return jo[fieldName].Value<bool>();
			return false;
		}

		public static int? RetrieveIntCheck(this JObject jo, string fieldName)
		{
			if (jo[fieldName] != null)
				return jo[fieldName].Value<int?>();
			return null;
		}
	}
}
