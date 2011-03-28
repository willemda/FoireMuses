using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using LoveSeat;
using FoireMuses.Core.Interfaces;
using LoveSeat.Interfaces;
using FoireMuses.Core.Loveseat.Business;

namespace FoireMuses.Core.Business
{
	/// <summary>
	/// represent a Score(un air) object in json
	/// </summary>
	public class JScore : Document, IScore
	{

		public JScore()
		{
			this.Add("type", "score");
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

		public string Title
		{
			get { return this["title"].Value<string>(); }
			set { this["title"] = value; }
		}

		public string Code1
		{
			get { return this["code1"].Value<string>(); }
			set { this["code1"] = value; }
		}

		public string Code2
		{
			get { return this["code2"].Value<string>(); }
			set { this["code2"] = value; }
		}

		public string Coirault
		{
			get { return this["coirault"].Value<string>(); }
			set { this["coirault"] = value; }
		}

		public string Composer
		{
			get { return this["composer"].Value<string>(); }
			set { this["composer"] = value; }
		}

		public string CoupeMetrique
		{
			get { return this["coupeMetrique"].Value<string>(); }
			set { this["coupeMetrique"] = value; }
		}

		public string Verses
		{
			get { return this["verses"].Value<string>(); }
			set { this["verses"] = value; }
		}

		public string Delarue
		{
			get { return this["delarue"].Value<string>(); }
			set { this["delarue"] = value; }
		}

		public string Comments
		{
			get { return this["comments"].Value<string>(); }
			set { this["comments"] = value; }
		}

		public string Editor
		{
			get { return this["editor"].Value<string>(); }
			set { this["editor"] = value; }
		}

		public string RythmSignature
		{
			get { return this["rythmSignature"].Value<string>(); }
			set { this["rythmSignature"] = value; }
		}

		public string OtherTitles
		{
			get { return this["otherTitles"].Value<string>(); }
			set { this["otherTitles"] = value; }
		}

		public string Stanza
		{
			get { return this["stanza"].Value<string>(); }
			set { this["stanza"] = value; }
		}

		public string ScoreType
		{
			get { return this["type"].Value<string>(); }
			set { this["type"] = value; }
		}

		public ITextualSource TextualSource
		{
			get { return this["textualSource"].Value<JTextualSource>(); }
			set { this["textualSource"] = value as JTextualSource; }
		}

		public IMusicalSource MusicalSource
		{
			get { return this["musicalSource"].Value<JMusicalSource>(); }
			set { this["musicalSource"] = value as JTextualSource; }
		}

		public IEnumerable<string> Tags
		{
			get { return this["tags"].Values<string>(); }
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
