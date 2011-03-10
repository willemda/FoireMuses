using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Utils
{
    class Utils
    {
        public static void CheckObject(string paramName,object o)
        {
            if (o == null)
                throw new ArgumentNullException(paramName);
        }


        public static void CheckString(string paramName, string str)
        {
            if(String.IsNullOrEmpty(str))
            {
                throw new ArgumentException(paramName);
            }
        }
    }
}
