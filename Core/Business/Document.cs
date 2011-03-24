using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveSeat.Interfaces;
using LoveSeat;
using Newtonsoft.Json.Linq;

namespace FoireMuses.Core.Business
{
	public class Document : JDocument, IAuditableDocument
	{
		public DateTime CreationDate { get; private set; }
		public DateTime LastUpdateDate { get; private set; }

		public Document() { }

		public Document(JObject jobject)
			: base(jobject)
		{
		}

		public virtual void Created()
		{

		}

		public virtual void Creating()
		{
			CreationDate = DateTime.Now;
			LastUpdateDate = DateTime.Now;
		}

		public virtual void Deleted()
		{

		}

		public virtual void Deleting()
		{

		}

		public virtual void Updated()
		{

		}

		public virtual void Updating()
		{
			LastUpdateDate = DateTime.Now;
		}
	}
}
