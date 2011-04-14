using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac.Registrars;
using FoireMuses.Core;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core.Controllers;
using Autofac;
using Autofac.Builder;
using MindTouch.Dream;
using MindTouch.Xml;
using log4net;

namespace FoireMuses.Core
{
	/// <summary>
	/// Contains a reference to the Controllers
	/// </summary>
	public class Instance
	{
		public IScoreController ScoreController { get; private set; }
		public ISourceController SourceController { get; private set; }
		public IUserController UserController { get; private set; }
		public IPlayController PlayController { get; private set; }
		public IConverterFactory ConverterFactory { get; private set; }

		public Instance(IContainer container, XDoc anInstanceXmlConfig)
		{
			ContainerBuilder builder = new ContainerBuilder();

			var c = new XDocAutofacContainerConfigurator(anInstanceXmlConfig["components"], DreamContainerScope.Factory);
			c.Configure(container);

			if (!container.IsRegistered<ISettingsController>())
			{
				XmlSettingsController controller = new XmlSettingsController(anInstanceXmlConfig);
				builder.Register<ISettingsController>(controller);
			}

			if (!container.IsRegistered<IScoreController>())
			{
				builder.Register<ScoreController>().As<IScoreController>();
			}

			if (!container.IsRegistered<IUserController>())
			{
				builder.Register<UserController>().As<IUserController>();
			}

			if (!container.IsRegistered<ISourceController>())
			{
				builder.Register<SourceController>().As<ISourceController>();
			}

			if (!container.IsRegistered<IPlayController>())
			{
				builder.Register<PlayController>().As<IPlayController>();
			}

			if (!container.IsRegistered<IConverterFactory>())
			{
				builder.Register<ConverterFactory>().As<IConverterFactory>();
			}

			builder.Build(container);

			ScoreController = container.Resolve<IScoreController>();
			UserController = container.Resolve<IUserController>();
			SourceController = container.Resolve<ISourceController>();
			PlayController = container.Resolve<IPlayController>();
			ConverterFactory = container.Resolve<IConverterFactory>();
		}
	}
}
