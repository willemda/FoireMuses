using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoireMuses.Core.Interfaces
{
	public interface IScore
	{
		string Id { get; set; }
		string Rev { get; }

		string Title{get;set;}
		string Code1 { get; set; }
		string Code2 { get; set; }
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

		string SourceMusicaleId { get; set; }
		int SourceMusicaleActNumber { get; set; }
		string SourceMusicaleActId { get; set; }
		int SourceMusicaleSceneNumber { get; set; }
		string SourceMusicaleSceneId { get; set; }
		string SourceMusicaleText { get; set; }
		bool SourceMusicaleIsSuggested { get; set; }

		string SourceTextuelleId { get; set; }
		int SourceTextuelleAir { get; set; }
		string SourceTextuellePage { get; set; }
		int SourceTextuelleTome { get; set; }
		int SourceTextuelleVolume { get; set; }

		IEnumerable<string> Tags { get; }
		void AddTag(string tag);
		void RemoveTag(string tag);

		string CreatorId { get; }
		string LastModifierId { get; }

		IEnumerable<string> CollaboratorsId { get; }
		void AddCollaborator(string collab);
		void RemoveCollaborator(string collab);
	}
}
