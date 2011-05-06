using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Loveseat.Business;
using FoireMuses.Core.Interfaces;
using LoveSeat;
using LoveSeat.Interfaces;
using FoireMuses.Core.Helpers;

namespace FoireMuses.Core.Business
{
	/// <summary>
	/// represent a Score(un air) object in this
	/// </summary>
	public class JScore : Document, IScore
	{

		public JScore()
		{
			this.Add("otype", "score");
		}

		public JScore(JObject jobject)
			: base(jobject)
		{
			JToken type;
			if (this.TryGetValue("otype", out type))
			{
				if (type.Value<string>() != "score")
					throw new Exception("Bad object type");
			}
			else
			{
				this.Add("otype", "score");
			}
		}

		public bool IsMaster
		{
			get
			{
				return this.RetrieveBoolCheck("isMaster");
			}
			set { this.AddCheck("isMaster",value); }
		}

		public string MasterId
		{
			get
			{
				return this.RetrieveStringCheck("masterId");
			}
			set { this.AddCheck("masterId", value); }
		}

		public string Title
		{
			get
			{
				return this.RetrieveStringCheck("title");
			}
			set { this.AddCheck("title", value); }
		}

		public string CodageMelodiqueRISM
		{
			get
			{
				return this.RetrieveStringCheck("codageMelodiqueRISM");
			}
			set { this.AddCheck("codageMelodiqueRISM", value); }
		}

		public string CodageParIntervalles
		{
			get
			{
				return this.RetrieveStringCheck("codageParIntervalles");
			}
			set { this.AddCheck("codageParIntervalles", value); }
		}

		public string CodageRythmique
		{
			get
			{
				return this.RetrieveStringCheck("codageRythmique");
			}
			set { this.AddCheck("codageRythmique", value); }
		}

		public string Code1
		{
			get
			{
				return this.RetrieveStringCheck("code1");
			}
			set { this.AddCheck("code1", value); }
		}

		public string Code2
		{
			get
			{
				return this.RetrieveStringCheck("code2");
			}
			set { this.AddCheck("code2", value); }
		}

		public string Coirault
		{
			get
			{
				return this.RetrieveStringCheck("coirault");
			}
			set { this.AddCheck("coirault", value); }
		}

		public string Composer
		{
			get
			{
				return this.RetrieveStringCheck("composer");
			}
			set { this.AddCheck("composer", value); }
		}

		public string CoupeMetrique
		{
			get
			{
				return this.RetrieveStringCheck("coupeMetrique");
			}
			set { this.AddCheck("coupeMetrique", value); }
		}

		public string Verses
		{
			get
			{
				return this.RetrieveStringCheck("verses");
			}
			set { this.AddCheck("verses", value); }
		}

		public string Delarue
		{
			get
			{
				return this.RetrieveStringCheck("delarue");
			}
			set { this.AddCheck("delarue", value); }
		}

		public string Comments
		{
			get
			{
				return this.RetrieveStringCheck("comments");
			}
			set { this.AddCheck("comments", value); }
		}

		public string Editor
		{
			get
			{
				return this.RetrieveStringCheck("editor");
			}
			set { this.AddCheck("editor", value); }
		}

		public string RythmSignature
		{
			get
			{
				return this.RetrieveStringCheck("rythmSignature");
			}
			set { this.AddCheck("rythmSignature", value); }
		}

		public string OtherTitles
		{
			get
			{
				return this.RetrieveStringCheck("otherTitles");
			}
			set { this.AddCheck("otherTitles", value); }
		}

		public string Stanza
		{
			get
			{
				return this.RetrieveStringCheck("stanza");
			}
			set { this.AddCheck("stanza", value); }
		}

		public string ScoreType
		{
			get
			{
				return this.RetrieveStringCheck("type");
			}
			set { this.AddCheck("type", value); }
		}

		public ITextualSource TextualSource
		{
			get
			{
				if (this["textualSource"] == null)
					return null;
				return new JTextualSource(this["textualSource"].Value<JObject>());
			}
			set { this["textualSource"] = value as JTextualSource; }
		}

		public IMusicalSource MusicalSource
		{
			get
			{
				if (this["musicalSource"] == null)
					return null;
				return new JMusicalSource(this["musicalSource"].Value<JObject>());
			}
			set { this["musicalSource"] = value as JTextualSource; }
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

		public IEnumerable<string> CollaboratorsId
		{
			get { return this["collaboratorsId"].Values<string>(); }
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
