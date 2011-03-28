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
	public class SourceController : ISourceController
	{
		private static readonly log4net.ILog theLogger = log4net.LogManager.GetLogger(typeof(ScoreController));

		private readonly ISourceDataMapper theSourceDataMapper;

		public SourceController(ISourceDataMapper aController)
		{
			theSourceDataMapper = aController;
		}

		public Result<ISource> Create(ISource aDoc, Result<ISource> aResult)
		{
			theSourceDataMapper.Create(aDoc, new Result<ISource>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<ISource> Update(string id,string rev,ISource aDoc, Result<ISource> aResult)
		{
			theSourceDataMapper.Update(id, rev, aDoc, new Result<ISource>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<ISource> Retrieve(string id, Result<ISource> aResult)
		{
			theSourceDataMapper.Retrieve(id, new Result<ISource>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<bool> Delete(string id, string rev, Result<bool> aResult)
		{
			theSourceDataMapper.Delete(id, rev, new Result<bool>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<SearchResult<ISource>> GetAll(int offset, int max, Result<SearchResult<ISource>> aResult)
		{
			throw new NotImplementedException();
		}

		public ISource FromJson(string aJson)
		{
			return theSourceDataMapper.FromJson(aJson);
		}

		public string ToJson(ISource aJson)
		{
			return theSourceDataMapper.ToJson(aJson);
		}
	}
}
