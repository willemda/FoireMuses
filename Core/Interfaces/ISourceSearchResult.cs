using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoireMuses.Core.Interfaces
{
	public interface ISourceSearchResult : ISearchResultItem
	{
		string Id { get; set; }
		string Name { get; set; }
		string Publisher { get; set; }
		string DateTo { get; set; }
		string DateFrom { get; set; }
	}
}
