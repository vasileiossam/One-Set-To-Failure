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

			// make text bold
			if (Control != null) 
			{ 
				var view = (Control as TextView);

				if (e.NewElement != null)
				{
					var entry = e.NewElement as CustomEntry;

					if (entry.FontAttributes == FontAttributes.Bold)
					{
						view.SetTypeface (view.Typeface, Android.Graphics.TypefaceStyle.Bold);
					}

					if (entry.TextAlignment == Xamarin.Forms.TextAlignment.Center)
					{
						view.TextAlignment = Android.Views.TextAlignment.Center;
						view.Gravity = GravityFlags.Center;
					}



					double fontSize = 0;
					if (!double.TryParse (entry.FontSize, out fontSize))
					{
						fontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Entry));						
					}  
					view.TextSize = (float) fontSize;

				}
			}

			// autoselect text on focus
			if (e.OldElement == null) 
			{
				var nativeEditText = (global::Android.Widget.EditText)Control;
				nativeEditText.SetSelectAllOnFocus (true);
			}
		}
	}
}

