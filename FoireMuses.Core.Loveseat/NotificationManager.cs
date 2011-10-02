using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core.Interfaces;
using DreamSeat;
using System.IO;
using DreamSeat.Interfaces;
using FoireMuses.Core.Business;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Loveseat.Business;
using MindTouch.Tasking;

namespace FoireMuses.Core.Loveseat
{
	public class NotificationManager: INotificationManager
	{
		public event EventHandler<EventArgs<IScore>> ScoreChanged;

		public event EventHandler<EventArgs<IPlay>> PlayChanged;

		public event EventHandler<EventArgs<ISource>> SourceChanged;

		public event EventHandler<EventArgs<ISourcePage>> SourcePageChanged;

		private CouchContinuousChanges<JDocument> Changes;
		private ChangeOptions theOptions;
		private readonly CouchDatabase theCouchDatabase;
		private readonly CouchClient theCouchClient;

		public NotificationManager(ISettingsController aSettingsController){
			theCouchClient = new CouchClient(aSettingsController.Host, aSettingsController.Port, aSettingsController.Username, aSettingsController.Password);
			theCouchDatabase = theCouchClient.GetDatabase(aSettingsController.DatabaseName);
			theOptions = new ChangeOptions();
			theOptions.Heartbeat = 10000;
			theOptions.Since = GetSequence();
		}

		public void Start(){
				theCouchDatabase.GetCoutinuousChanges<JDocument>(theOptions, new CouchChangeDelegate<JDocument>(OnChanged), new Result<CouchContinuousChanges<JDocument>>());
		}

		private void OnChanged(object sender, CouchChangeResult<JDocument> aChange){
			SetSequence(aChange.Sequence);
			JToken jtoken;
			aChange.Doc.TryGetValue("otype", out jtoken);
			if(jtoken == null)
				return;
			switch (jtoken.Value<string>())
			{
				case "score":
					if (ScoreChanged != null)
					{
						ScoreChanged(this, new EventArgs<IScore> { Item = new JScore(aChange.Doc) });
					}
					break;
				case "play":
					if (PlayChanged != null)
					{
						PlayChanged(this, new EventArgs<IPlay> { Item = new JPlay(aChange.Doc) });
					}
					break;
				case "source":
					if (SourceChanged != null)
					{
						 SourceChanged(this, new EventArgs<ISource> { Item = new JSource(aChange.Doc) });
					}
					break;
				case "sourcePage":
					if (SourcePageChanged != null)
					{
						SourcePageChanged(this, new EventArgs<ISourcePage> { Item = new JSourcePage(aChange.Doc) });
					}
					break;
			}
			
		}

		public void Stop(){
		}

		private int GetSequence()
		{
			int seqNumber = 1;
			try
			{
				if (File.Exists("sequence.txt"))
				{
					seqNumber = Int32.Parse(File.ReadAllText("sequence.txt"));
					return seqNumber;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("There was a problem while reading the sequence number from sequence.txt\n" + e);
				return seqNumber;
			}
			return seqNumber;
		}
		private void SetSequence(int seq)
		{
			try
			{
				File.WriteAllText("sequence.txt", seq.ToString());
			}
			catch (Exception e)
			{
				Console.WriteLine("There was a problem while writing the sequence number to sequence.txt\n" + e);
			}
		}
	}
}
