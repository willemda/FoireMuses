using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;

namespace FoireMuses.UnitTests.Mock
{
    public class MockPlayController : IPlayController
    {
        public MindTouch.Tasking.Result<Core.SearchResult<IPlay>> GetPlaysFromSource(int offset, int max, string sourceId, MindTouch.Tasking.Result<Core.SearchResult<IPlay>> aResult)
        {
            throw new NotImplementedException();
        }

        public IPlay FromJson(string aJson)
        {
            throw new NotImplementedException();
        }

        public string ToJson(IPlay anObject)
        {
            throw new NotImplementedException();
        }

        public string ToJson(Core.SearchResult<IPlay> aSearchResult)
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

        public IPlay CreateNew()
        {
            throw new NotImplementedException();
        }

        public MindTouch.Tasking.Result<IPlay> Create(IPlay aDoc, MindTouch.Tasking.Result<IPlay> aResult)
        {
            throw new NotImplementedException();
        }

        public MindTouch.Tasking.Result<IPlay> Update(string id, string rev, IPlay aDoc, MindTouch.Tasking.Result<IPlay> aResult)
        {
            throw new NotImplementedException();
        }

        public MindTouch.Tasking.Result<IPlay> Retrieve(string id, MindTouch.Tasking.Result<IPlay> aResult)
        {
            throw new NotImplementedException();
        }

        public MindTouch.Tasking.Result<bool> Delete(string id, string rev, MindTouch.Tasking.Result<bool> aResult)
        {
            throw new NotImplementedException();
        }

        public MindTouch.Tasking.Result<bool> Exists(string id, MindTouch.Tasking.Result<bool> aResult)
        {
            throw new NotImplementedException();
        }

        public MindTouch.Tasking.Result<Core.SearchResult<IPlay>> GetAll(int offset, int max, MindTouch.Tasking.Result<Core.SearchResult<IPlay>> aResult)
        {
            throw new NotImplementedException();
        }
    }
}
