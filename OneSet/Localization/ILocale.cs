using System;

namespace OneSet.Localization
{
	public interface ILocale
	{
		string GetCurrent();

		void SetLocale();
	}
}

