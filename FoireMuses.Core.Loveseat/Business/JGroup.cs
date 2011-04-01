using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core.Loveseat.Business;

namespace FoireMuses.Core.Business
{
	public class JGroup : Document, IGroup
	{
		public JGroup()
		{
			this.Add("type", "group");
		}

		public JGroup(JObject jobject)
			: base(jobject)
		{
			JToken type;
			if (this.TryGetValue("otype", out type))
			{
				if (type.Value<string>() != "group")
					throw new Exception("Bad object type");
			}
			else
			{
				this.Add("otype", "group");
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


		public string Name
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public bool IsPublic
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public string CreatorId
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}
	}
}
