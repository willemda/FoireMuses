﻿using System;
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
        public virtual void BeforeCreate() { }
        public virtual void AfterCreate() { }
        public virtual void BeforeUpdate() { }
        public virtual void AfterUpdate() { }
    }
}
