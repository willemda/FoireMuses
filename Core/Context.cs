using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Dream;
using MindTouch.Tasking;

namespace FoireMuses.Core
{
	public class Context : ITaskLifespan
	{
		private static readonly log4net.ILog theLogger = log4net.LogManager.GetLogger(typeof(Context));

		public static Context Current
		{
			get
			{
				TaskEnv current = TaskEnv.CurrentOrNull;
				if (current == null)
				{
					throw new DreamContextAccessException("Context.Current is not set because there is no task environment.");
				}

				Context context = current.GetState<Context>();
				if (context == null)
				{
					throw new DreamContextAccessException("Context.Current is not set because the current task environment does not contain a reference.");
				}
				if (context.isTaskDisposed)
				{
					throw new DreamContextAccessException("Context.Current is not set because the current context is already disposed.");
				}
				return context;
			}
		}

		//TODO : Specify a Type for the user.
		public object User { get; set; }
		public Instance Instance { get; private set; }

		private TaskEnv theOwnerEnv;
		private bool isTaskDisposed;

		public Context(Instance anInstance)
		{
			theLogger.Debug("Creating Context");
			Instance = anInstance;
		}

		public void AttachToCurrentTaskEnv()
		{
			theLogger.Debug("Attaching Context to current TaskEnv");
			lock (this)
			{
				var env = TaskEnv.Current;
				if (env.GetState<Context>() != null)
				{
					throw new DreamContextAccessException("tried to attach context to env that already has a dreamcontext");
				}
				if (theOwnerEnv != null && theOwnerEnv == env)
				{
					throw new DreamContextAccessException("tried to re-attach dreamcontext to env it is already attached to");
				}
				if (theOwnerEnv != null)
				{
					throw new DreamContextAccessException("tried to attach dreamcontext to an env, when it already is attached to another");
				}
				theOwnerEnv = env;
				env.SetState(this);
			}
		}

		public object Clone()
		{
			return new Context(Instance) {User = User};
		}

		public void Dispose()
		{
			if (isTaskDisposed)
			{
				Console.WriteLine("disposing already disposed context");
			}

			isTaskDisposed = true;
		}
	}
}
