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
using System.IO;
using MindTouch.Dream;
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
				.Start("component").Attr("type", "FoireMuses.Core.Interfaces.IConverterFactory, FoireMuses.Core")
				.Attr("implementation", "FoireMuses.Core.ConverterFactory, FoireMuses.Core")
				.Attr("name", "UserDataMapper").End()
				.End().End();
			theInstanceFactory = new InstanceFactory(new ContainerBuilder().Build(), instances);
		}

		[TestInitialize]
		public void Setup()
		{
			Context context = new Context(theInstanceFactory.GetDefaultInstance()); // TODO
			context.User = context.Instance.UserController.Retrieve("danny",new Result<IUser>()).Wait();
			context.AttachToCurrentTaskEnv();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CreationWithNullMustThrowException()
		{
			Context.Current.Instance.ScoreController.Insert(null, new Result<IScore>()).Wait();
		}

		[TestMethod]
		public void CreationMustBeOk()
		{
			JObject o = new JObject();
			o["title"] = "la belle qui dors";
			o["editor"] = "arnaud";
			IScore score = Context.Current.Instance.ScoreController.FromJson(o.ToString());
			Result<IScore> result = new Result<IScore>();
			score = Context.Current.Instance.ScoreController.Insert(score, result).Wait();
			Assert.AreEqual("la belle qui dors",score.Title);
			Assert.AreEqual("arnaud",score.Editor);
			Console.WriteLine(score.ToString());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void UpdateWithNullMustThrowException()
		{
			Context.Current.Instance.ScoreController.Update(null,null,null, new Result<IScore>()).Wait();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetByIdWithNullMustThrowException()
		{
			Context.Current.Instance.ScoreController.Retrieve(null, new Result<IScore>()).Wait();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void DeleteWithNullMustThrowException()
		{
			Context.Current.Instance.ScoreController.Delete(null,null, new Result<bool>()).Wait();
		}

        [TestMethod]
        public void CreationFromMusicXmlMustBeOk()
        {
            XDoc xdoc = XDocFactory.From(File.OpenRead(@"C:\Projects\FoireMuses\Dichterliebe01.xml"),MimeType.XML);
            IScore score  = Context.Current.Instance.ScoreController.CreateNew();
            Result<IScore> result = new Result<IScore>();
            score = Context.Current.Instance.ScoreController.AttachMusicXml(score, xdoc, false, result).Wait();
            Assert.AreEqual("Im wunderschönen Monat Mai", score.Title);
            Assert.AreEqual("Robert Schumann", score.Composer);
        }

	}
}