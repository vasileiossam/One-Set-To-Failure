using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading;
using Android.Content.PM;
using System.Threading.Tasks;

namespace OneSet.Droid
{
	[Activity(Label = "One Set To Fatigue", Icon = "@drawable/icon", Theme = "@style/Theme.Splash", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, MainLauncher = true, NoHistory = true)]
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

