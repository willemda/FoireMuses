using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;

namespace FoireMuses.UnitTests.Mock
{
    public class MockSourceController : ISourceController
    {
        public ISource FromJson(string aJson)
        {
            throw new NotImplementedException();
        }

        public string ToJson(ISource anObject)
        {
            throw new NotImplementedException();
        }

        public string ToJson(Core.SearchResult<ISource> aSearchResult)
        {
            throw new NotImplementedException();
        }

        public ISource FromXml(MindTouch.Xml.XDoc aXml)
        {
            throw new NotImplementedException();
        }

        public MindTouch.Xml.XDoc ToXml(ISource anObject)
        {
            throw new NotImplementedException();
        }

        public ISource CreateNew()
        {
            throw new NotImplementedException();
        }

        public MindTouch.Tasking.Result<ISource> Create(ISource aDoc, MindTouch.Tasking.Result<ISource> aResult)
        {
            throw new NotImplementedException();
        }

        public MindTouch.Tasking.Result<ISource> Update(string id, string rev, ISource aDoc, MindTouch.Tasking.Result<ISource> aResult)
        {
            throw new NotImplementedException();
        }

        public MindTouch.Tasking.Result<ISource> Retrieve(string id, MindTouch.Tasking.Result<ISource> aResult)
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

        public MindTouch.Tasking.Result<Core.SearchResult<ISource>> GetAll(int offset, int max, MindTouch.Tasking.Result<Core.SearchResult<ISource>> aResult)
        {
            throw new NotImplementedException();
        }
    }
}
