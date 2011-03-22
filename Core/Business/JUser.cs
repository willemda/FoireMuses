using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Exceptions;
using LoveSeat.Interfaces;
using LoveSeat;

namespace FoireMuses.Core.Business
{
    public class JUser : Document
    {

        public JUser ()
		{
			this.Add ("type", "user");
		}

        public JUser(JObject jobject) : base(jobject) {
            JToken type;
            if (this.TryGetValue("otype", out type))
            {
                if (type.Value<string>() != "user")
                    throw new Exception("Bad object type");
            }
            else
            {
                this.Add("otype", "user");
            }
        }

        private void validate()
        {
            JToken jtok;
            if(this.TryGetValue("username", out jtok)){
                if(String.IsNullOrEmpty(jtok.Value<string>())){
                    throw new EmptyFieldException("username");
                }
            }
            
          
            if(this.TryGetValue("password", out jtok)){
                if(String.IsNullOrEmpty(jtok.Value<string>())){
                    throw new EmptyFieldException("password");
                }
            }

        }


        public void Created()
        {
            base.Created();
        }

        public void Creating()
        {
            base.Creating();
        }

        public void Deleted()
        {
            base.Deleted();
        }

        public void Deleting()
        {
            base.Deleting();
        }

        public void Updated()
        {
            base.Updated();
        }

        public void Updating()
        {
            base.Updating();
        }
    }
}
