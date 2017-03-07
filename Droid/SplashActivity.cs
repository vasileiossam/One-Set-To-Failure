using Android.App;
using Android.Content;
using Android.OS;
using Android.Content.PM;
using System.Threading.Tasks;

namespace OneSet.Droid
{
	[Activity(
        Label = "One Set To Failure", 
        Icon = "@drawable/icon", 
        Theme = "@style/MyTheme.Splash", 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, 
        MainLauncher = true, NoHistory = true)]
	public class SplashActivity : Activity
	{
		protected override async void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			var intent = new Intent(this, typeof(MainActivity));
			StartActivity(intent);

			// following statement will prevent a compiler warning about async method lacking await
			await Task.FromResult(0);
		}
	}
}

