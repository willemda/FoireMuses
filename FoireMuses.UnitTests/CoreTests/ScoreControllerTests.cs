using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using FoireMuses.Core.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FoireMuses.Core;
using Autofac.Builder;
using MindTouch.Xml;
using MindTouch.Tasking;


#if NUnit
using NUnit.Framework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
using TestFixtureAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
using TestFixtureSetUpAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute;
using TestFixtureTearDownAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute;
#endif
namespace FoireMuses.UnitTests.CoreTests
{
	[TestClass]
	public class ScoreControllerTests
	{
		private static InstanceFactory theInstanceFactory;

		[ClassInitialize]
		public static void Setup(TestContext testContext)
		{
			var instances = new XDoc("instances")
				.Start("instance").Attr("webhost", "test.foiremuses.org").Attr("databaseName", "foiremusesxml").End();

			theInstanceFactory = new InstanceFactory(new ContainerBuilder().Build(), instances);
			Context context = new Context(theInstanceFactory.GetDefaultInstance()); // TODO

			context.AttachToCurrentTaskEnv();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CreationWithNullMustThrowException()
		{
			Context.Current.Instance.ScoreController.Create(null, new Result<JScore>()).Wait();
		}
	}
}
