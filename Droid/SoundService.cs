using System;
using OneSet.Droid;
using Xamarin.Forms;
using Android.Media;
using OneSet.Abstract;

[assembly: Dependency (typeof (SoundService))]

namespace OneSet.Droid
{
	public class SoundService : ISoundService
	{
	    public void Play(string soundFileName)
		{
			// nothing worked...
			// var uri = Android.Net.Uri.Parse("android.resource://" + MainActivity.PACKAGE_NAME + "/raw/" + soundFileName);
			//var resId = Android.Content.Res.Resources.System.GetIdentifier (soundFileName, "raw", MainActivity.PACKAGE_NAME);

			int resId;
			if (soundFileName == "Bleep") 
			{
				resId = Resource.Raw.Bleep;
			}
			else 
			{
				throw new NotImplementedException();
			}

			var player = MediaPlayer.Create (Android.App.Application.Context, resId);
		    player?.Start ();
		}
	}
}

