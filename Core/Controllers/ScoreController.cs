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
    using System.Collections.Generic;
    using System.Collections;

    public class ScoreController : BaseController<JScore>, IScoreController
    {
        private static readonly log4net.ILog theLogger = log4net.LogManager.GetLogger(typeof(ScoreController));


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
                    CouchViews.VIEW_SCORES,
                    CouchViews.VIEW_SCORES_FROM_SOURCE,
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

        public override Result<JScore> Create(JScore aDoc, Result<JScore> aResult)
        {
            base.Create(aDoc, new Result<JScore>()).WhenDone(
                aResult.Return,
                aResult.Throw
                );
            return aResult;
        }

        public override Result<JScore> GetById(string id, Result<JScore> aResult)
        {
            base.GetById(id, new Result<JScore>()).WhenDone(
                aResult.Return,
                aResult.Throw
                );
            return aResult;
        }

        public override Result<JScore> Get(JScore aDoc, Result<JScore> aResult)
        {
            base.Get(aDoc, new Result<JScore>()).WhenDone(
                aResult.Return,
                aResult.Throw
                );
            return aResult;
        }

        private Yield UpdateHelper(JScore aDoc, Result<JScore> aResult)
        {
            yield return CheckAuthorization(aDoc, new Result());
            //if we reach there we have the update rights.
            Result<JScore> jscore = new Result<JScore>();
            yield return base.Update(aDoc, new Result<JScore>());
            //finally return the jscore updated
            aResult.Return(jscore.Value);
            yield break;
        }

        public override Result<JScore> Update(JScore aDoc, Result<JScore> aResult)
        {
            Coroutine.Invoke(UpdateHelper, aDoc, new Result<JScore>()).WhenDone(
                aResult.Return,
                aResult.Throw
                );
            return aResult;
        }

        public override Result<JObject> Delete(JScore aDoc, Result<JObject> aResult)
        {
            base.Delete(aDoc, new Result<JObject>()).WhenDone(
                aResult.Return,
                aResult.Throw
                );
            return aResult;
        }


        public Result<ViewResult<string, string, JScore>> GetScoresFromPlay(JPlay aJPlay, Result<ViewResult<string, string, JScore>> aResult)
        {
            ArgCheck.NotNull("aResult", aResult);
            ArgCheck.NotNull("aJPlay", aJPlay);

            ViewOptions voptions = new ViewOptions();
            KeyOptions koptions = new KeyOptions();
            koptions.Add(aJPlay.Id);
            voptions.Key = koptions;

            Context.Current.Instance.CouchDbController.CouchDatabase.GetView
            (
                CouchViews.VIEW_SCORES,
                CouchViews.VIEW_SCORES_FROM_PLAY,
                voptions,
                new Result<ViewResult<string, string, JScore>>()
            ).WhenDone(
                    aResult.Return,
                    aResult.Throw
                );
            return aResult;
        }




        public Result<ViewResult<string, string>> GetAll(int limit, Result<ViewResult<string, string>> aResult)
        {


            ArgCheck.NotNull("aResult", aResult);
            ViewOptions voptions = new ViewOptions();
            if (limit > 0)
            {
                voptions.Limit = limit;
            }

            Context.Current.Instance.CouchDbController.CouchDatabase.GetView
            (
                CouchViews.VIEW_SCORES,
                CouchViews.VIEW_ALL,
                voptions,
                new Result<ViewResult<string, string>>()
            ).WhenDone(
                    aResult.Return,
                    aResult.Throw
                );

            return aResult;
        }


        public Result<ViewResult<string, string>> GetAll(Result<ViewResult<string, string>> aResult)
        {
            ArgCheck.NotNull("aResult", aResult);
            return GetAll(0, aResult);
        }

    }
}

