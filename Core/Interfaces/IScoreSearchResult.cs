using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoireMuses.Core.Interfaces
{
	public interface IScoreSearchResult : ISearchResultItem
	{
		string Id { get; set; }
		string Title { get; set; }
		string Editor { get; set; }
		string Verses { get; set; }
		string Composer { get; set; }
		string MusicalSourceReferenceText { get; set; }
		string TextualSourceReferenceText { get; set; }
	}
}
