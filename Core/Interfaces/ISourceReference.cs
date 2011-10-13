using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoireMuses.Core.Interfaces
{
	public interface ISourceReference
	{
		string SourceId { get; set; }
		string Page { get; set; }
		int? AirNumber { get; set; }
		int? Tome { get; set; }
		int? Volume { get; set; }
	}
}
