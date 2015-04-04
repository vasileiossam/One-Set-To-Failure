using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Toasts.Forms.Plugin.Droid;

namespace Set.Droid
{
	[Activity (Label = "One Set To Fatigue", Icon = "@drawable/icon",  ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			var currentDomain = AppDomain.CurrentDomain;
			currentDomain.UnhandledException += HandleUnhandledException;

			var bootstrapper = new Bootstrapper ();
			bootstrapper.Automapper ();

			global::Xamarin.Forms.Forms.Init (this, bundle);

			ToastNotificatorImplementation.Init();

			Context context = this.ApplicationContext;
			App.Version = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;

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

