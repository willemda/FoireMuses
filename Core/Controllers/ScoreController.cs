//  
//  ScoreController.cs
//  
//  Author:
//       danny <${AuthorEmail}>
// 
//  Copyright (c) 2011 danny
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using MindTouch.Tasking;
using LoveSeat;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Interfaces;
using FoireMuses.Core.Utils;
using FoireMuses.Core.Business;
using LoveSeat.Support;

namespace FoireMuses.Core.Controllers
{
    using Yield = System.Collections.Generic.IEnumerator<IYield>;
    using FoireMuses.Core.Exceptions;

	public class ScoreController : BaseController<JScore>,IScoreController 
	{
        private static readonly log4net.ILog theLogger = log4net.LogManager.GetLogger(typeof(ScoreController));

        string VIEW_SCORES_FROM_SOURCE_ID = "scores";
        string VIEW_SCORES_FROM_SOURCE_NAME = "fromsource";

		public Result<ViewResult<string, string, JScore>> GetScoresFromSource(JSource aJSource, Result<ViewResult<string, string, JScore>> aResult)
		{
            try
            {
                ArgCheck.NotNull("aResult", aResult);
                ArgCheck.NotNull("aJSource", aJSource);

                ViewOptions voptions = new ViewOptions();
                KeyOptions koptions = new KeyOptions();
                koptions.Add(aJSource.Id);
                voptions.Key = koptions;

                Context.Current.Instance.CouchDbController.CouchDatabase.GetView
                (
                    VIEW_SCORES_FROM_SOURCE_ID,
                    VIEW_SCORES_FROM_SOURCE_NAME,
                    voptions,
                    new Result<ViewResult<string, string, JScore>>()
                ).WhenDone(
                        aResult.Return,
                        aResult.Throw
                    );
            }
            catch (Exception e)
            {
                aResult.Throw(e);
            }
			return aResult;
		}

        string VIEW_SCORES_FROM_PLAY_ID = "";
        string VIEW_SCORES_FROM_PLAY_NAME = "";

		public Result<ViewResult<string[], string, JScore>> GetScoresFromPlay(JPlay aJPlay, Result<ViewResult<string[], string, JScore>> aResult)
		{

            try
            {
                ArgCheck.NotNull("aResult", aResult);
                ArgCheck.NotNull("aJPlay", aJPlay);

                Context.Current.Instance.CouchDbController.CouchDatabase.GetView
                (
                    VIEW_SCORES_FROM_PLAY_ID,
                    VIEW_SCORES_FROM_PLAY_NAME,
                    new Result<ViewResult<string[], string, JScore>>()
                ).WhenDone(
                        aResult.Return,
                        aResult.Throw
                    );
            }
            catch (Exception e)
            {
                aResult.Throw(e);
            }
            return aResult;
		}


        string VIEW_SCORES_ID = "scores";
        string VIEW_SCORES_HEAD = "head";

        public Result<ViewResult<string, string>> GetHead(int limit, Result<ViewResult<string, string>> aResult)
        {

            try
            {
                ArgCheck.NotNull("aResult", aResult);
                ViewOptions voptions = new ViewOptions();
                if (limit > 0)
                {
                    voptions.Limit = limit;
                }

                Context.Current.Instance.CouchDbController.CouchDatabase.GetView
                (
                    VIEW_SCORES_ID,
                    VIEW_SCORES_HEAD,
                    voptions,
                    new Result<ViewResult<string, string>>()
                ).WhenDone(
                        aResult.Return,
                        aResult.Throw
                    );
            }
            catch (Exception e)
            {
                aResult.Throw(e);
            }
            return aResult;
        }

        public Result<ViewResult<string, string>> GetHead(Result<ViewResult<string, string>> aResult)
        {
            return GetHead(0, aResult);
        }


        public Yield GetByIdd(string id,Result<JScore> aResult)
        {
            Result<JScore> jscoreRes = new Result<JScore>();
            yield return Context.Current.Instance.CouchDbController.CouchDatabase.GetDocument(id, jscoreRes);
            if (jscoreRes.HasException)
            {
                aResult.Throw(jscoreRes.Exception);
                yield break;
            }

            if (jscoreRes.Value == null)
            {
                aResult.Throw(new NoResultException());
                yield break;
            }

            Result<bool> type = new Result<bool>();
            yield return TypeScore(jscoreRes.Value, type);
            if (type.HasException)
            {
                aResult.Throw(type.Exception);
                yield break;
            }

            if (type.Value == true)
            {
                aResult.Return(jscoreRes.Value);
            }
            else
            {
                aResult.Throw(new NoResultException());
                yield break;
            } 
            
        }

        public Result<bool> TypeScore(JScore score, Result<bool> aResult)
        {
            JToken type;
            score.TryGetValue("otype", out type);
            if (type.Value<string>() == "score")
                aResult.Return(true);
            else
                aResult.Return(false);
            return aResult;
        }

        private void ErrorManage(Exception e)
        {
            theLogger.Error("Exception ici");
        }

        public override Result<JScore> GetById(string id, Result<JScore> aResult)
        {
            Coroutine.Invoke(GetByIdd, id, new Result<JScore>()).WhenDone(
                aResult.Return,
                aResult.Throw
                );
            return aResult;
            /*
            try
            {
                ArgCheck.NotNull("aResult", aResult);
                ArgCheck.NotNull("id", id);
                Result<JScore> res = new Result<JScore>();
                Context.Current.Instance.CouchDbController.CouchDatabase.GetDocument(id, res).WhenDone(
                    aResult.Return,
                    aResult.Throw
                    );
                //this.Readed();
            }
            catch (Exception e)
            {
                aResult.Throw(e);
            }

            return aResult;*/
        }

        public override void Readed(JScore score, Result<JScore> res)
        {
            JToken type;
            score.TryGetValue("otype", out type);
            if (type.Value<string>() == "score")
                res.Return(score);
            else
                res.Throw(new Exception("Bad type"));
        }
	}
}

