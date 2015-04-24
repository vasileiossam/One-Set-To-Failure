using System;
using Xamarin.Forms;
using System.Diagnostics;
using System.Globalization;

namespace Set
{
	public class DoubleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;
			if ((double) value == 0) return string.Empty;
			return ((double)value).ToString();
        }
    
	    public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
	    {
			return value;
	    }
	}
}
