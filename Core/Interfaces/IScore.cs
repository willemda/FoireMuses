using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoireMuses.Core.Interfaces
{
	public interface IScore
	{
		string Id { get; set; }
		string Rev { get; set;}

		string Title{get;set;}
		string CodageMelodiqueRISM { get; set; }
		string CodageParIntervalles { get; set; }
        string CodageRythmique { get; set; }
		string Coirault { get; set; }
		string Composer { get; set; }
		string CoupeMetrique { get; set; }
		string Verses { get; set; }
		string Delarue { get; set; }
		string Comments { get; set; }
		string Editor { get; set; }
		string RythmSignature { get; set; }
		string OtherTitles { get; set; }
		string Stanza { get; set; }
		string ScoreType { get; set; }

		IMusicalSource MusicalSource { get; set; }

		ITextualSource TextualSource { get; set; }

		//TODO: replace by list
		IEnumerable<string> Tags { get; }
		void AddTag(string tag);
		void RemoveTag(string tag);

		string CreatorId { get; }
		string LastModifierId { get; }

		//TODO: replace by list
		IEnumerable<string> CollaboratorsId { get; }
		void AddCollaborator(string collab);
		void RemoveCollaborator(string collab);
	}
}
