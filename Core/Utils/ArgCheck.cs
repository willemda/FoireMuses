using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MindTouch.Tasking;

namespace FoireMuses.Core.Utils
{
	class ArgCheck
	{
		public static void NotNull(string paramName, object o)
		{
			if (o == null)
				throw new ArgumentNullException(paramName);
		}

		public static void NotNullNorEmpty(string paramName, string str)
		{
			if (String.IsNullOrEmpty(str))
			{
				throw new ArgumentException(paramName);
			}
		}

		// For asynchronous methods, you have to throw exception using the Result object

		//public static void NotNull(object o, string paramName)
		//{
		//    NotNull(o, paramName, null);
		//}
		//public static void NotNull(object o, string paramName, AResult aResult)
		//{
		//    if (o == null)
		//    {
		//        ArgumentNullException ex = new ArgumentNullException(paramName);
		//        if (aResult != null)
		//            aResult.Throw(ex);
		//        else
		//            throw ex;
		//    }
		//}
		//public static void NotNullNorEmpty(string str, string paramName)
		//{
		//    NotNullNorEmpty(str, paramName);
		//}
		//public static void NotNullNorEmpty(string str, string paramName, AResult aResult)
		//{
		//    if (String.IsNullOrEmpty(str))
		//    {
		//        ArgumentNullException ex = new ArgumentNullException(paramName);
		//        if (aResult != null)
		//            aResult.Throw(ex);
		//        else
		//            throw ex;
		//    }
		//}
	}
}
