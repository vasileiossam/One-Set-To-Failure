using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using OneSet.Droid.Renders;
using OneSet.Controls;

[assembly: ExportRenderer (typeof (BlueButton), typeof (BlueButtonRender))]

namespace OneSet.Droid.Renders
{
	public class BlueButtonRender : ButtonRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged (e);

		    Control?.SetBackgroundResource (Resource.Drawable.ButtonBlue);
		}

	}
}

