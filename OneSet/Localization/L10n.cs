using System;
using System.Reflection;
using System.Diagnostics;
using System.Resources;
using System.Threading;
using System.Globalization;
using OneSet.Resx;
using Xamarin.Forms;

namespace OneSet.Localization
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
			ResourceManager temp = new ResourceManager("OneSet.Resx.AppResources", typeof(AppResources).GetTypeInfo().Assembly);
			string result = temp.GetString (key, new CultureInfo (netLanguage));

			return result; 
		}

		public static string GetWeightUnit()
		{
			if (App.Settings.IsMetric)
			{
				return "Kgs";
			}
			return "Lbs";
		}
	}
}

