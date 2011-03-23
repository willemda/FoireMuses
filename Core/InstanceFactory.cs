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
		private readonly Dictionary<string, Instance> theInstancesList = new Dictionary<string, Instance>();

		public InstanceFactory(IContainer aContainer, XDoc aConfig)
		{
			aConfig["//instance"].ForEach(
				x => theInstancesList.Add(
					x["@webhost"].AsText,
					new Instance(aContainer,x)));

			if(theInstancesList.Count == 0)
				throw  new ArgumentException("Invalid Configuration, you have to specify at least one instance");
		}

		public Instance GetInstance(DreamContext aContext, DreamMessage aRequest)
		{
			Instance instance = null;
			if (!theInstancesList.TryGetValue(aContext.Uri.Host, out instance))
			{
				instance = GetDefaultInstance();
			}
			return instance;
		}

		public Instance GetDefaultInstance()
		{
			//return the first one
			return theInstancesList[theInstancesList.Keys.First()];
		}
	}
}
