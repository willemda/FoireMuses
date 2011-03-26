using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using MindTouch.Tasking;
using Newtonsoft.Json.Linq;

namespace FoireMuses.Core.Controllers
{
	class PlayController : IPlayController
	{


		public Result<IPlay> Create(IPlay aDoc, Result<IPlay> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<IPlay> Update(IPlay aDoc, Result<IPlay> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<IPlay> Get(IPlay aDoc, Result<IPlay> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<IPlay> Get(string id, Result<IPlay> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<bool> Delete(IPlay aDoc, Result<bool> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<SearchResult<IPlay>> GetAll(int offset, int max, Result<SearchResult<IPlay>> aResult)
		{
			throw new NotImplementedException();
		}
	}
}
