using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Interfaces;

namespace FoireMuses.Core.Loveseat.Business
{
	public class JSourcePage : Document, ISourcePage
	{

		public JSourcePage()
		{
			this.Add("otype", "sourcePage");
		}

		public JSourcePage(JObject jobject)
			: base(jobject)
		{
			JToken type;
			if (this.TryGetValue("otype", out type))
			{
				if (type.Value<string>() != "sourcePage")
					throw new Exception("Bad object type");
			}
			else
			{
				this.Add("otype", "sourcePage");
			}
		}

		public int? PageNumber
		{
			get { return this["pageNumber"].Value<int?>(); }
			set { this["pageNumber"] = value; }
		}

		public int? DisplayPageNumber
		{
			get { return this["displayPageNumber"].Value<int?>(); }
			set { this["displayPageNumber"] = value; }
		}

		public int? PageNumberFormat
		{
			get { return this["pageNumberFormat"].Value<int?>(); }
			set { this["pageNumberFormat"] = value; }
		}

		public string TextContent
		{
			get { return this["textContent"].Value<string>(); }
			set { this["textContent"] = value; }
		}


		public string SourceId
		{
			get { return this["sourceId"].Value<string>(); }
			set { this["sourceId"] = value; }
		}

		public string CreatorId
		{
			get { return this["creatorId"].Value<string>(); }
			private set { this["creatorId"] = value; }
		}

		public string LastModifierId
		{
			get { return this["lastModifierId"].Value<string>(); }
			private set { this["lastModifierId"] = value; }
		}

		public IEnumerable<string> CollaboratorsId
		{
			get { return this["collaboratorsId"].Values<string>(); }
		}
	}
}
