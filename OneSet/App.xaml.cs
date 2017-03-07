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
using Plugin.Toasts;
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
	    public static RestTimerItem RestTimerItem;
			
		public App ()
		{
            InitializeComponent();

            Settings = DependencyService.Get<ISettingsStorage>().Load();
			L10n.SetLocale ();

			var netLanguage = DependencyService.Get<ILocale>().GetCurrent();
		    AppResources.Culture = new CultureInfo (netLanguage);

            RestTimerItem = new RestTimerItem(Container.Resolve<ISoundService>());

            Database = Container.Resolve<Database>();

            var navigateService = Container.Resolve<INavigationService>();
            MainPage = navigateService.InitNavigation<MainViewModel>();
		}

		protected override void OnStart ()
		{
            CurrentDate = DateTime.Today;
		    RestTimerItem.Reset();
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

        #region Toast messages
        public static async Task ShowWarning(string message)
	    {
	        await ShowToast(AppResources.ToastWarningTitle, message);
	    }
        public static async Task ShowError(string title, string message)
        {
            await ShowToast($"{AppResources.ToastErrorTitle}: {title}", message);
        }
        public static async Task ShowError(string message)
        {
            await ShowToast(AppResources.ToastErrorTitle, message);
        }
        public static async Task ShowInfo(string message)
        {
            await ShowToast(AppResources.ToastInfoTitle, message);
        }
        public static async Task ShowSuccess(string message)
        {
            await ShowToast(AppResources.ToastSuccessTitle, message);
        }
        public static async Task ShowToast(string title, string message)
		{
		    var notificator = DependencyService.Get<IToastNotificator>();
            var options = new NotificationOptions
            {
                Title = title,
                Description = message,
                IsClickable = true,
            };
            await notificator.Notify(options);
		}
        #endregion

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