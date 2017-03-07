using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using OneSet.Droid.Renders;
using OneSet.Controls;

[assembly: ExportRenderer (typeof (NoCapsButton), typeof (NoCapsButtonRender))]

namespace OneSet.Droid.Renders
{
	public class NoCapsButtonRender : ButtonRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged (e);
            var button = Control;
            button.SetAllCaps(false);
        }

	}
}

