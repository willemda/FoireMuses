using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using MindTouch.Xml;
using MindTouch.Dream;
using FoireMuses.Core.Interfaces;
using Autofac.Builder;
using FoireMuses.Core.Controllers;

namespace FoireMuses.Core
{
    public class InstanceFactory
    {
        private Dictionary<string, Instance> listInstance = new Dictionary<string, Instance>();

        public InstanceFactory(IContainer container, XDoc config)
        {
            listInstance.Add("ask.devel.foiremuses.be", new Instance(container));
        }

        public Instance GetInstance(DreamContext context, DreamMessage request){
            Instance toReturn;
            //listInstance.TryGetValue(context.Uri.Path, out toReturn);
            listInstance.TryGetValue("ask.devel.foiremuses.be", out toReturn);
            return toReturn;
        }
    }
}
