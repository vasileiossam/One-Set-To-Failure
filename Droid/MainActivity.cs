using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using OxyPlot.Xamarin.Forms;
using Plugin.Toasts;
using Xamarin.Forms;

namespace Set.Droid
{
	[Activity (Label = "One Set To Fatigue", Icon = "@drawable/icon",  ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		public static String PACKAGE_NAME;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			PACKAGE_NAME = ApplicationContext.ApplicationInfo.PackageName;

			var currentDomain = AppDomain.CurrentDomain;
			currentDomain.UnhandledException += HandleUnhandledException;

			var bootstrapper = new Bootstrapper ();
			bootstrapper.Automapper ();

			OxyPlot.Xamarin.Forms.Platform.Android.Forms.Init();

			global::Xamarin.Forms.Forms.Init (this, bundle);

            DependencyService.Register<ToastNotificatorImplementation>();
            ToastNotificatorImplementation.Init(this);

            Context context = this.ApplicationContext;
			App.Version = context.PackageManager.GetPackageInfo(context.PackageName, PackageInfoFlags.Activities).VersionName;
			App.ScreenWidth = Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density;
			App.ScreenHeight = Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density;
		
			LoadApplication(new App ());

			bootstrapper.CheckMapper ();
		}

		static void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			//Exception d = (Exception)e.ExceptionObject;
			Console.WriteLine("TEST");
		}

		protected override void Dispose (bool disposing)
		{
			var currentDomain = AppDomain.CurrentDomain;
			currentDomain.UnhandledException -= HandleUnhandledException;
			base.Dispose (disposing);
		}
	}
}

