using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Set.Droid.Renders;
using Set.Controls;

[assembly: ExportRenderer (typeof (BlueButton), typeof (BlueButtonRender))]

namespace Set.Droid.Renders
{
	public class BlueButtonRender : ButtonRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged (e);

			if (Control != null) 
			{ 
				Control.SetBackgroundResource (Resource.Drawable.ButtonBlue);
			}
		}

	}
}

