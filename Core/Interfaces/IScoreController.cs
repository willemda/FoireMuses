using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Tasking;
using FoireMuses.Core.Business;
using LoveSeat;
using Newtonsoft.Json.Linq;

namespace FoireMuses.Core.Interfaces
{
	public interface IScoreController : IBaseController<IScore>
	{
		Result<SearchResult<IScore>> GetScoresFromSource(int offset, int max,ISource aJSource, Result<SearchResult<IScore>> aResult);
	}
}
