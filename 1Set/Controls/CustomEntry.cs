using Xamarin.Forms;

namespace Set.Controls
{
	public class CustomEntry : Entry
	{
		public CustomEntry ()
		{
			 
		}

		public static readonly BindableProperty TextAlignmentProperty = BindableProperty.Create<CustomEntry, TextAlignment> (p => p.TextAlignment, TextAlignment.Start);

		public TextAlignment TextAlignment 
		{
			get { return (TextAlignment)GetValue (TextAlignmentProperty); }
			set { SetValue (TextAlignmentProperty, value); }
		}

	}
}

