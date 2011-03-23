
namespace FoireMuses.Core.Interfaces
{
	internal interface ISettingsController
	{
		string Host { get; }
		int Port { get; }
		string Username { get; }
		string Password { get; }
		string DatabaseName { get; }
	}
}