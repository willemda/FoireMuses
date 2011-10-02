using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DreamSeat;
using MindTouch.Tasking;
using System.IO;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Interfaces;

namespace FoireMuses.Core.Loveseat
{
	public abstract class BaseDataMapper<T>
	{
		protected CouchDatabase CouchDatabase { get; private set; }
		protected CouchClient CouchClient { get; private set; }

		protected BaseDataMapper(ISettingsController aSettingsController)
		{
			CouchClient = new CouchClient(aSettingsController.Host, aSettingsController.Port, aSettingsController.Username, aSettingsController.Password);
			CouchDatabase = CouchClient.GetDatabase(aSettingsController.DatabaseName);
		}

		public string ToJson(SearchResult<T> aSearchResult)
		{
			JObject json = new JObject { { "total_rows", aSearchResult.TotalCount }, { "offset", aSearchResult.Offset }, { "max", aSearchResult.Max } };
			JArray docs = new JArray();
			foreach (T doc in aSearchResult)
			{
				docs.Add(doc);
			}
			json.Add("rows", docs);
			return json.ToString();
		}

		public string ToJson(T anObject)
		{
			return anObject.ToString();
		}

		public Result<bool> AddAttachment(string id, Stream file, long anAttachmentLength, string fileName, Result<bool> aResult)
		{
			CouchDatabase.AddAttachment(id, file, anAttachmentLength, fileName, new Result<JObject>()).WhenDone(
				a => aResult.Return(true),
				aResult.Throw
				);
			return aResult;
		}

		public Result<Stream> GetAttachment(string id, string fileName, Result<Stream> aResult)
		{
			CouchDatabase.GetAttachment(id, fileName, new Result<Stream>()).WhenDone(
				aResult.Return,
				aResult.Throw
				);
			return aResult;
		}

		public Result<bool> DeleteAttachment(string aDocumentId, string aFileName, Result<bool> aResult)
		{
			CouchDatabase.DeleteAttachment(aDocumentId, aFileName, new Result<JObject>()).WhenDone(
				a => aResult.Return(true),
				aResult.Throw
				);
			return aResult;
		}
	}
}
