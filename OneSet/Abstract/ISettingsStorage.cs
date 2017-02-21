using OneSet.Models;

namespace OneSet.Abstract
{
	public interface ISettingsStorage
	{
		void Save(Settings settings);
		Settings Load();
	}
}

