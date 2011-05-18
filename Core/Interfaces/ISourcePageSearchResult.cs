using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoireMuses.Core.Interfaces
{
	public interface ISourcePageSearchResult : ISearchResultItem
	{
		string Id { get; set; }
		string PageNumber { get; set; }
		string DisplayPageNumber { get; set; }
		string SourceId { get; set; }
	}
}
