using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FoireMuses.Core.Interfaces
{
	public interface IUser
	{
		string Username { get; set; }
		string Password { get; set; }
		string Email { get; set; }
		IEnumerable<string> Groups { get; }
		void AddGroup(string group);
		void RemoveGroup(string group);
	}
}
