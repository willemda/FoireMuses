using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoireMuses.Core.Interfaces
{
	public interface ITextualSource : ISourceReference
	{
		string PieceId { get; set; }
		int? SceneNumber { get; set; }
		int? ActNumber { get; set; }
	}
}
