using System;

namespace Set.Localization
{
	public interface ILocale
	{
		string GetCurrent();

		void SetLocale();
	}
}

