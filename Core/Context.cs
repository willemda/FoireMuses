using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindTouch.Dream;

namespace Core
{
	public class Context
	{
		public static Context Current
		{
			get
			{
				Context dc = CurrentOrNull;
				if (dc == null)
				{
					throw new InvalidOperationException("A247Context.Current is not set");
				}
				return dc;
			}
		}
		public static Context CurrentOrNull
		{
			get
			{
				DreamContext dreamContext = DreamContext.Current;
				var context = dreamContext.GetState<Context>();
				if (context == null)
				{
					throw new Exception("Invalid context");
				}
				return context;
			}
		}

		//TODO : Specify a Type for the user.
		public object User { get; set; }
		public Instance Instance { get; private set; }

		public Context(Instance anInstance)
		{
			Instance = anInstance;
		}
	}
}
