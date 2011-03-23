﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoireMuses.Core.Exceptions
{
    class EmptyFieldException : Exception
    {
        private string theFieldName;

        public EmptyFieldException(string theFieldName)
            : base(theFieldName)
        {
            this.theFieldName = theFieldName;
        }
    }
}
