using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Loveseat.Business;
using FoireMuses.Core.Interfaces;
using LoveSeat;
using LoveSeat.Interfaces;

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
                if (this["isMaster"] == null)
                    return false;
                return this["isMaster"].Value<bool>();
            }
            set { this["isMaster"] = value; }
        }

        public string MasterId
        {
            get
            {
                if (this["masterId"] == null)
                    return null;
                return this["masterId"].Value<string>();
            }
            set { this["masterId"] = value; }
        }

        public string Title
        {
            get
            {
                if (this["title"] == null)
                    return null;
                return this["title"].Value<string>();
            }
            set { this["title"] = value; }
        }

        public string CodageMelodiqueRISM
        {
            get
            {
                if (this["codageMelodiqueRISM"] == null)
                    return null;
                return this["codageMelodiqueRISM"].Value<string>();
            }
            set { this["codageMelodiqueRISM"] = value; }
        }

        public string CodageParIntervalles
        {
            get
            {
                if (this["codageParIntervalles"] == null)
                    return null;
                return this["codageParIntervalles"].Value<string>();
            }
            set { this["codageParIntervalles"] = value; }
        }

        public string CodageRythmique
        {
            get
            {
                if (this["codageRythmique"] == null)
                    return null;
                return this["codageRythmique"].Value<string>();
            }
            set { this["codageRythmique"] = value; }
        }

		public string Code1
		{
			get
			{
				if (this["code1"] == null)
					return null;
				return this["code1"].Value<string>();
			}
			set { this["code1"] = value; }
		}

		public string Code2
		{
			get
			{
				if (this["code2"] == null)
					return null;
				return this["code2"].Value<string>();
			}
			set { this["code2"] = value; }
		}

        public string Coirault
        {
            get
            {
                if (this["coirault"] == null)
                    return null;
                return this["coirault"].Value<string>();
            }
            set { this["coirault"] = value; }
        }

        public string Composer
        {
            get
            {
                if (this["composer"] == null)
                    return null;
                return this["composer"].Value<string>();
            }
            set { this["composer"] = value; }
        }

        public string CoupeMetrique
        {
            get
            {
                if (this["coupeMetrique"] == null)
                    return null;
                return this["coupeMetrique"].Value<string>();
            }
            set { this["coupeMetrique"] = value; }
        }

        public string Verses
        {
            get
            {
                if (this["verses"] == null)
                    return null;
                return this["verses"].Value<string>();
            }
            set { this["verses"] = value; }
        }

        public string Delarue
        {
            get
            {
                if (this["delarue"] == null)
                    return null;
                return this["delarue"].Value<string>();
            }
            set { this["delarue"] = value; }
        }

        public string Comments
        {
            get
            {
                if (this["comments"] == null)
                    return null;
                return this["comments"].Value<string>();
            }
            set { this["comments"] = value; }
        }

        public string Editor
        {
            get
            {
                if (this["editor"] == null)
                    return null;
                return this["editor"].Value<string>();
            }
            set { this["editor"] = value; }
        }

        public string RythmSignature
        {
            get
            {
                if (this["rythmSignature"] == null)
                    return null;
                return this["rythmSignature"].Value<string>();
            }
            set { this["rythmSignature"] = value; }
        }

        public string OtherTitles
        {
            get
            {
                if (this["otherTitles"] == null)
                    return null;
                return this["otherTitles"].Value<string>();
            }
            set { this["otherTitles"] = value; }
        }

        public string Stanza
        {
            get
            {
                if (this["stanza"] == null)
                    return null;
                return this["stanza"].Value<string>();
            }
            set { this["stanza"] = value; }
        }

        public string ScoreType
        {
            get
            {
                if (this["type"] == null)
                    return null;
                return this["type"].Value<string>();
            }
            set { this["type"] = value; }
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
			get {
				if (this["tags"] == null)
					return null;
				return this["tags"].Values<string>(); }
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
			get {
				if (this["creatorId"] == null)
					return null;
				return this["creatorId"].Value<string>(); }
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
