using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoireMuses.Core
{
	public class SearchResult<T> : IEnumerable<T>
	{
		public int Offset { get; private set; }
		public int Max { get; private set; }
		public int TotalCount { get; private set; }
		IEnumerable<T> Ienum{get; private set;}

		public SearchResult(IList<T> list, int offset, int max, int totalCount){
			Ienum = list.AsEnumerable();
			Offset = offset;
			Max = max;
			TotalCount = totalCount;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return Ienum.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return Ienum.GetEnumerator();
		}
	}
}
