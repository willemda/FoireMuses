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
		public Result<JDocument> CreateDocument(JDocument aDocument, Result<JDocument> aResult)
		{
			ArgCheck.NotNull("aDocument", aDocument);
			ArgCheck.NotNull("aResult", aResult);

			Result<JDocument> res = new Result<JDocument>();

			Context.Current.Instance.CouchDbController.CouchDatabase.CreateDocument(aDocument, res).WhenDone(
				 res.Return,
				 res.Throw
				 );
			return res;
		}

		public Result<JDocument> UpdateDocument(JDocument aDocument, Result<JDocument> aResult)
		{
			ArgCheck.NotNull("aResult", aResult);

			Result<JDocument> res = new Result<JDocument>();

			Context.Current.Instance.CouchDbController.CouchDatabase.UpdateDocument(aDocument, res).WhenDone(
				res.Return,
				res.Throw
				);
			return res;
		}

		public Result<JDocument> GetDocument(string id, Result<JDocument> aResult)
		{
			ArgCheck.NotNull("aResult", aResult);
			ArgCheck.NotNullNorEmpty("id", id);

			Result<JDocument> res = new Result<JDocument>();

			Context.Current.Instance.CouchDbController.CouchDatabase.GetDocument(id, res).WhenDone(
				res.Return,
				res.Throw
				);
			return res;
		}

		public Result<JObject> DeleteDocument(JDocument aDocument, Result<JObject> aResult)
		{
			ArgCheck.NotNull("aResult",aResult);

			Result<JObject> res = new Result<JObject>();

			Context.Current.Instance.CouchDbController.CouchDatabase.DeleteDocument(aDocument, res)
				.WhenDone(
				res.Return,
				res.Throw
				);
			return res;
		}

		public Result<ViewResult<string, string, JDocument>> GetViewDocument(string aViewId, string aViewName, Result<ViewResult<string, string, JDocument>> aResult)
		{
			ArgCheck.NotNull("aResult", aResult);
			ArgCheck.NotNullNorEmpty("aViewId", aViewId);
			ArgCheck.NotNullNorEmpty("aViewName", aViewName);

			var aRes = new Result<ViewResult<string, string, JDocument>>();

			Context.Current.Instance.CouchDbController.CouchDatabase.GetView(aViewId, aViewName, aRes).WhenDone(
				aRes.Return,
				aRes.Throw
				);
			return aRes;
		}

		public Result<ViewResult<string[], string, JDocument>> GetViewDocument(string aViewId, string aViewName, Result<ViewResult<string[], string, JDocument>> aResult)
		{

			ArgCheck.NotNull("aResult", aResult);
			ArgCheck.NotNullNorEmpty("aViewId", aViewId);
			ArgCheck.NotNullNorEmpty("aViewName", aViewName);

			var aRes = new Result<ViewResult<string[], string, JDocument>>();

			Context.Current.Instance.CouchDbController.CouchDatabase.GetView(aViewId, aViewName, aRes).WhenDone(
				aRes.Return,
				aRes.Throw
				);
			return aRes;
		}

		public Result<JScore> CreateDocument(JDocument aDocument, Result<JScore> aResult)
		{
			ArgCheck.NotNull("aResult", aResult);

			//Result<JScore> res = new Result<JScore>();

			//Context.Current.Instance.CouchDbController.CouchDatabase.CreateDocument(aDocument, res).WhenDone(
			//     res.Return,
			//     res.Throw
			//     );
			return aResult;
		}

		public Result<Business.JScore> GetDocument(JDocument aDocument, Result<Business.JScore> aResult)
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

		public Result<Business.JScore> UpdateDocument(JDocument aDocument, Result<Business.JScore> aResult)
		{
			throw new NotImplementedException();
		}
	}
}

