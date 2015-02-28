using System;
using System.Reflection;
using System.Diagnostics;
using System.Resources;
using System.Threading;
using System.Globalization;
using Xamarin.Forms;
using Set.Resx;

namespace Set.Localization
{
	public class L10n
	{
		public static void SetLocale ()
		{
			DependencyService.Get<ILocale>().SetLocale();
		}

		/// <remarks>
		/// Maybe we can cache this info rather than querying every time
		/// </remarks>
		public static string Locale ()
		{
			return DependencyService.Get<ILocale>().GetCurrent();
		}

		public static string Localize(string key, string comment) {

			var netLanguage = Locale ();

			// Platform-specific
			ResourceManager temp = new ResourceManager("Set.Resx.AppResources", typeof(AppResources).GetTypeInfo().Assembly);
			string result = temp.GetString (key, new CultureInfo (netLanguage));

			return result; 
		}

		public static string GetWeightUnit()
		{
			if (App.Settings.IsMetric == 1)
			{
				return "Kgs";
			}
			return "Lbs";
		}
	}
}

