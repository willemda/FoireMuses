using FoireMuses.Core.Business;
using MindTouch.Tasking;
using FoireMuses.Core;
using System;

namespace FoireMuses.ConsoleTest
{
	class Program
	{
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

			context.Instance.ScoreController.GetDocument((string)null, new Result<JScore>()).Wait();
		}

		
	}
}
