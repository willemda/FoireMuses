using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoireMuses.Core.Querys
{
	public class ScoreQuery
	{
		public string Title { get; set; }
		public string TitleWild { get; set; }
		public string Editor { get; set; }
		public string Composer { get; set; }
		public string Music { get; set; }
		public string Verses { get; set; }
		public string IsMaster { get; set; }
		public string MasterId { get; set; }
		public int Offset { get; set; }
		public int Max { get; set; }
	}
}
