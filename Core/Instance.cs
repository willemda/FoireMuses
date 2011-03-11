using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core.Controllers;

namespace FoireMuses.Core
{
	/// <summary>
	/// Contains a reference to the Controllers
	/// </summary>
	public class Instance
	{
		internal ICouchDBController CouchDbController { get; private set; }
		public IScoreController ScoreController { get; private set; }
        public ISourceController SourceController { get; private set; }
        public ViewController ViewController { get; private set; }

		public  Instance()
		{
			CouchDbController = new CouchDBController();
			ScoreController = new ScoreController();
            SourceController = new SourceController();
            ViewController = new ViewController();

		}
	}
}
