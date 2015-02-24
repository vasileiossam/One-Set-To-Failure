using System;
using Set.Models;
using System.Threading.Tasks;

namespace Set
{
	public interface ISettingsStorage
	{
		Task SaveAsync(Settings settings);
		Task<Settings> LoadAsync();
	}
}

