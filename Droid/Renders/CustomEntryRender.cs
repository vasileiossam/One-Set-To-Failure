using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using OneSet.Droid.Renders;
using Android.Views;
using Android.Widget;
using OneSet.Controls;

[assembly: ExportRenderer (typeof (CustomEntry), typeof (CustomEntryRender))]

namespace OneSet.Droid.Renders
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

					if (entry != null && entry.FontAttributes == FontAttributes.Bold)
					{
						view.SetTypeface (view.Typeface, Android.Graphics.TypefaceStyle.Bold);
					}

					if (entry != null && entry.TextAlignment == Xamarin.Forms.TextAlignment.Center)
					{
						view.TextAlignment = Android.Views.TextAlignment.Center;
						view.Gravity = GravityFlags.Center;
					}

				    if (entry != null)
				    {
				        var fontSize = entry.FontSize;
				        if (fontSize <= 0)
				        {
				            fontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Entry));						
				        }  
				        view.TextSize = (float) fontSize;
				    }
				    //view.InputType = Android.Text.InputTypes.TextFlagNoSuggestions;

				}
			}

			// autoselect text on focus
		    if (e.OldElement != null) return;
		    var nativeEditText = (EditText)Control;
		    nativeEditText?.SetSelectAllOnFocus (true);
		}
	}
}

