using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Tasking;
using MindTouch.Xml;
using System.IO;
using MindTouch.Dream;

namespace FoireMuses.Core.Interfaces
{
	public interface IScoreController : IBaseController<IScore>
	{
		Result<bool> AddAttachment(string aScoreId, Stream file, string fileName, Result<bool> aResult);
		Result<IScore> AttachMusicXml(IScore score, XDoc xdoc, bool overwriteMusicXmlValues, Result<IScore> aResult);
		Result<SearchResult<IScore>> GetScoresFromSource(int offset, int max,string aSourceId, Result<SearchResult<IScore>> aResult);
		Result<Stream> GetAttachedFile(string scoreId, string fileName, Result<Stream> aResult);
		Result<Stream> GetConvertedScore(MimeType mimetype, string id, Result<Stream> aResult);
	}
}
