﻿using System;
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
using FoireMuses.Core.Querys;
using MusicXml;
using FoireMuses.MusicXMLImport;
#endif
namespace FoireMuses.UnitTests.CoreTests
{
	[TestClass]
	public class IndexControllerTests
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
				.Attr("name", "ConverterFactory").End()
				.Start("component").Attr("type", "FoireMuses.Core.Interfaces.INotificationManager, FoireMuses.Core")
				.Attr("implementation", "FoireMuses.Core.Loveseat.NotificationManager, FoireMuses.Core.Loveseat")
				.Attr("name", "ConverterFactory").End()
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
		public void IndexAndSearchMustBeOk()
		{
			JObject o = new JObject();
			o["title"] = "la belle qui dors";
			o["editor"] = "arnaud";
			IScore score = Context.Current.Instance.ScoreController.FromJson(o.ToString());
			Context.Current.Instance.IndexController.AddScore(score, new Result()).Wait();
			ScoreQuery q = new ScoreQuery()
			{
				Offset = 0,
				Max = 10,
				Title = "belle",
				Editor = "arnaud"
			};
			SearchResult<IScoreSearchResult> result = Context.Current.Instance.IndexController.SearchScore(q, new Result<SearchResult<IScoreSearchResult>>()).Wait();
		}


		[TestMethod]
		public void ConvertLilyToCodeMustBeOk()
		{
			XDoc doc = XDocFactory.From(File.OpenRead(@"G:\test.xml"), MimeType.XML);
			string lily = "c' d' e' f' dis'";
			XScore score = new XScore(doc);
			string codage = score.GetCodageMelodiqueRISM();
			string codage2 = score.GetCodageParIntervalle();
			string code = Context.Current.Instance.IndexController.LilyToCodageMelodiqueRISM(lily);
			string code2 = Context.Current.Instance.IndexController.LilyToCodageParIntervalles(lily);
		}
	}
}