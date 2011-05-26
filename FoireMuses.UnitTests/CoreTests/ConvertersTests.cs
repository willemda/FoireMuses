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
using FoireMuses.Core.Utils;
#endif
namespace FoireMuses.UnitTests.CoreTests
{
	[TestClass]
	public class ConvertersTests
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
			context.User = context.Instance.UserController.Retrieve("danny", new Result<IUser>()).Wait();
			context.AttachToCurrentTaskEnv();
		}

		[TestMethod]
		public void ConvertToLilyMustBeOk()
		{
			XDoc xdoc = XDocFactory.From(File.OpenRead(@"G:\MozaVeilSample.xml"), MimeType.XML);
			using (TemporaryFile inputFile = new TemporaryFile())
			using (TemporaryFile outputFile = new TemporaryFile())
			{
				xdoc.Save(inputFile.Path);
				//yield return Context.Current.Instance.SourceController.Exists("bla", new Result<bool>());
				IList<string> pngFilePath = Context.Current.Instance.ConverterFactory.GetConverter(Constants.Midi).Convert(inputFile.Path, outputFile.Path);
				foreach (string ooh in pngFilePath)
				{
					ooh.ToString();
				}
			}
		}

	}
}