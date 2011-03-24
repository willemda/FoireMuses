using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core.Business;
using LoveSeat;
using LoveSeat.Support;
using FoireMuses.Core.Utils;
using MindTouch.Tasking;
using Newtonsoft.Json.Linq;

namespace FoireMuses.Core.Controllers
{
	class SourceController : ISourceController
	{


		public Result<JSource> Create(JSource aDoc, Result<JSource> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<JSource> Update(JSource aDoc, Result<JSource> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<JSource> Get(JSource aDoc, Result<JSource> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<JSource> Get(string id, Result<JSource> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<bool> Delete(JSource aDoc, Result<bool> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<SearchResult<JSource>> GetAll(int offset, int max, Result<SearchResult<JSource>> aResult)
		{
			throw new NotImplementedException();
		}
	}
}
