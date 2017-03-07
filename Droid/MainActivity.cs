using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Autofac;
using Plugin.Toasts;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using OneSet.Data;

namespace OneSet.Droid
{
	[Activity (
        Label = "One Set To Failure",
        Icon = "@drawable/icon",
        Theme = "@style/MyTheme",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : FormsAppCompatActivity
    {
		protected override void OnCreate (Bundle bundle)
		{
            // set the layout resources first
            ToolbarResource = Resource.Layout.toolbar;
            TabLayoutResource = Resource.Layout.tabs;

            base.OnCreate (bundle);

            var currentDomain = AppDomain.CurrentDomain;
			currentDomain.UnhandledException += HandleUnhandledException;

			OxyPlot.Xamarin.Forms.Platform.Android.Forms.Init();
			Forms.Init (this, bundle);
		    
            DependencyService.Register<ToastNotification>();
            ToastNotification.Init(this, new PlatformOptions { SmallIconDrawable = Android.Resource.Drawable.IcDialogInfo });

            var context = ApplicationContext;
			App.Version = context.PackageManager.GetPackageInfo(context.PackageName, PackageInfoFlags.Activities).VersionName;
			App.ScreenWidth = Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density;
			App.ScreenHeight = Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density;

            var bootstrapper = new Bootstrapper();
            App.Container = bootstrapper.CreateContainer();

            LoadApplication(new App());
		}

        private static void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
            // TODO handle exception
		}

		protected override void Dispose (bool disposing)
		{
			var currentDomain = AppDomain.CurrentDomain;
			currentDomain.UnhandledException -= HandleUnhandledException;
			base.Dispose (disposing);
		}
	}
}

