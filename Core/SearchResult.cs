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

		private readonly IEnumerable<T> theResults;

		public SearchResult(IEnumerable<T> aList, int anOffset, int aMax, int aTotalCount)
		{
			theResults = aList;
			Offset = anOffset;
			Max = aMax;
			TotalCount = aTotalCount;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return theResults.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return theResults.GetEnumerator();
		}
	}
}
