using System;
using System.Globalization;
using System.Threading.Tasks;
using Autofac;
using OneSet.Abstract;
using OneSet.Data;
using OneSet.Localization;
using OneSet.Models;
using OneSet.Resx;
using OneSet.ViewModels;
using OneSet.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace OneSet
{
	public partial class App : Application
	{
        public static IContainer Container { get; set; }

	    public static Database Database { get; set; }
	    public static DateTime CurrentDate {get; set;}
		public static Settings Settings { get; set; }
		public static string Version {get; set;}
		public static double ScreenWidth {get; set;}
		public static double ScreenHeight {get; set;}
		public static int? TotalTrophies {get; set;}
        
		public App ()
		{
            InitializeComponent();

            Settings = DependencyService.Get<ISettingsStorage>().Load();
			L10n.SetLocale ();

			var netLanguage = DependencyService.Get<ILocale>().GetCurrent();
		    AppResources.Culture = new CultureInfo (netLanguage);

            Database = Container.Resolve<Database>();
            
            var page = Container.Resolve<MainPage>();
            var navigationService = Container.Resolve<IMasterDetailNavigation>();
            navigationService.InitNavigation(page);

            Task.Run(async () =>
            {
                await navigationService.NavigateToDetail<WorkoutsViewModel>();
            }).Wait();

            MainPage = page;
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

        public static void ShowErrorPage(object sender, Exception ex)
		{
			var viewModel = new ErrorViewModel () { Sender = sender, Exception = ex };

			var mainPage = Current.MainPage;
			if (mainPage != null)
			{
				mainPage.Navigation.PushAsync (new ErrorPage { ViewModel = viewModel });
			} 
			else
			{
				var mainNav = new NavigationPage (new ErrorPage { ViewModel = viewModel });
				Current.MainPage = mainNav;
                
			}
		}
	}
}