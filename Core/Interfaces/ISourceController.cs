using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Tasking;
using Newtonsoft.Json.Linq;
using System.IO;

namespace FoireMuses.Core.Interfaces
{
	public interface ISourceController : IBaseController<ISource>
	{
		Result<bool> BulkImportSourcePages(string sourceId, Stream file, Result<bool> aResult);
	}
}
