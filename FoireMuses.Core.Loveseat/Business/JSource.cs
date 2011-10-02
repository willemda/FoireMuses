using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DreamSeat;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Interfaces;
using DreamSeat.Interfaces;
using FoireMuses.Core.Loveseat.Business;
using FoireMuses.Core.Helpers;

namespace FoireMuses.Core.Business
{
	/// <summary>
	/// represent a Source(une source) object in json
	/// </summary>
	public class JSource : Document, ISource
	{
		public JSource()
		{
			this.Add("otype", "source");
		}

		public JSource(JObject jobject)
			: base(jobject)
		{
			JToken type;
			if (this.TryGetValue("otype", out type))
			{
				if (type.Value<string>() != "source")
					throw new Exception("Bad object type");
			}
			else
			{
				this.Add("otype", "source");
			}
		}

		public string Name
		{
			get
			{
				return this.RetrieveStringCheck("name");
			}
			set { this.AddCheck("name", value); }
		}

		public string Publisher
		{
			get
			{
				return this.RetrieveStringCheck("publisher");
			}
			set { this.AddCheck("publisher", value); }
		}

		public string FreeZone
		{
			get
			{
				return this.RetrieveStringCheck("free");
			}
			set { this.AddCheck("free", value); }
		}

		public string Cote
		{
			get
			{
				return this.RetrieveStringCheck("cote");
			}
			set { this.AddCheck("cote", value); }
		}

		public string Abbreviation
		{
			get
			{
				return this.RetrieveStringCheck("abbr");
			}
			set { this.AddCheck("abbr", value); }
		}

		public bool ApproxDate
		{
			get
			{
				return this.RetrieveBoolCheck("approx");
			}
			set { this.AddCheck("approx", value); }
		}

		public bool IsMusicalSource
		{
			get
			{
				return this.RetrieveBoolCheck("musicalSource");
			}
			set { this.AddCheck("musicalSource", value); }
		}

		public int? DateFrom
		{
			get
			{
				return this.RetrieveIntCheck("dateFrom");
			}
			set { this.AddCheck("dateFrom", value); }
		}

		public int? DateTo
		{
			get
			{
				return this.RetrieveIntCheck("dateTo");
			}
			set { this.AddCheck("dateTo", value); }
		}

		public IEnumerable<string> Tags
		{
			get
			{
				if (this["tags"] == null)
					return null;
				return this["tags"].Values<string>();
			}
		}

		public void AddTag(string tag)
		{
			if (!Tags.Contains(tag))
			{
				JArray temp = this["tags"].Value<JArray>();
				temp.Add(tag);
				this["tags"] = temp;
			}
		}

		public void RemoveTag(string tag)
		{
			this["tags"] = this["tags"].Value<JArray>().Remove(tag);
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

		public IList<string> CollaboratorsId
		{
			get { return this["collaboratorsId"].Values<string>().ToList(); }
			set 
			{
				JArray ja = new JArray();
				foreach (string item in value)
				{
					ja.Add(item);
				}
				this["collaboratorsId"] = ja;
			}
		}



		public void AddCollaborator(string collab)
		{
			if (!Tags.Contains(collab))
			{
				JArray temp = this["collaboratorsId"].Value<JArray>();
				temp.Add(collab);
				this["collaboratorsId"] = temp;
			}
		}

		public void RemoveCollaborator(string collab)
		{
			this["collaboratorsId"] = this["collaboratorsId"].Value<JArray>().Remove(collab);
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
