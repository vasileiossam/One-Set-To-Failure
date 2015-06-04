using System;
using System.Globalization;
using Xamarin.Forms;

namespace Set
{
    public class IntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;
			if ((int) value == 0) return string.Empty;
            return ((int)value).ToString();
        }
    
	    public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
	    {
			return value;
	    }
	}
}
