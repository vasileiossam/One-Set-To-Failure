using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Set.Droid.Renders;
using Android.Views;
using Android.Widget;
using Set.Controls;

[assembly: ExportRenderer (typeof (CustomEntry), typeof (CustomEntryRender))]

namespace Set.Droid.Renders
{
	public class CustomEntryRender : EntryRenderer
	{

		protected override void OnElementChanged (ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged (e);

			if (Control != null) 
			{ 
				var view = (Control as TextView);
				view.SetTypeface (view.Typeface, Android.Graphics.TypefaceStyle.Bold);
			}
		}
	}
}

