using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveSeat;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Interfaces;
using LoveSeat.Interfaces;
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
	}
}
