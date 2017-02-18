using System;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using System.Diagnostics;
using OneSet.Concrete;

namespace OneSet.Localization
{
	// You exclude the 'Extension' suffix when using in Xaml markup
	[ContentProperty ("Text")]
	public class TranslateExtension : IMarkupExtension
	{
		public string Text { get; set; }

		public object ProvideValue (IServiceProvider serviceProvider)
		{
			if (Text == null) return null;

			// Do your translation lookup here, using whatever method you require
			var translated = L10n.Localize (Text, Text); 

			return translated;
		}
	}
}

