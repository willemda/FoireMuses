using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using FoireMuses.Core.Exceptions;
using LoveSeat.Interfaces;
using LoveSeat;
using System.Text.RegularExpressions;

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


        static Regex goodCharsForUsernameRegex = new Regex("\\w{5,20}");
        static Regex goodCharsForPasswordRegex = new Regex("\\w{5,20}");

        private void validate()
        {
            CheckUsername();
            CheckPassword();
        }

        private void CheckUsername(){
            JToken jtok;
            if(this.TryGetValue("username", out jtok)){
                if(jtok != null && String.IsNullOrEmpty(jtok.Value<string>())){
                    throw new EmptyFieldException("username");
                }
                if(DoesContainBadCharacters(goodCharsForUsernameRegex, jtok.Value<string>())){
                    throw new BadCharacterException("username");
                }
            }
        }

        private void CheckPassword(){
            JToken jtok;
            if(this.TryGetValue("password", out jtok)){
                if(jtok != null && String.IsNullOrEmpty(jtok.Value<string>())){
                    throw new EmptyFieldException("password");
                }
                if(DoesContainBadCharacters(goodCharsForPasswordRegex, jtok.Value<string>())){
                    throw new BadCharacterException("password");
                }
            }
        }

        
        private bool DoesContainBadCharacters(Regex theGoodCharsRegex,string theString)
        {
            if (!theGoodCharsRegex.IsMatch(theString))
            {
                return true;
            }
            return false;
        }


        public override void Created()
        {
            base.Created();
        }

        public override void Creating()
        {
            base.Creating();
            validate();
        }

        public override void Deleted()
        {
            base.Deleted();
        }

        public override void Deleting()
        {
            base.Deleting();
        }

        public override void Updated()
        {
            base.Updated();
        }

        public override void Updating()
        {
            base.Updating();
            validate();
        }
    }
}
