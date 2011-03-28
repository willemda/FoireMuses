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

		public Result<IPlay> Update(string id,string rev, IPlay aDoc, Result<IPlay> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<IPlay> Retrieve(string id, Result<IPlay> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<bool> Delete(string id, string rev, Result<bool> aResult)
		{
			throw new NotImplementedException();
		}

		public Result<SearchResult<IPlay>> GetAll(int offset, int max, Result<SearchResult<IPlay>> aResult)
		{
			throw new NotImplementedException();
		}

		public IPlay FromJson(string aJson)
		{
			throw new NotImplementedException();
		}

		public string ToJson(IPlay aJson)
		{
			throw new NotImplementedException();
		}


		public IPlay FromXml(MindTouch.Xml.XDoc aXml)
		{
			throw new NotImplementedException();
		}

		public MindTouch.Xml.XDoc ToXml(IPlay anObject)
		{
			throw new NotImplementedException();
		}
	}
}
