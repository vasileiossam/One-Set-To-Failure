using System;
using Set.Abstract;
using Xamarin.Forms;
using System.Globalization;
using Set.Models;
using System.Reflection;
using Set.Localization;
using Toasts.Forms.Plugin.Abstractions;
using Set.Resx;
using System.Threading.Tasks;
using Set.ViewModels;
using Set.Views;

namespace Set
{
	public class App : Application
	{
		public static Database Database = new Database ();
		public static DateTime CurrentDate {get; set;}
		public static Settings Settings { get; set; }
		public static string Version {get; set;}
		public static double ScreenWidth {get; set;}
		public static double ScreenHeight {get; set;}

		public App ()
		{	
			
			Settings = DependencyService.Get<ISettingsStorage>().Load();
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
			bool tapped = await notificator.Notify(type, header, message, TimeSpan.FromSeconds(1.5));
		}

		public static void SaveSettings()
		{
			DependencyService.Get<ISettingsStorage> ().Save(Settings);
		}

		public static void ShowErrorPage(object sender, Exception ex)
		{
			var viewModel = new ErrorViewModel () { Sender = sender, Exception = ex };

			var mainPage = App.Current.MainPage;
			if (mainPage != null)
			{
				mainPage.Navigation.PushAsync (new ErrorPage (){ ViewModel = viewModel });
			} 
			else
			{
				var mainNav = new NavigationPage (new ErrorPage (){ ViewModel = viewModel });
				App.Current.MainPage = mainNav;
			}
		}

	}
}