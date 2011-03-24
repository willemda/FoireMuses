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

		}


	}
}
