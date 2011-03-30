using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Tasking;

namespace FoireMuses.Core.Interfaces
{
	public interface IScoreController : IBaseController<IScore>
	{
		Result<SearchResult<IScore>> GetScoresFromSource(int offset, int max,string aSourceId, Result<SearchResult<IScore>> aResult);
	}
}
