using System;
using Set.Models;
using System.Threading.Tasks;

namespace Set
{
	public interface ISettingsStorage
	{
		void Save(Settings settings);
		Settings Load();
	}
}

