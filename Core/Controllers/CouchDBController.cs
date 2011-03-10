using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveSeat;
using MindTouch.Tasking;
using Newtonsoft.Json.Linq;
using Core.Interfaces;
using Core.Settings;

namespace Core.Controllers
{
	/// <summary>
	///  
	/// </summary>
    internal class CouchDBController : ICouchDBController
    {
         public CouchDatabase MyCouchDatabase { get;private set;}
		
		
		public CouchDBController ()
		{
			CouchClient theClient = new CouchClient (TheSettings.Host, TheSettings.Port, TheSettings.Username, TheSettings.Password);
			/*Result<CouchDatabase> res = new Result<CouchDatabase>();
			theClient.GetDatabase(TheSettings.DatabaseName, res).WhenDone(
				a=>{theDatabase=a},*/
			MyCouchDatabase = theClient.GetDatabase(TheSettings.DatabaseName);
		}
		
    }
}
