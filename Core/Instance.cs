using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoireMuses.Core;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core.Controllers;
using Autofac;
using Autofac.Builder;

namespace FoireMuses.Core
{
	/// <summary>
	/// Contains a reference to the Controllers
	/// </summary>
	public class Instance
	{
		internal ICouchDBController CouchDbController { get; private set; }
		public IScoreController ScoreController { get; private set; }
        public ISourceController SourceController { get; private set; }
        public ViewController ViewController { get; private set; }
        public IUserController UserController { get; private set; }

		public Instance(IContainer container)
		{
            if (!container.IsRegistered<IScoreController>())
            {
                var builder = new ContainerBuilder();
                builder.Register<ScoreController>().As<IScoreController>();
                builder.Build(container);
            }
            ScoreController = container.Resolve<IScoreController>();

            if (!container.IsRegistered<ICouchDBController>())
            {
                var builder = new ContainerBuilder();
                builder.Register<CouchDBController>().As<ICouchDBController>();
                builder.Build(container);
            }
            CouchDbController = container.Resolve<ICouchDBController>();

            if (!container.IsRegistered<IUserController>())
            {
                var builder = new ContainerBuilder();
                builder.Register<UserController>().As<IUserController>();
                builder.Build(container);
            }
            UserController = container.Resolve<IUserController>();


            if (!container.IsRegistered<ISourceController>())
            {
                var builder = new ContainerBuilder();
                builder.Register<SourceController>().As<ISourceController>();
                builder.Build(container);
            }
            SourceController = container.Resolve<ISourceController>();

            ViewController = new ViewController();
		}
	}
}
