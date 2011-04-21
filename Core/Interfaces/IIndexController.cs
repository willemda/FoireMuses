using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Tasking;
using FoireMuses.Core.Querys;

namespace FoireMuses.Core.Interfaces
{
	public interface IIndexController
	{
		Result AddScore(IScore score, Result aResult);
		Result UpdateScore(IScore score, Result aResult);
		Result DeleteScore(string scoreId, Result aResult);
		Result<SearchResult<IScoreSearchResult>> SearchScore(ScoreQuery query, Result<SearchResult<IScoreSearchResult>> aResult);
		string ToJson<T>(SearchResult<T> aSearchResult)where T : ISearchResultItem;
	}
}
