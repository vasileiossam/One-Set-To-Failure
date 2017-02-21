using System;
using System.Globalization;
using Xamarin.Forms;

namespace OneSet.Converters
{
    public class IntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;
			return (int) value == 0 ? string.Empty : ((int)value).ToString();
        }
    
	    public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
	    {
			return value;
	    }
	}
}
