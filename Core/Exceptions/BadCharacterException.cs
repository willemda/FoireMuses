using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoireMuses.Core.Exceptions
{
    public class BadCharacterException : Exception
    {
        private string theFieldName;

        public BadCharacterException(string theFieldName)
            : base(theFieldName)
        {
            this.theFieldName = theFieldName;
        }
    }
}
