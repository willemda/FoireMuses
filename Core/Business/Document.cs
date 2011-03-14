using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveSeat;
using FoireMuses.Core.Interfaces;
using Newtonsoft.Json.Linq;

namespace FoireMuses.Core.Business
{
    public class Document : JDocument,IDocument
    {
        public Document (){}

        public Document(JObject jobject) : base(jobject) { }

        //eventually override these methods in the subclasses if they are needed
        public void BeforeCreate() { }
        public void AfterCreate() { }
        public void BeforeUpdate() { }
        public void AfterUpdate() { }
    }
}
