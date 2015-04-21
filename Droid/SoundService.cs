using System;
using Set.Models;
using System.Threading.Tasks;
using Set.Droid;
using Xamarin.Forms;
using System.IO;
using Android.Media;

[assembly: Dependency (typeof (SoundService))]

namespace Set.Droid
{
	public class SoundService : ISoundService
	{

		public SoundService ()
		{

		}

		public void Play(string soundFileName)
		{
			//var uri = Android.Net.Uri.Parse("android.resource://" + MainActivity.PACKAGE_NAME + "/raw/" + soundFileName);
			var uri = Android.Net.Uri.Parse("android.resource://" + MainActivity.PACKAGE_NAME + "/" + soundFileName + ".mp3");
			var player = MediaPlayer.Create (Android.App.Application.Context, uri);
			if (player != null)
			{
				player.Start ();
			}
		}
	}
}

