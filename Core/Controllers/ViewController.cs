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
            if (Context.Current.Instance.CouchDbController.CouchDatabase.DocumentExists("_design/scores"))
            {
                
                CouchDesignDocument mavue = Context.Current.Instance.CouchDbController.CouchDatabase.GetDocument("_design/scores",new Result<CouchDesignDocument>()).Wait();
                Context.Current.Instance.CouchDbController.CouchDatabase.DeleteDocument(mavue);
            }
            CouchDesignDocument view = new CouchDesignDocument("scores");
            view.Views.Add("all",
                          new CouchView(
                             @"function(doc){
				                       if(doc.type && doc.type == 'score'){
				                          emit(doc.title, doc._id)
				                       }
				                    }"));
            view.Views.Add("fromsource",
                           new CouchView(
                              @"function(doc){
				                       if(doc.type && doc.type == 'score' && doc.source_id){
				                          emit(doc.source_id, doc.title)
				                       }
				                    }"));
            Context.Current.Instance.CouchDbController.CouchDatabase.CreateDocument(view, new Result<CouchDesignDocument>());
        }

        public void createGetAllView()
        {
            if (Context.Current.Instance.CouchDbController.CouchDatabase.DocumentExists("_design/alldoc"))
            {

                CouchDesignDocument mavue = Context.Current.Instance.CouchDbController.CouchDatabase.GetDocument("_design/alldoc", new Result<CouchDesignDocument>()).Wait();
                Context.Current.Instance.CouchDbController.CouchDatabase.DeleteDocument(mavue);
            }
            CouchDesignDocument view = new CouchDesignDocument("alldoc");
            view.Views.Add("all",
                          new CouchView(
                             @"function(doc){
				                       if(doc.type){
				                          emit(doc.type, doc._id)
				                       }
				                    }"));
            Context.Current.Instance.CouchDbController.CouchDatabase.CreateDocument(view, new Result<CouchDesignDocument>());
        }

        public void createGetHeadScoresView()
        {
            if (Context.Current.Instance.CouchDbController.CouchDatabase.DocumentExists("_design/scores"))
            {

                CouchDesignDocument mavue = Context.Current.Instance.CouchDbController.CouchDatabase.GetDocument("_design/scores", new Result<CouchDesignDocument>()).Wait();
                Context.Current.Instance.CouchDbController.CouchDatabase.DeleteDocument(mavue);
            }
            CouchDesignDocument view = new CouchDesignDocument("scores");
            view.Views.Add("head",
                          new CouchView(
                             @"function(doc){
				                       if(doc.otype && doc.title && doc.otype=='score'){
				                          emit(doc._id, doc.title)
				                       }
				                    }"));
            Context.Current.Instance.CouchDbController.CouchDatabase.CreateDocument(view, new Result<CouchDesignDocument>());
        }
    }
}
