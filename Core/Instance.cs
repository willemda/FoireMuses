using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core.Controllers;
using Autofac;
using Autofac.Builder;
using MindTouch.Xml;

namespace FoireMuses.Core
{
	/// <summary>
	/// Contains a reference to the Controllers
	/// </summary>
	public class Instance
	{
		internal IStoreController CouchDbController { get; private set; }
		public IScoreController ScoreController { get; private set; }
		public ISourceController SourceController { get; private set; }
		public ViewController ViewController { get; private set; }
		public IUserController UserController { get; private set; }

		public Instance(IContainer container, XDoc anInstanceXmlConfig)
		{
			ContainerBuilder builder = new ContainerBuilder();

			if(!container.IsRegistered<ISettingsController>())
			{
				XmlSettingsController controller = new XmlSettingsController(anInstanceXmlConfig);
				builder.Register<ISettingsController>(controller);
			}
			if (!container.IsRegistered<IScoreController>())
			{
				builder.Register<ScoreController>().As<IScoreController>();
			}

			if (!container.IsRegistered<IStoreController>())
			{
				builder.Register<CouchDBController>().As<IStoreController>();
			}

			if (!container.IsRegistered<IUserController>())
			{
				builder.Register<UserController>().As<IUserController>();
			}

			if (!container.IsRegistered<ISourceController>())
			{
				builder.Register<SourceController>().As<ISourceController>();
			}

			builder.Build(container);

			ScoreController = container.Resolve<IScoreController>();
			CouchDbController = container.Resolve<IStoreController>();
			UserController = container.Resolve<IUserController>();
			SourceController = container.Resolve<ISourceController>();
			ViewController = new ViewController();
		}
	}
}
