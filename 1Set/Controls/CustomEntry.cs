using System;
using Xamarin.Forms;

namespace Set.Controls
{
	public class CustomEntry : Entry
	{
		public CustomEntry ()
		{
			 
		}

		public static readonly BindableProperty FontSizeProperty = BindableProperty.Create<CustomEntry, double> (p => p.FontSize, 24);

		public double FontSize 
		{
			get { return (double)GetValue (FontSizeProperty); }
			set { SetValue (FontSizeProperty, value); }
		}

		public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create<CustomEntry, FontAttributes> (p => p.FontAttributes, FontAttributes.None);

		public FontAttributes FontAttributes 
		{
			get { return (FontAttributes)GetValue (FontAttributesProperty); }
			set { SetValue (FontAttributesProperty, value); }
		}

		public static readonly BindableProperty TextAlignmentProperty = BindableProperty.Create<CustomEntry, Xamarin.Forms.TextAlignment> (p => p.TextAlignment, Xamarin.Forms.TextAlignment.Start);

		public Xamarin.Forms.TextAlignment TextAlignment 
		{
			get { return (Xamarin.Forms.TextAlignment)GetValue (TextAlignmentProperty); }
			set { SetValue (TextAlignmentProperty, value); }
		}

	}
}

