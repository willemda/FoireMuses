using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveSeat;
using MindTouch.Tasking;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core.Utils;

namespace FoireMuses.Core.Controllers
{
	/// <summary>
	///  
	/// </summary>
	internal class CouchDBController : ICouchDBController
	{
		public CouchDatabase CouchDatabase { get; private set; }

		public CouchDBController()
		{
			CouchClient theClient = new CouchClient(Settings.Host, Settings.Port, Settings.Username, Settings.Password);
			/*Result<CouchDatabase> res = new Result<CouchDatabase>();
			theClient.GetDatabase(TheSettings.DatabaseName, res).WhenDone(
				a=>{theDatabase=a},*/
			CouchDatabase = theClient.GetDatabase(Settings.DatabaseName);
		}

	}
}
