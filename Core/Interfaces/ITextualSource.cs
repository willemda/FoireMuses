using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoireMuses.Core.Interfaces
{
	public interface ITextualSource
	{
		string SourceId { get; set; }
		string Page { get; set; }
		int AirNumber { get; set; }
		int? ActNumber { get; set; }
		int? SceneNumber { get; set; }
		string Comment { get; set; }
		bool IsSuggested { get; set; }
	}
}
