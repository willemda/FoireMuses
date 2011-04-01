using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
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
using FoireMuses.Core.Interfaces;
using Newtonsoft.Json.Linq;
#endif

namespace FoireMuses.UnitTests.CoreTests
{
	[TestClass]
	public class UserControllerTests
	{

		private static InstanceFactory theInstanceFactory;

		[ClassInitialize]
		public static void Setup(TestContext testContext)
		{
			var instances = new XDoc("instances")
				.Start("instance").Attr("webhost", "test.foiremuses.org").Attr("databaseName", "foiremusesxml")
				.Start("components")
				.Start("component").Attr("type", "FoireMuses.Core.Interfaces.IScoreDataMapper, FoireMuses.Core")
				.Attr("implementation", "FoireMuses.Core.Loveseat.LoveseatScoreDataMapper, FoireMuses.Core.Loveseat")
				.Attr("name", "ScoreDataMapper").End()
				.Start("component").Attr("type", "FoireMuses.Core.Interfaces.IPlayDataMapper, FoireMuses.Core")
				.Attr("implementation", "FoireMuses.Core.Loveseat.LoveseatPlayDataMapper, FoireMuses.Core.Loveseat")
				.Attr("name", "PlayDataMapper").End()
				.Start("component").Attr("type", "FoireMuses.Core.Interfaces.ISourceDataMapper, FoireMuses.Core")
				.Attr("implementation", "FoireMuses.Core.Loveseat.LoveseatSourceDataMapper, FoireMuses.Core.Loveseat")
				.Attr("name", "SourceDataMapper").End()
				.Start("component").Attr("type", "FoireMuses.Core.Interfaces.IUserDataMapper, FoireMuses.Core")
				.Attr("implementation", "FoireMuses.Core.Loveseat.LoveseatUserDataMapper, FoireMuses.Core.Loveseat")
				.Attr("name", "UserDataMapper").End()
				.End().End();
			theInstanceFactory = new InstanceFactory(new ContainerBuilder().Build(), instances);
		}

		[TestInitialize]
		public void Setup()
		{
			Context context = new Context(theInstanceFactory.GetDefaultInstance()); // TODO
			context.AttachToCurrentTaskEnv();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CreationWithNullMustThrowException()
		{
			Context.Current.Instance.UserController.Create(null, new Result<IUser>()).Wait();
		}

		[TestMethod]
		public void CreationUserMustBeOk()
		{
			JObject jo = new JObject { {"_id","popol"},{"otype","user"},{"password","azerty"},{"email","salut@gmail.com"}};
			string joString = jo.ToString();
			IUser user = Context.Current.Instance.UserController.FromJson(joString);
			Result<IUser> result = new Result<IUser>();
			Context.Current.Instance.UserController.Create(user, result).Wait();
			Assert.IsTrue(result.HasValue);
			IUser user2 = result.Value;
			Assert.AreEqual("popol", user2.Id);
			Assert.AreEqual("azerty", user2.Password);
			Assert.AreEqual("salut@gmail.com", user2.Email);
		}
	}
}
