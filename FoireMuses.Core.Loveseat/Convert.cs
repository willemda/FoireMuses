﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace FoireMuses.Core.Loveseat
{
	public class Convert<T>
	{
		public string ToJson(SearchResult<T> aSearchResult)
		{
			JObject json = new JObject { { "total_rows", aSearchResult.TotalCount }, { "offset", aSearchResult.Offset }, { "max", aSearchResult.Max } };
			JArray docs = new JArray();
			foreach (T doc in aSearchResult)
			{
				docs.Add(doc);
			}
			json.Add("rows", docs);
			return json.ToString();
		}

		public string ToJson(T anObject)
		{
			return anObject.ToString();
		}

		public T FromJson(string json)
		{
			Type type = typeof(T);
			Type[] argTypes = new Type[] { typeof(JObject) };

			ConstructorInfo cInfo = type.GetConstructor(argTypes);

			object[] argVals = new object[] { JObject.Parse(json) };
			return (T)cInfo.Invoke(argVals);
		}

	}
}
