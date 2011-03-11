using FoireMuses.Core.Business;
using MindTouch.Tasking;
using FoireMuses.Core;
using System;
using LoveSeat;

namespace FoireMuses.ConsoleTest
{
	class Program
	{
        private static readonly log4net.ILog theLogger = log4net.LogManager.GetLogger(typeof(Program));

		static void Main(string[] args)
		{
			Context context = new Context(new Instance());
			context.User = "coucou";
			TaskEnv.ExecuteNew(() => StartTests(context));

			Console.WriteLine("Press enter to close");
			Console.ReadLine();
		}

		private static void StartTests(Context context)
		{
			context.AttachToCurrentTaskEnv();


            

			//context.Instance.ScoreController.GetDocument((string)null, new Result<JScore>()).Wait();
            JSource maSource = new JSource();
            maSource.Add("livre","livres très anciens page 8");
            maSource = context.Instance.SourceController.Create(maSource, new Result<JSource>()).Wait();
            theLogger.Debug(maSource.ToString());
            
            JSource maSource2 = new JSource();
            maSource2.Add("livre", "livres très anciens page 99");
            maSource2 = context.Instance.SourceController.Create(maSource2, new Result<JSource>()).Wait();
            theLogger.Debug(maSource.ToString());

            JScore monScore = new JScore();
            monScore.Add("titre", "bel air de printemps");
            monScore.Add("source_id", maSource.Id);
            monScore = context.Instance.ScoreController.Create(monScore, new Result<JScore>()).Wait();
            theLogger.Debug(monScore.ToString());

            JScore monScore2 = new JScore();
            monScore2.Add("titre", "bel air de printemps");
            monScore2.Add("source_id", maSource2.Id);
            monScore2 = context.Instance.ScoreController.Create(monScore2, new Result<JScore>()).Wait();
            theLogger.Debug(monScore2.ToString());

            //only for testing and debug purposes
            context.Instance.ViewController.createScoresFromSourceView();

            ViewResult<string, string, JScore> maView;
            maView = context.Instance.ScoreController.GetScoresFromSource(maSource,new Result<ViewResult<string, string, JScore>>()).Wait();
            theLogger.Debug(maView.ToString());

		}

		
	}
}
