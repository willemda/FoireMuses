using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using LoveSeat;
using FoireMuses.Core.Interfaces;
using LoveSeat.Interfaces;

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

		public string SourceMusicaleId
		{
			get { return this["musicalSource"]["id"].Value<string>(); }
			set { this["musicalSource"]["id"] = value; }
		}

		public int SourceMusicaleActNumber
		{
			get { return this["musicalSource"]["act"].Value<int>(); }
			set { this["musicalSource"]["act"] = value; }
		}

		public string SourceMusicaleActId
		{
			get { return this["musicalSource"]["actId"].Value<string>(); }
			set { this["musicalSource"]["actId"] = value; }
		}

		public int SourceMusicaleSceneNumber
		{
			get { return this["musicalSource"]["scene"].Value<int>(); }
			set { this["musicalSource"]["scene"] = value; }
		}

		public string SourceMusicaleSceneId
		{
			get { return this["musicalSource"]["sceneId"].Value<string>(); }
			set { this["musicalSource"]["sceneId"] = value; }
		}

		public string SourceMusicaleText
		{
			get { return this["musicalSource"]["text"].Value<string>(); }
			set { this["musicalSource"]["text"] = value; }
		}

		public bool SourceMusicaleIsSuggested
		{
			get { return this["musicalSource"]["isSuggested"].Value<bool>(); }
			set { this["musicalSource"]["isSuggested"] = value; }
		}

		public string SourceTextuelleId
		{
			get { return this["textualSource"]["id"].Value<string>(); }
			set { this["textualSource"]["id"] = value; }
		}

		public int SourceTextuelleAir
		{
			get { return this["textualSource"]["air"].Value<int>(); }
			set { this["textualSource"]["air"] = value; }
		}

		public string SourceTextuellePage
		{
			get { return this["textualSource"]["page"].Value<string>(); }
			set { this["textualSource"]["page"] = value; }
		}

		public int SourceTextuelleTome
		{
			get { return this["textualSource"]["tome"].Value<int>(); }
			set { this["textualSource"]["tome"] = value; }
		}

		public int SourceTextuelleVolume
		{
			get { return this["textualSource"]["volume"].Value<int>(); }
			set { this["textualSource"]["volume"] = value; }
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
