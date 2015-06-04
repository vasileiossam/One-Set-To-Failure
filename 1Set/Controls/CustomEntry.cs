using Xamarin.Forms;

namespace Set.Controls
{
	public class CustomEntry : Entry
	{
		public CustomEntry ()
		{
			 
		}

		public static readonly BindableProperty FontSizeProperty = BindableProperty.Create<CustomEntry, string>(p => p.FontSize, "24");

		public string FontSize 
		{
			get { return (string)GetValue (FontSizeProperty); }
			set { SetValue (FontSizeProperty, value); }
		}

		public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create<CustomEntry, FontAttributes> (p => p.FontAttributes, FontAttributes.None);

		public FontAttributes FontAttributes 
		{
			get { return (FontAttributes)GetValue (FontAttributesProperty); }
			set { SetValue (FontAttributesProperty, value); }
		}

		public static readonly BindableProperty TextAlignmentProperty = BindableProperty.Create<CustomEntry, TextAlignment> (p => p.TextAlignment, TextAlignment.Start);

		public TextAlignment TextAlignment 
		{
			get { return (TextAlignment)GetValue (TextAlignmentProperty); }
			set { SetValue (TextAlignmentProperty, value); }
		}

	}
}

