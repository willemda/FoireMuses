using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Tasking;
using MindTouch.Xml;
using System.IO;

namespace FoireMuses.Core.Interfaces
{
	/// <summary>
	/// Main function to persist and retrieve objects in DataStore
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IBaseController<T>
	{
		/// <summary>
		/// Insert an Object from json
		/// </summary>
		/// <param name="aJson">json string</param>
		/// <returns>created object</returns>
		T FromJson(string aJson);
		
		/// <summary>
		/// Convert object to json
		/// </summary>
		/// <param name="anObject">an object</param>
		/// <returns>json encoded object</returns>
		string ToJson(T anObject);

		/// <summary>
		/// Convert a SearchResult of object to json
		/// </summary>
		/// <param name="aSearchResult">SearchResult to convert</param>
		/// <returns>json encoded SearchResult</returns>
		string ToJson(SearchResult<T> aSearchResult);

		/// <summary>
		/// Insert new instance of object
		/// </summary>
		/// <returns></returns>
		T CreateNew();

		/// <summary>
		/// Insert object in DataStore
		/// </summary>
		/// <param name="aDoc">aDocument to insert</param>
		/// <param name="aResult">a Result</param>
		/// <returns></returns>
		Result<T> Insert(T aDoc, Result<T> aResult);

		/// <summary>
		/// Update a document in DataStore
		/// </summary>
		/// <param name="aDocumentId">Id of the document to update</param>
		/// <param name="aDocumentRevision">Revision of the document</param>
		/// <param name="aDocument">Document to update</param>
		/// <param name="aResult">a Result</param>
		/// <returns></returns>
		Result<T> Update(string aDocumentId, string aDocumentRevision, T aDocument, Result<T> aResult);

		/// <summary>
		/// Return an object base on id
		/// </summary>
		/// <param name="aDocumentId">Id of the document</param>
		/// <param name="aResult">a Result</param>
		/// <returns></returns>
		Result<T> Retrieve(string aDocumentId, Result<T> aResult);

		/// <summary>
		/// Delete a document
		/// </summary>
		/// <param name="aDocumentId">Id of the document to update</param>
		/// <param name="aDocumentRevision">Revision of the document</param>
		/// <param name="aResult">a Result</param>
		/// <returns></returns>
		Result<bool> Delete(string aDocumentId, string aDocumentRevision, Result<bool> aResult);

		/// <summary>
		/// Check if a document exists
		/// </summary>
		/// <param name="aDocumentId">Id of the document to update</param>
		/// <param name="aResult">a Result</param>
		/// <returns></returns>
		Result<bool> Exists(string aDocumentId, Result<bool> aResult);

		/// <summary>
		/// Return list of objects
		/// </summary>
		/// <param name="anOffset">an Offset</param>
		/// <param name="aMax">a maximum number of item to return</param>
		/// <param name="aResult">a Result</param>
		/// <returns></returns>
		Result<SearchResult<T>> GetAll(int anOffset, int aMax, Result<SearchResult<T>> aResult);

		/// <summary>
		/// Add an Attachment to the document
		/// </summary>
		/// <param name="aDocumentId">id of the document</param>
		/// <param name="aStream">Stream to the file to attach</param>
		/// <param name="aFileName">File Name</param>
		/// <param name="aResult">a Result</param>
		/// <returns></returns>
		Result<bool> AddAttachment(string aDocumentId, Stream aStream, string aFileName, Result<bool> aResult);

		/// <summary>
		/// Retrieve an attachment
		/// </summary>
		/// <param name="aDocumentId">Id of the document</param>
		/// <param name="aFileName">File Name</param>
		/// <param name="aResult">a Result</param>
		/// <returns></returns>
		Result<Stream> GetAttachment(string aDocumentId, string aFileName, Result<Stream> aResult);

		/// <summary>
		/// Delete attachment
		/// </summary>
		/// <param name="aDocumentId">Id of the document</param>
		/// <param name="aFileName">File Name</param>
		/// <param name="aResult">a Result</param>
		/// <returns></returns>
		Result<bool> DeleteAttachment(string aDocumentId, string aFileName, Result<bool> aResult);
	}
}
