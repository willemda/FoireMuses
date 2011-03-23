using FoireMuses.Core.Business;
using MindTouch.Tasking;
using FoireMuses.Core;
using System;
using LoveSeat;
using Autofac.Builder;
using MindTouch.Xml;
using Newtonsoft.Json.Linq;

namespace FoireMuses.ConsoleTest
{
	class Program
	{
		private static readonly log4net.ILog theLogger = log4net.LogManager.GetLogger(typeof(Program));

		static void Main(string[] args)
		{
			InstanceFactory ifact = new InstanceFactory(new ContainerBuilder().Build(), new XDoc("config"));
			Context context = new Context(ifact.GetInstance(null, null)); // TODO 
			context.User = null;
			TaskEnv.ExecuteNew(() => StartTests(context));

			Console.WriteLine("Press enter to close");
			Console.ReadLine();
		}

		private static void StartTests(Context context)
		{
			context.AttachToCurrentTaskEnv();

			/*JUser user = new JUser();
			JArray arr = new JArray();
			arr.Add("group1");
			arr.Add("group2");
			user.Add("groupsId", arr);
			JToken tok;
			user.TryGetValue("groupsId", out tok);
			Console.WriteLine(user["groupsId"].Value<JArray>().First);*/

			//context.Instance.ViewController.createGetAllScoresView();
			//context.Instance.ViewController.createGetUserByUsernameView();
			//return;


			//context.Instance.ScoreController.GetDocument((string)null, new Result<JScore>()).Wait();
			JSource maSource = new JSource();
			maSource.Add("book", "livres très anciens page 8");
			maSource = context.Instance.SourceController.Create(maSource, new Result<JSource>()).Wait();
			theLogger.Debug(maSource.ToString());

			JSource maSource2 = new JSource();
			maSource2.Add("book", "livres très anciens page 99");
			maSource2 = context.Instance.SourceController.Create(maSource2, new Result<JSource>()).Wait();
			theLogger.Debug(maSource2.ToString());

			JScore monScore = new JScore();
			monScore.Add("title", "bel air de printemps");
			monScore.Add("source_id", maSource.Id);
			monScore = context.Instance.ScoreController.Create(monScore, new Result<JScore>()).Wait();
			theLogger.Debug(monScore.ToString());

			JScore monScore2 = new JScore();
			monScore2.Add("title", "bel air d'automne");
			monScore2.Add("source_id", maSource.Id);
			monScore2 = context.Instance.ScoreController.Create(monScore2, new Result<JScore>()).Wait();
			theLogger.Debug(monScore2.ToString());

			JScore monScore3 = new JScore();
			monScore3.Add("title", "bel air d'hiver");
			monScore3.Add("source_id", maSource2.Id);
			monScore3 = context.Instance.ScoreController.Create(monScore3, new Result<JScore>()).Wait();
			theLogger.Debug(monScore3.ToString());

			//only for testing and debug purposes
			context.Instance.ViewController.createScoresFromSourceView();
			context.Instance.ViewController.createGetAllView();

			ViewResult<string, string, JScore> maView;
			maView = context.Instance.ScoreController.GetScoresFromSource(maSource, new Result<ViewResult<string, string, JScore>>()).Wait();

			foreach (var row in maView.Rows)
			{
				theLogger.DebugFormat(
					"\nid : {0}" +
					"\nkey : {1}" +
					"\nvalue : {2}" +
					"\ndoc : {3}", row.Id, row.Key, row.Value, row.Doc
				);
			}

		}


	}
}
