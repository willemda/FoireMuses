﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DreamSeat;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Interfaces;
using DreamSeat.Interfaces;
using FoireMuses.Core.Loveseat.Business;

namespace FoireMuses.Core.Business
{
	/// <summary>
	/// represent a Play(une pièce) object in json
	/// </summary>
	public class JPlay : Document, IPlay
	{

		public JPlay()
		{
			this.Add("type", "play");
		}

		public JPlay(JObject jobject)
			: base(jobject)
		{
			JToken type;
			if (this.TryGetValue("otype", out type))
			{
				if (type.Value<string>() != "play")
					throw new Exception("Bad object type");
			}
			else
			{
				this.Add("otype", "play");
			}
		}


		public override void Created()
		{
			base.Created();
		}

		public override void Creating()
		{
			base.Creating();
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
		}


		public string Abstract
		{
			get
			{
				return this["abstract"].Value<string>();
			}
			set
			{
				this["abstract"] = value;
			}
		}

		public string ActionLocation
		{
			get
			{
				return this["actionLocation"].Value<string>();
			}
			set
			{
				this["actionLocation"] = value;
			}
		}

		public string Author
		{
			get
			{
				return this["author"].Value<string>();
			}
			set
			{
				this["author"] = value;
			}
		}

		public string ContemporaryComments
		{
			get
			{
				return this["comments"].Value<string>();
			}
			set
			{
				this["comments"] = value;
			}
		}

		public string CreationPlace
		{
			get
			{
				return this["creationPlace"].Value<string>();
			}
			set
			{
				this["creationPlace"] = value;
			}
		}

		public string CreationYear
		{
			get
			{
				return this["creationYear"].Value<string>();
			}
			set
			{
				this["creationYear"] = value;
			}
		}

		public string Critics
		{
			get
			{
				return this["critics"].Value<string>();
			}
			set
			{
				this["critics"] = value;
			}
		}

		public string Decors
		{
			get
			{
				return this["decors"].Value<string>();
			}
			set
			{
				this["decors"] = value;
			}
		}

		public string EntrepreneurName
		{
			get
			{
				return this["entrepreneurName"].Value<string>();
			}
			set
			{
				this["entrepreneurName"] = value;
			}
		}

		public string Genre
		{
			get
			{
				return this["type"].Value<string>();
			}
			set
			{
				this["type"] = value;
			}
		}

		public string Iconography
		{
			get
			{
				return this["iconography"].Value<string>();
			}
			set
			{
				this["iconography"] = value;
			}
		}

		public string MusicianName
		{
			get
			{
				return this["musicianName"].Value<string>();
			}
			set
			{
				this["musicianName"] = value;
			}
		}

		public string Resonances
		{
			get
			{
				return this["resonances"].Value<string>();
			}
			set
			{
				this["resonances"] = value;
			}
		}

		public string SourceFolio
		{
			get
			{
				return this["sourceFolio"].Value<string>();
			}
			set
			{
				this["sourceFolio"] = value;
			}
		}

		public string Title
		{
			get
			{
				return this["title"].Value<string>();
			}
			set
			{
				this["title"] = value;
			}
		}

		public string SourceId
		{
			get
			{
				return this["sourceId"].Value<string>();
			}
			set
			{
				this["sourceId"] = value;
			}
		}

		public int? SourceTome
		{
			get
			{
				return this["sourceTome"].Value<int?>();
			}
			set
			{
				this["sourceTome"] = value;
			}
		}

		public int? SourceVolume
		{
			get
			{
				return this["sourceVolume"].Value<int?>();
			}
			set
			{
				this["sourceVolume"] = value;
			}
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
	}
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  