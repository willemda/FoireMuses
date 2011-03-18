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
        public CouchClient CouchClient { get; private set; }

        public CouchDBController()
        {
            CouchClient = new CouchClient(Settings.Host, Settings.Port, Settings.Username, Settings.Password);
            /*Result<CouchDatabase> res = new Result<CouchDatabase>();
            theClient.GetDatabase(TheSettings.DatabaseName, res).WhenDone(
                a=>{theDatabase=a},*/
            // async needed? it's only done one time I think so it's not very usefull.
            //TODO: the database deletion is only for testing facility, remove it!
            if (CouchClient.HasDatabase(Settings.DatabaseName))
            {
                //CouchClient.DeleteDatabase(Settings.DatabaseName);
            }
            CouchDatabase = CouchClient.GetDatabase(Settings.DatabaseName);
        }

	}
}
