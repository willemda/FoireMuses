
namespace FoireMuses.Core.Interfaces
{
	public interface ISettingsController
	{
		string Host { get; }
		int Port { get; }
		string Username { get; }
		string Password { get; }
		string DatabaseName { get; }
		string LilyPondCommand { get; }
		string ToLyCommand { get; }
		string ToLyArgs { get; }
		string ToLyExpectedFile { get; }
		string ToPsArgs { get; }
		string ToPsExpectedFile { get; }
		string ToPdfArgs { get; }
		string ToPdfExpectedFile { get; }
		string ToPngCommand { get; }
		string ToPngArgs { get; }
		string ToPngExpectedFile { get; }
		string ToMidiArgs { get; }
		string ToMidiExpectedFile { get; }
	}
}