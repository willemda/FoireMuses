using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveSeat.Interfaces;
using LoveSeat;
using Newtonsoft.Json.Linq;

namespace FoireMuses.Core.Loveseat.Business
{
	public class Document : JDocument, IAuditableDocument
	{
		public DateTimeOffset CreationDate { get; private set; }
		public DateTimeOffset LastUpdateDate { get; private set; }

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
			CreationDate = DateTimeOffset.Now;
			LastUpdateDate = DateTimeOffset.Now;
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
			LastUpdateDate = DateTimeOffset.Now;
		}
	}
}
