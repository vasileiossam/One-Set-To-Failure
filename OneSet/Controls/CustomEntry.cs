using Xamarin.Forms;
using static Xamarin.Forms.BindableProperty;

namespace OneSet.Controls
{
	public class CustomEntry : Entry
	{
	    public static readonly BindableProperty TextAlignmentProperty = Create<CustomEntry, TextAlignment> (p => p.TextAlignment, TextAlignment.Start);

		public TextAlignment TextAlignment 
		{
			get { return (TextAlignment)GetValue (TextAlignmentProperty); }
			set { SetValue (TextAlignmentProperty, value); }
		}

	}
}

