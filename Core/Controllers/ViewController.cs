using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveSeat;
using MindTouch.Tasking;

namespace FoireMuses.Core.Controllers
{
    /// <summary>
    /// For testing purposes only
    /// </summary>
    public class ViewController
    {
        public void createScoresFromSourceView()
        {
            CouchDesignDocument view = new CouchDesignDocument("scoresfromsource");
            view.Views.Add("all",
                           new CouchView(
                              @"function(doc){
				                       if(doc.type && doc.type == 'score' && doc.source_id){
				                          emit(doc.source_id, doc.titre)
				                       }
				                    }"));
            Context.Current.Instance.CouchDbController.CouchDatabase.CreateDocument(view, new Result<CouchDesignDocument>());
        }
    }
}
