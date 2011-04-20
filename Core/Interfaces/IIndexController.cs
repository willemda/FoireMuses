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
		Result DeleteScore(IScore score, Result aResult);
		Result<SearchResult<ScoreSearchResult>> SearchScore(ScoreQuery query, Result<SearchResult<ScoreSearchResult>> aResult);
	}
}
