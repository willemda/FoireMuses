using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoireMuses.Core.Interfaces
{
	public interface ISourcePage
	{
		string Id { get; set; }
		string Rev { get; set; }

		string SourceId { get; set; }
		int? PageNumber { get; set; }
		int? DisplayPageNumber { get; set; }
		int? PageNumberFormat { get; set; }
		string TextContent { get; set; }

		string CreatorId { get; }
		string LastModifierId { get; }

		IEnumerable<string> CollaboratorsId { get; }
	}
}
