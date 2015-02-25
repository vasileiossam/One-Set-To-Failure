using System;
using Set.Abstract;
using Xamarin.Forms;
using System.Globalization;
using Set.Models;
using System.Reflection;

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

		public static Settings Settings { get; set; }

		public App ()
		{	
			Settings = DependencyService.Get<ISettingsStorage>().LoadAsync().Result;
			L10n.SetLocale ();

			var netLanguage = DependencyService.Get<ILocale>().GetCurrent();
		    AppResources.Culture = new CultureInfo (netLanguage);

			var assembly = typeof(App).GetTypeInfo().Assembly;
			foreach (var res in assembly.GetManifestResourceNames()) 
				System.Diagnostics.Debug.WriteLine("found resource: " + res);

			var mainNav = new NavigationPage (new WorkoutListPage ());
        	MainPage = mainNav;
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}