using System;
using System.Diagnostics;
using System.Globalization;
using Set.Localization;
using Set.Models;
using Set.Resx;
using Set.ViewModels;
using Set.Views;
using Toasts.Forms.Plugin.Abstractions;
using Xamarin.Forms;

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
		public static int? TotalTrophies {get; set;}

		public static int RestTimerSecondsLeft { get; set; }
			
		public App ()
		{	
			//Debugger.Break ();
			Settings = DependencyService.Get<ISettingsStorage>().Load();
			L10n.SetLocale ();

			var netLanguage = DependencyService.Get<ILocale>().GetCurrent();
		    AppResources.Culture = new CultureInfo (netLanguage);

		    var mainNav = new NavigationPage(new WorkoutListPage())
		    {
		        BarBackgroundColor = ColorPalette.Primary,
		        BarTextColor = ColorPalette.Icons
		    };

		    MainPage = mainNav;
		}

		protected override void OnStart ()
		{
			CurrentDate = DateTime.Today;
			RestTimerSecondsLeft = 0;
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
		    await notificator.Notify(type, header, message, TimeSpan.FromSeconds(1.5));
		}

	    public static void SaveSettings()
		{
			DependencyService.Get<ISettingsStorage> ().Save(Settings);
		}

		public static void ShowErrorPage(object sender, Exception ex)
		{
			var viewModel = new ErrorViewModel () { Sender = sender, Exception = ex };

			var mainPage = Current.MainPage;
			if (mainPage != null)
			{
				mainPage.Navigation.PushAsync (new ErrorPage (){ ViewModel = viewModel });
			} 
			else
			{
				var mainNav = new NavigationPage (new ErrorPage (){ ViewModel = viewModel });
				Current.MainPage = mainNav;
			}
		}
	}
}