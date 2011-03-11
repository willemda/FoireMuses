using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveSeat;
using FoireMuses.Core.Interfaces;
using Newtonsoft.Json.Linq;

namespace FoireMuses.Core.Business
{
    public abstract class Document : JDocument,IDocument
    {
        public Document (){}

        public Document(JObject jobject) : base(jobject) { }

        public abstract void Validate();
    }
}
