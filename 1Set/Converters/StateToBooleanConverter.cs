using System;
using System.Globalization;
using Xamarin.Forms;

namespace Set
{
	public class StateToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType,
			object parameter, CultureInfo culture)
		{
			if (value == null) return false;
			return (RestTimerStates) parameter == (RestTimerStates) value;
		}

		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}
