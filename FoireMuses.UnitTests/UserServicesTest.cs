/*
 * MindTouch Dream - a distributed REST framework 
 * Copyright (C) 2006-2011 MindTouch, Inc.
 * www.mindtouch.com  oss@mindtouch.com
 *
 * For community documentation and downloads visit wiki.developer.mindtouch.com;
 * please review the licensing section.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Linq;
using System.Net.Mail;
using Autofac.Builder;
using MindTouch.Dream;
using MindTouch.Dream.Services;
using MindTouch.Dream.Test;
using MindTouch.Tasking;
using MindTouch.Xml;

#if NUnit
using NUnit.Framework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
using TestFixtureAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
using TestFixtureSetUpAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute;
using TestFixtureTearDownAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core.Business;
using System.Collections.Generic;
#endif

namespace MindTouch.Core.Test.Services
{

    [TestFixture]
    public class UserServicesTest
    {



        //--- Constants ---
        private const string DEFAULT_HOST = "defaulthost";

        //--- Class Fields ---
        // private static readonly ILog _log = LogUtils.CreateLog();

        //--- Fields ---
        private static DreamHostInfo _hostInfo;
        private static DreamServiceInfo _service;
        private static Plug _plug;
        private static MockUserController muc;

        //--- Mock Controllers ---

        internal class MockUserController : IUserController
        {

            private JUser monUser = null;



            public Result<JUser> GetByUsername(string username, Result<JUser> aResult)
            {
                if (monUser != null)
                {
                    JToken nom;
                    monUser.TryGetValue("username", out nom);
                    if (nom.Value<string>() == username)
                    {
                        aResult.Return(monUser);
                    }
                    else
                    {
                        aResult.Throw(new Exception("Todo"));
                    }
                }
                else
                {
                    aResult.Throw(new Exception("Todo"));
                }
                return aResult;
            }

            public Result<JUser> Create(JUser aDoc, Result<JUser> aResult)
            {
                monUser = aDoc;
                aResult.Return(monUser);
                return aResult;
            }

            public Result<JUser> Get(string id, Result<JUser> aResult)
            {
                if (monUser != null && monUser.Id == id )
                {
                    aResult.Return(monUser);
                }
                else
                {
                    aResult.Throw(new Exception("Todo"));
                }
                return aResult;
            }

            public Result<JUser> Get(JUser aDoc, Result<JUser> aResult)
            {
                if (monUser != null && monUser.Id == aDoc.Id)
                {
                    aResult.Return(monUser);
                }
                else
                {
                    aResult.Throw(new Exception("Todo"));
                }
                return aResult;
            }

            public Result<JUser> Update(JUser aDoc, Result<JUser> aResult)
            {
                if (monUser != null && monUser.Id == aDoc.Id)
                {
                    monUser = aDoc;
                    aResult.Return(monUser);
                }
                else
                {
                    aResult.Throw(new Exception("Todo"));
                }
                return aResult;
            }

            public Result<JObject> Delete(JUser aDoc, Result<JObject> aResult)
            {
                if (monUser != null && monUser.Id == aDoc.Id)
                {
                    monUser = null;
                    aResult.Return(aDoc);
                }
                else
                {
                    aResult.Throw(new Exception("Todo"));
                }
                return aResult;
            }

            public void Created()
            {
                throw new NotImplementedException();
            }

            public void Updated()
            {
                throw new NotImplementedException();
            }

            public void Deleted()
            {
                throw new NotImplementedException();
            }

            public void Readed()
            {
                throw new NotImplementedException();
            }

            public Result<LoveSeat.ViewResult<string, string, JUser>> GetAll(Result<LoveSeat.ViewResult<string, string, JUser>> aResult)
            {
                throw new NotImplementedException();
            }
        }

        //--- Methods ---

        [TestFixtureSetUp]
        public static void GlobalSetup(TestContext testContext)
        {
            var config = new XDoc("config");
            var builder = new ContainerBuilder();
            muc = new MockUserController();
            builder.Register(c => muc).As<IUserController>().ServiceScoped();
            _hostInfo = DreamTestHelper.CreateRandomPortHost(config, builder.Build());
            _hostInfo.Host.Self.At("load").With("name", "foiremuses.webservice").Post(DreamMessage.Ok());
            _service = DreamTestHelper.CreateService(
                _hostInfo,
                "http://foiremuses.org/service",
                "foiremuses",
                new XDoc("config")
            );
            _plug = _service.WithInternalKey().AtLocalHost;
        }

        [TestInitialize]
        public void Setup()
        {
            muc = new MockUserController();
        }


        [TestFixtureTearDown]
        public static void GlobalTeardown()
        {
            _hostInfo.Dispose();
        }

        /*[SetUp]
        public void Setup()
        {
            _smtpClientFactory.Client = new SmtplClientMock();
            _smtpClientFactory.Settings = null;
        }*/

        /*[Test]
        public void Can_send_email_with_default_settings()
        {
            var email = new XDoc("email")
                .Attr("configuration", "default")
                .Elem("to", "to@bar.com")
                .Elem("from", "from@bar.com")
                .Elem("subject", "subject")
                .Elem("body", "body");
            
            var response = _plug.At("message").Post(email, new Result<DreamMessage>()).Wait();
            Assert.IsTrue(response.IsSuccessful);
            Assert.AreEqual(DEFAULT_HOST, _smtpClientFactory.Settings.Host);
            Assert.AreEqual("from@bar.com", _smtpClientFactory.Client.Message.From.ToString());
            Assert.AreEqual("to@bar.com", _smtpClientFactory.Client.Message.To.First().ToString());
            Assert.AreEqual("subject", _smtpClientFactory.Client.Message.Subject);
            Assert.AreEqual("body", _smtpClientFactory.Client.Message.Body);
        }*/

        [Test]
        public void Can_create_user_from_json()
        {
            var user = new JObject();
            user.Add("_id", "1");
            user.Add("username", "toto");
            user.Add("password", "azerty");
            var response = _plug.At("users").Post(DreamMessage.Ok(MimeType.JSON, user.ToString()), new Result<DreamMessage>()).Wait();
            Assert.IsTrue(response.IsSuccessful);
            Assert.AreEqual("1", JObject.Parse(response.ToText())["_id"]);
            Assert.AreEqual("toto", JObject.Parse(response.ToText())["username"]);
            Assert.AreEqual("azerty", JObject.Parse(response.ToText())["password"]);
        }

        [Test]
        public void Can_update_user()
        {
            var user = new JObject();
            user.Add("_id", "1");
            user.Add("username", "toto");
            user.Add("password", "azerty");
            _plug.At("users").Post(DreamMessage.Ok(MimeType.JSON, user.ToString()), new Result<DreamMessage>()).Wait();
            user.Remove("password");
            user.Add("password", "123456");
            var response = _plug.At("users").Put(DreamMessage.Ok(MimeType.JSON, user.ToString()), new Result<DreamMessage>()).Wait();
            Assert.IsTrue(response.IsSuccessful);
            Assert.AreEqual("1", JObject.Parse(response.ToText())["_id"]);
            Assert.AreEqual("toto", JObject.Parse(response.ToText())["username"]);
            Assert.AreEqual("123456", JObject.Parse(response.ToText())["password"]);
        }

        [Test]
        public void Can_read_user_by_id()
        {
            var user = new JObject();
            user.Add("_id", "1");
            user.Add("username", "toto");
            user.Add("password", "azerty");
            _plug.At("users").Post(DreamMessage.Ok(MimeType.JSON, user.ToString()), new Result<DreamMessage>()).Wait();
            var response  = _plug.At("users","1").Get(DreamMessage.Ok(), new Result<DreamMessage>()).Wait();
            Assert.IsTrue(response.IsSuccessful);
            Assert.AreEqual("1", JObject.Parse(response.ToText())["_id"]);
            Assert.AreEqual("toto", JObject.Parse(response.ToText())["username"]);
            Assert.AreEqual("azerty", JObject.Parse(response.ToText())["password"]);
        }

        [Test]
        public void Can_read_user_by_username()
        {
            var user = new JObject();
            user.Add("_id", "1");
            user.Add("username", "toto");
            user.Add("password", "azerty");
            _plug.At("users").Post(DreamMessage.Ok(MimeType.JSON, user.ToString()), new Result<DreamMessage>()).Wait();
            var response = _plug.At("users", "username","toto").Get(DreamMessage.Ok(), new Result<DreamMessage>()).Wait();
            Assert.IsTrue(response.IsSuccessful);
            Assert.AreEqual("1", JObject.Parse(response.ToText())["_id"]);
            Assert.AreEqual("toto", JObject.Parse(response.ToText())["username"]);
            Assert.AreEqual("azerty", JObject.Parse(response.ToText())["password"]);
        }

        // Delete methods not allowed
        /*[Test]
        public void Can_delete_score()
        {
            var score = new JObject();
            score.Add("_id", "1");
            score.Add("title", "la belle au bois dormant");
            _plug.At("scores").Post(DreamMessage.Ok(MimeType.JSON, score.ToString()), new Result<DreamMessage>()).Wait();
            var response = _plug.At("scores").Delete(DreamMessage.Ok(MimeType.JSON, score.ToString()), new Result<DreamMessage>()).Wait();
            Console.Write(response.ToString());
            Assert.IsTrue(response.IsSuccessful);
            Assert.AreEqual("1", JObject.Parse(response.ToText())["_id"]);
            Assert.AreEqual("la belle au bois dormant", JObject.Parse(response.ToText())["title"]);
        }*/
    }
}