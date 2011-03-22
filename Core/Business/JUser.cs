using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Exceptions;

namespace FoireMuses.Core.Business
{
    public class JUser : Document
    {

        public JUser ()
		{
			this.Add ("type", "user");
		}

        public JUser(JObject jobject) : base(jobject) {
            this.Add("type", "user");
        }

        public override void BeforeCreate(){
            validate();
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

    }
}
