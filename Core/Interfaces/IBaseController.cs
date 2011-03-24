using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Tasking;
using Newtonsoft.Json.Linq;
using LoveSeat;

namespace FoireMuses.Core.Interfaces
{
    public interface IBaseController<T>
    {
        Result<T> Create(T aDoc, Result<T> aResult);
        Result<T> GetById(string id, Result<T> aResult);
        Result<T> Get(T aDoc, Result<T> aResult);
        Result<T> Update(T aDoc, Result<T> aResult);
        Result<JObject> Delete(T aDoc, Result<JObject> aResult);
    }
}
