using Set.Models;

namespace Set
{
	public interface ISettingsStorage
	{
		void Save(Settings settings);
		Settings Load();
	}
}

