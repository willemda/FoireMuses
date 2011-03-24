using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core.Business;
using FoireMuses.Core.Utils;
using LoveSeat;
using MindTouch.Tasking;
using LoveSeat.Support;
using Newtonsoft.Json.Linq;

namespace FoireMuses.Core.Controllers
{
	class PlayController : IPlayController
	{


		public Result<JPlay> Create(JPlay aDoc, Result<JPlay> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<JPlay> Update(JPlay aDoc, Result<JPlay> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<JPlay> Get(JPlay aDoc, Result<JPlay> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<JPlay> Get(string id, Result<JPlay> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<bool> Delete(JPlay aDoc, Result<bool> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<SearchResult<JPlay>> GetAll(int offset, int max, Result<SearchResult<JPlay>> aResult)
		{
			throw new NotImplementedException();
		}
	}
}
