using OneSet.Models;

namespace OneSet
{
	public interface ISettingsStorage
	{
		void Save(Settings settings);
		Settings Load();
	}
}

