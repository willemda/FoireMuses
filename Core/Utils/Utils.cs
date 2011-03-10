using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Exceptions
{
    class Utils
    {
        public static void CheckObject(object o)
        {
            if (o == null)
                throw new ArgumentNullException();
        }


        public static void CheckString(string str)
        {
            CheckObject(str);
            Regex aRegex = new Regex("^.*$");
            if (!aRegex.Match(str).Success)
            {
                throw new ArgumentInvalideException();
            }
        }
    }
}
