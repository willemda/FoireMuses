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

namespace FoireMuses.Core.Controllers
{
	public class ScoreController : IScoreController
	{
		public Result<JScore> CreateDocument(JScore aJScore, Result<JScore> aResult)
		{
            try
            {
                ArgCheck.NotNull("aJScore", aJScore);
                ArgCheck.NotNull("aResult", aResult);

                Context.Current.Instance.CouchDbController.CouchDatabase.CreateDocument(aJScore, new Result<JScore>()).WhenDone(
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

		public Result<JScore> UpdateDocument(JScore aJScore, Result<JScore> aResult)
		{
            try
            {
                ArgCheck.NotNull("aResult", aResult);
                ArgCheck.NotNull("aJScore", aJScore);

                Context.Current.Instance.CouchDbController.CouchDatabase.UpdateDocument(aJScore, new Result<JScore>()).WhenDone(
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

		public Result<JScore> GetDocument(string id, Result<JScore> aResult)
		{
            try
            {
                ArgCheck.NotNull("aResult", aResult);
                ArgCheck.NotNullNorEmpty("id", id);

                Context.Current.Instance.CouchDbController.CouchDatabase.GetDocument(id, new Result<JScore>()).WhenDone(
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

        public Result<JScore> GetDocument(JScore aJScore, Result<JScore> aResult)
        {
            try
            {
                ArgCheck.NotNull("aResult", aResult);
                ArgCheck.NotNull("aJScore", aJScore);

                Context.Current.Instance.CouchDbController.CouchDatabase.GetDocument(aJScore.Id, new Result<JScore>()).WhenDone(
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

		public Result<JObject> DeleteDocument(JScore aJScore, Result<JObject> aResult)
		{
            try
            {
                ArgCheck.NotNull("aResult", aResult);
                ArgCheck.NotNull("aJScore", aJScore);

                Context.Current.Instance.CouchDbController.CouchDatabase.DeleteDocument(aJScore, new Result<JObject>())
                    .WhenDone(
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

        string VIEW_SCORES_FROM_SOURCE_ID = "";
        string VIEW_SCORES_FROM_SOURCE_NAME = "";

		public Result<ViewResult<string, string, JScore>> GetScoresFromSource(JSource aJSource, Result<ViewResult<string, string, JScore>> aResult)
		{
            try
            {
                ArgCheck.NotNull("aResult", aResult);
                ArgCheck.NotNull("aJSource", aJSource);

                Context.Current.Instance.CouchDbController.CouchDatabase.GetView
                (
                    VIEW_SCORES_FROM_SOURCE_ID,
                    VIEW_SCORES_FROM_SOURCE_NAME,
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

