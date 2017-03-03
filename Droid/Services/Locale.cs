using System;
using System.Threading;
using OneSet.Droid.Services;
using OneSet.Localization;
using Xamarin.Forms;

[assembly:Dependency(typeof(Locale))]

namespace OneSet.Droid.Services
{
	public class Locale : ILocale
	{
		public void SetLocale (){

			var androidLocale = Java.Util.Locale.Default; // user's preferred locale
			var netLocale = androidLocale.ToString().Replace ("_", "-"); 
			var ci = new System.Globalization.CultureInfo (netLocale);
			Thread.CurrentThread.CurrentCulture = ci;
			Thread.CurrentThread.CurrentUICulture = ci;
		}

		/// <remarks>
		/// Not sure if we can cache this info rather than querying every time
		/// </remarks>
		public string GetCurrent() 
		{
			var androidLocale = Java.Util.Locale.Default; // user's preferred locale

			// en, es, ja
			var netLanguage = androidLocale.Language.Replace ("_", "-"); 
			// en-US, es-ES, ja-JP
			var netLocale = androidLocale.ToString().Replace ("_", "-"); 

			#region Debugging output
			Console.WriteLine (@"android:  " + androidLocale);
			Console.WriteLine (@"netlang:  " + netLanguage);
			Console.WriteLine (@"netlocale:" + netLocale);

			var ci = new System.Globalization.CultureInfo (netLocale);
			Thread.CurrentThread.CurrentCulture = ci;
			Thread.CurrentThread.CurrentUICulture = ci;

			Console.WriteLine (@"thread:  "+Thread.CurrentThread.CurrentCulture);
			Console.WriteLine (@"threadui:"+Thread.CurrentThread.CurrentUICulture);
			#endregion

			return netLocale;
		}
	}
}

