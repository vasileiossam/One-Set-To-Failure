using System;
using System.Globalization;
using OneSet.ViewModels;
using Xamarin.Forms;

namespace OneSet.Converters
{
	public class StateToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType,
			object parameter, CultureInfo culture)
		{
		    return (RestTimerStates) parameter == (RestTimerStates?) value;
		}

		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}
