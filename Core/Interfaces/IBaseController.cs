using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Tasking;

namespace FoireMuses.Core.Interfaces
{
	public interface IBaseController<T>
	{
		Result<T> Create(T aDoc, Result<T> aResult);
		Result<T> Update(T aDoc, Result<T> aResult);
		Result<T> Get(T aDoc, Result<T> aResult);
		Result<T> Get(string id, Result<T> aResult);
		Result<bool> Delete(T aDoc, Result<bool> aResult);
		Result<SearchResult<T>> GetAll(int offset, int max, Result<SearchResult<T>> aResult);
	}
}
