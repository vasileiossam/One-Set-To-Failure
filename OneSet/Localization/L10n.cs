using System.Reflection;
using System.Resources;
using System.Globalization;
using OneSet.Resx;
using Xamarin.Forms;

namespace OneSet.Localization
{
    // ReSharper disable once InconsistentNaming
	public class L10n
	{
		public static void SetLocale ()
		{
			DependencyService.Get<ILocale>().SetLocale();
		}

        public static string Locale ()
		{
			return DependencyService.Get<ILocale>().GetCurrent();
		}

		public static string Localize(string key, string comment) {

			var netLanguage = Locale ();

			// Platform-specific
			var temp = new ResourceManager("OneSet.Resx.AppResources", typeof(AppResources).GetTypeInfo().Assembly);
			string result = temp.GetString (key, new CultureInfo (netLanguage));

			return result; 
		}

		public static string GetWeightUnit()
		{
		    return App.Settings.IsMetric ? "Kgs" : "Lbs";
		}
	}
}

