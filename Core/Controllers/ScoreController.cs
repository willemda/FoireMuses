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
using Core.Interfaces;


namespace Core.Controllers
{
	using Context;
	using Core.Utils;

	public class ScoreController : IScoreController
	{
		public ScoreController ()
		{
		}

		public Result<JDocument> CreateDocument (JDocument aDocument, Result<JDocument> aResult)
		{
			Utils.CheckObject ("aResult", aResult);

            Result<JDocument> res = new Result<JDocument>();

           Context.Current.CreateDocument(aDocument, res).WhenDone(
                res.Return,
                res.Throw
                );
            return res;
        }

        public Result<JDocument> UpdateDocument(JDocument aDocument, Result<JDocument> aResult)
        {

            Utils.CheckObject ("aResult", aResult);

            Result<JDocument> res = new Result<JDocument>();

            theDatabase.UpdateDocument(aDocument, res).WhenDone(
                res.Return,
                res.Throw
                );
            return res;
        }

        public Result<JDocument> GetDocument (string id, Result<JDocument> aResult)
        {

            Utils.CheckObject ("aResult", aResult);
        	Utils.CheckString ("id", id);

            Result<JDocument> res = new Result<JDocument>();

            theDatabase.GetDocument(id, res).WhenDone(
                res.Return,
                res.Throw
                );
            return res;
        }

        public Result<JObject> DeleteDocument(JDocument aDocument, Result<JObject> aResult)
        {

            Utils.CheckObject (aResult);

            Result<JObject> res = new Result<JObject>();

            theDatabase.DeleteDocument(aDocument,res)
                .WhenDone(
                res.Return,
                res.Throw
                );
            return res;
        }



        public Result<ViewResult<string, string, JDocument>> getViewDocument (string aViewId, string aViewName, Result<ViewResult<string, string, JDocument>> aResult)
        {

            Utils.CheckObject ("aResult", aResult);
			Utils.CheckString ("aViewId", aViewId);
			Utils.CheckString ("aViewName", aViewName);

            var aRes = new Result<ViewResult<string, string, JDocument>>();

            theDatabase.GetView(aViewId, aViewName, aRes).WhenDone(
                aRes.Return,
                aRes.Throw
                );
            return aRes;
        }

        public Result<ViewResult<string[], string, JDocument>> getViewDocument (string aViewId, string aViewName, Result<ViewResult<string[], string, JDocument>> aResult)
        {

            Utils.CheckObject ("aResult", aResult);
        	Utils.CheckString ("aViewId", aViewId);
        	Utils.CheckString ("aViewName", aViewName);

            var aRes = new Result<ViewResult<string[], string, JDocument>>();

            theDatabase.GetView(aViewId, aViewName, aRes).WhenDone(
                aRes.Return,
                aRes.Throw
                );
            return aRes;
        }
	}
}

