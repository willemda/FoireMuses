using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Tasking;

namespace FoireMuses.Core.Interfaces
{
	using Yield =IEnumerator<IYield>;

	public interface ISourcePageController : IBaseController<ISourcePage>
	{
		Result<bool> AddFascimile(string id, Stream file, Result<bool> aResult);
		Result<SearchResult<ISourcePage>> GetPagesFromSource(string sourceId, int offset, int max, Result<SearchResult<ISourcePage>> aResult);
		Result<bool> BulkImportSourcePages(string sourceId, Stream file, Result<bool> aResult);
	}
}
