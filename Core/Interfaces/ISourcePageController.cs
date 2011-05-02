using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Tasking;

namespace FoireMuses.Core.Interfaces
{
	using Yield = System.Collections.Generic.IEnumerator<MindTouch.Tasking.IYield>;

	public interface ISourcePageController : IBaseController<ISourcePage>
	{
		Result<SearchResult<ISourcePage>> GetPagesFromSource(int offset, int max, string sourceId, Result<SearchResult<ISourcePage>> aResult);
	}
}
