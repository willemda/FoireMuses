using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoireMuses.Core.Interfaces
{
    public interface ISource
    {
		string Id { get; set; }
		string Rev { get; set; }

		string Name { get; set; }
		string FreeZone { get; set; }
		int? DateFrom { get; set; }
		int? DateTo{ get; set; }
		string Cote{ get; set; }
		string Abbreviation { get; set; }
		bool IsMusicalSource { get; set; }
		bool ApproxDate { get; set; }
		string Publisher { get; set; }

    }
}
