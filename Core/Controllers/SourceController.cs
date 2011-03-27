using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core.Utils;
using MindTouch.Tasking;
using Newtonsoft.Json.Linq;

namespace FoireMuses.Core.Controllers
{
	class SourceController : ISourceController
	{
		public Result<ISource> Create(ISource aDoc, Result<ISource> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<ISource> Update(string id,string rev,ISource aDoc, Result<ISource> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<ISource> Retrieve(string id, Result<ISource> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<bool> Delete(string id, Result<bool> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<SearchResult<ISource>> GetAll(int offset, int max, Result<SearchResult<ISource>> aResult)
		{
			throw new NotImplementedException();
		}

		public ISource FromJson(string aJson)
		{
			throw new NotImplementedException();
		}

		public string ToJson(ISource aJson)
		{
			throw new NotImplementedException();
		}
	}
}
