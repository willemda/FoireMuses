using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using MindTouch.Tasking;
using Newtonsoft.Json.Linq;

namespace FoireMuses.Core.Controllers
{
	public class PlayController : IPlayController
	{

		private static readonly log4net.ILog theLogger = log4net.LogManager.GetLogger(typeof(ScoreController));

		private readonly IPlayDataMapper thePlayDataMapper;

		public PlayController(IPlayDataMapper aController)
		{
			thePlayDataMapper = aController;
		}
		public Result<IPlay> Create(IPlay aDoc, Result<IPlay> aResult)
		{
			if (Context.Current.User == null)
				throw new UnauthorizedAccessException();
			thePlayDataMapper.Create(aDoc, new Result<IPlay>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IPlay> Update(string id,string rev, IPlay aDoc, Result<IPlay> aResult)
		{
			if (Context.Current.User == null)
				throw new UnauthorizedAccessException();
			thePlayDataMapper.Update(id, rev, aDoc, new Result<IPlay>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<IPlay> Retrieve(string id, Result<IPlay> aResult)
		{
			thePlayDataMapper.Retrieve(id, new Result<IPlay>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<bool> Delete(string id, string rev, Result<bool> aResult)
		{
			thePlayDataMapper.Delete(id, rev, new Result<bool>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<SearchResult<IPlay>> GetAll(int offset, int max, Result<SearchResult<IPlay>> aResult)
		{
			throw new NotImplementedException();
		}

		public IPlay FromJson(string aJson)
		{
			return thePlayDataMapper.FromJson(aJson);
		}

		public string ToJson(IPlay anObject)
		{
			return thePlayDataMapper.ToJson(anObject);
		}


		public IPlay FromXml(MindTouch.Xml.XDoc aXml)
		{
			throw new NotImplementedException();
		}

		public MindTouch.Xml.XDoc ToXml(IPlay anObject)
		{
			throw new NotImplementedException();
		}


		public Result<bool> Exists(string id, Result<bool> aResult)
		{
			this.Retrieve(id, new Result<IPlay>()).WhenDone(
				a =>
				{
					if (a != null)
						aResult.Return(true);
					else
						aResult.Return(false);
				},
				aResult.Throw
				);
			return aResult;
		}

		public Result<SearchResult<IPlay>> GetPlaysFromSource(int offset, int max, string sourceId, Result<SearchResult<IPlay>> aResult)
		{
			thePlayDataMapper.GetPlaysFromSource(offset, max, sourceId, new Result<SearchResult<IPlay>>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}
	}
}
