
namespace FoireMuses.Core.Interfaces
{
	public interface ISettingsController
	{
		string Host { get; }
		int Port { get; }
		string Username { get; }
		string Password { get; }
		string DatabaseName { get; }
	}
}