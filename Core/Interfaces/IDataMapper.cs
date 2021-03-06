﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Tasking;
using Newtonsoft.Json.Linq;
using MindTouch.Xml;
using System.IO;

namespace FoireMuses.Core.Interfaces
{

	public class EventArgs<T> : EventArgs{
		public T Item{get;set;}
	}

	public interface INotificationManager
	{
		event EventHandler<EventArgs<IScore>> ScoreChanged;
		event EventHandler<EventArgs<IPlay>> PlayChanged;
		event EventHandler<EventArgs<ISource>> SourceChanged;
		event EventHandler<EventArgs<ISourcePage>> SourcePageChanged;
		void Start();
		void Stop();
	}

	public interface IDataMapper<T>
	{
		Result<T> Create(T aDocument, Result<T> aResult);
		Result<T> Retrieve(string aDocumentId, Result<T> aResult);
		Result<T> Update(string aDocumentId,string aRev,T aDocument, Result<T> aResult);
		Result<bool> Delete(string aDocumentId, string aRev, Result<bool> aResult);
		Result<SearchResult<T>> GetAll(int offset, int max, Result<SearchResult<T>> aResult);

		Result<bool> AddAttachment(string aDocumentId, Stream file, long anAttachmentLength, string fileName, Result<bool> aResult);
		Result<Stream> GetAttachment(string aDocumentId, string FileName, Result<Stream> aResult);
		Result<bool> DeleteAttachment(string aDocumentId, string aFileName, Result<bool> aResult);

		T CreateNew();

		T FromJson(string aJson);
		string ToJson(T anObject);
		string ToJson(SearchResult<T> aSearchResult);
	}

	/// <summary>
	/// A Interface that describes what a store controller must be able to do.
	/// </summary>
	public interface IScoreDataMapper : IDataMapper<IScore>
	{
		Result<SearchResult<IScore>> SearchScoreForText(int offset, int max, string textSearch, IScore aScore, Result<SearchResult<IScore>> aResult);
		Result<SearchResult<IScore>> SearchScoreForCode(int offset, int max, string code, IScore aScore, Result<SearchResult<IScore>> aResult);
		Result<SearchResult<IScore>> ScoresFromSource(int offset, int max, string aSourceId, Result<SearchResult<IScore>> aResult);
	}
	public interface IUserDataMapper : IDataMapper<IUser>
	{
		Result<SearchResult<IUser>> SearchUserForText(int offset,int max, string textSearch, IUser aUser, Result<SearchResult<IUser>> aResult);
	}

	public interface ISourceDataMapper : IDataMapper<ISource>
	{
	}

	public interface IPlayDataMapper : IDataMapper<IPlay>
	{
		Result<SearchResult<IPlay>> GetPlaysFromSource(int offset, int max, string aSourceId, Result<SearchResult<IPlay>> aResult);
	}

	public interface ISourcePageDataMapper : IDataMapper<ISourcePage>
	{
		Result<SearchResult<ISourcePage>> GetPagesFromSource(int offset, int max, string aSourceId, Result<SearchResult<ISourcePage>> aResult);
	}
}
