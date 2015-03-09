using System;
using Set.Abstract;
using Xamarin.Forms;
using System.Globalization;
using Set.Models;
using System.Reflection;
using Set.Localization;
using Toasts.Forms.Plugin.Abstractions;
using Set.Resx;

namespace Set
{
	public class App : Application
	{
        static Database _database;
        public static Database Database
        {
            get
            {
                if (_database == null)
                {
                    _database = new Database();
                }
                return _database;
            }
        }

		public static DateTime CurrentDate {get; set;}
		public static Settings Settings { get; set; }

		public App ()
		{	
			Settings = DependencyService.Get<ISettingsStorage>().LoadAsync().Result;
			L10n.SetLocale ();

			var netLanguage = DependencyService.Get<ILocale>().GetCurrent();
		    AppResources.Culture = new CultureInfo (netLanguage);
					 
	    	var mainNav = new NavigationPage (new WorkoutListPage ());

			mainNav.BarBackgroundColor = ColorPalette.Primary;
			mainNav.BarTextColor = ColorPalette.Icons;

        	MainPage = mainNav;
		}

		protected override void OnStart ()
		{
			CurrentDate = DateTime.Today;
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

		public static async void ShowToast(ToastNotificationType type, string header, string message)
		{
			var notificator = DependencyService.Get<IToastNotificator>();
			bool tapped = await notificator.Notify(type, header, message, TimeSpan.FromSeconds(2));
		}
	}
}