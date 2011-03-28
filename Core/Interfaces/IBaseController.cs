﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Tasking;
using MindTouch.Xml;

namespace FoireMuses.Core.Interfaces
{
	public interface IBaseController<T>
	{
		T FromJson(string aJson);
		string ToJson(T aJson);

		T FromXml(XDoc aXml);
		XDoc ToXml(T anObject);


		Result<T> Create(T aDoc, Result<T> aResult);
		Result<T> Update(string id, string rev, T aDoc, Result<T> aResult);
		Result<T> Retrieve(string id, Result<T> aResult);
		Result<bool> Delete(string id, string rev, Result<bool> aResult);
		Result<SearchResult<T>> GetAll(int offset, int max, Result<SearchResult<T>> aResult);
	}
}
