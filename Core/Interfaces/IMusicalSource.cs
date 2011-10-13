using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoireMuses.Core.Interfaces
{
	public interface IMusicalSource : ISourceReference
	{
		bool IsSuggested { get; set; }
	}
}
