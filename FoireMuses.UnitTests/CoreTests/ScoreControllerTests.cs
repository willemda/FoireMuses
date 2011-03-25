using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using FoireMuses.Core.Business;
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
	public class ScoreControllerTests
	{
		private static InstanceFactory theInstanceFactory;

		[ClassInitialize]
		public static void Setup(TestContext testContext)
		{
			var instances = new XDoc("instances")
				.Start("instance").Attr("webhost", "test.foiremuses.org").Attr("databaseName", "foiremusesxml").End();

			theInstanceFactory = new InstanceFactory(new ContainerBuilder().Build(), instances);
		}

		[TestInitialize]
		public void Setup()
		{
			Context context = new Context(theInstanceFactory.GetDefaultInstance()); // TODO
			JUser myUser = new JUser();
			myUser.Add("_id", "1");
			myUser.Add("username", "danny");
			context.User = myUser;
			context.AttachToCurrentTaskEnv();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CreationWithNullMustThrowException()
		{
			Context.Current.Instance.ScoreController.Create(null, new Result<IScore>()).Wait();
		}

		[TestMethod]
		public void CreationMustBeOk()
		{
			JObject o = new JObject();
			o["title"] = "la belle qui dors";
			o["editor"] = "arnaud";
			IScore score = Factory.IScoreFromJson(o.ToString());
			Result<IScore> result = new Result<IScore>();
			Context.Current.Instance.ScoreController.Create(score, result).Wait();
			Assert.IsTrue(result.HasValue);
			Assert.AreEqual("la belle qui dors",score.Title);
			Assert.AreEqual("arnaud",score.Editor);
			Console.WriteLine((score as JScore).ToString());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void UpdateWithNullMustThrowException()
		{
			Context.Current.Instance.ScoreController.Update(null, new Result<IScore>()).Wait();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetWithNullMustThrowException()
		{
			Context.Current.Instance.ScoreController.Get((IScore)null, new Result<IScore>()).Wait();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetByIdWithNullMustThrowException()
		{
			Context.Current.Instance.ScoreController.Get((string)null, new Result<IScore>()).Wait();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void DeleteWithNullMustThrowException()
		{
			Context.Current.Instance.ScoreController.Delete(null, new Result<bool>()).Wait();
		}

	}
}
