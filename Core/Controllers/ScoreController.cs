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
                theLogger.Debug(koptions.ToString());

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


		/*public Result<Business.JScore> GetDocument(JScore aJScore, Result<Business.JScore> aResult)
		{
			if(Context.Current.User == null)
			{
				aResult.Throw(new Exception("No User specified"));
			}
			else
			{
				aResult.Return(new JScore());
			}
			return aResult;
		}

		public Result<Business.JScore> UpdateDocument(JScore aJScore, Result<Business.JScore> aResult)
		{
			throw new NotImplementedException();
		}*/
	}
}

