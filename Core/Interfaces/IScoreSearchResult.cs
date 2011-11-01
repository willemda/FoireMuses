using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace FoireMuses.Core.Interfaces
{
	public interface IScoreSearchResult : ISearchResultItem
	{
		string Id { get; set; }
		string Title { get; set; }
		string Editor { get; set; }
		string Verses { get; set; }
		string Composer { get; set; }
		JObject MusicalSourceReference { get; set; }
		JObject TextualSourceReference { get; set; }
	}
}
