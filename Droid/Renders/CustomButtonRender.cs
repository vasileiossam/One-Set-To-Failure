using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Set.Droid.Renders;
using Set.Controls;

[assembly: ExportRenderer (typeof (CustomButton), typeof (CustomButtonRender))]

namespace Set.Droid.Renders
{
	public class CustomButtonRender : ButtonRenderer
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

