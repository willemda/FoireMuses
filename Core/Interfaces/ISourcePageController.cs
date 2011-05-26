using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Tasking;

namespace FoireMuses.Core.Interfaces
{
	using Yield = System.Collections.Generic.IEnumerator<MindTouch.Tasking.IYield>;
using System.IO;

	public interface ISourcePageController : IBaseController<ISourcePage>
	{
		Result<bool> AddFascimile(string id, Stream file, Result<bool> aResult);
		Result<SearchResult<ISourcePage>> GetPagesFromSource(int offset, int max, string sourceId, Result<SearchResult<ISourcePage>> aResult);
	}
}
