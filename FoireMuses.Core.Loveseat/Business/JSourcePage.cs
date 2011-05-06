using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core.Helpers;

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
			get
			{
				return this.RetrieveIntCheck("pageNumber");
			}
			set { this.AddCheck("pageNumber", value); }
		}

		public int? DisplayPageNumber
		{
			get
			{
				return this.RetrieveIntCheck("displayPageNumber");
			}
			set { this.AddCheck("displayPageNumber", value); }
		}

		public int? PageNumberFormat
		{
			get
			{
				return this.RetrieveIntCheck("pageNumberFormat");
			}
			set { this.AddCheck("pageNumberFormat", value); }
		}

		public string TextContent
		{
			get
			{
				return this.RetrieveStringCheck("textContent");
			}
			set { this.AddCheck("textContent", value); }
		}


		public string SourceId
		{
			get
			{
				return this.RetrieveStringCheck("sourceId");
			}
			set { this.AddCheck("souceId", value); }
		}

		public string CreatorId
		{
			get
			{
				return this.RetrieveStringCheck("creatorId");
			}
			private set { this.AddCheck("creatorId", value); }
		}

		public string LastModifierId
		{
			get
			{
				return this.RetrieveStringCheck("lastModifierId");
			}
			private set { this.AddCheck("lastModifierId", value); }
		}
		
		public IEnumerable<string> CollaboratorsId
		{
			get { return this["collaboratorsId"].Values<string>(); }
		}

		public override void Created()
		{
			base.Created();
		}

		public override void Creating()
		{
			base.Creating();
			CreatorId = Context.Current.User.Id;
		}

		public override void Deleted()
		{
			base.Deleted();
		}

		public override void Deleting()
		{
			base.Deleting();
		}

		public override void Updated()
		{
			base.Updated();
		}

		public override void Updating()
		{
			base.Updating();
			LastModifierId = Context.Current.User.Id;
		}
	}
}
