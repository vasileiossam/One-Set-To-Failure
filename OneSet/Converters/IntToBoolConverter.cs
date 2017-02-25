using System;
using System.Globalization;
using Xamarin.Forms;

namespace OneSet.Converters
{
    public class IntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            if (value == null) return false;
			return (int) value != 0;
        }
    
	    public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
	    {
	        if ((bool) value) return 1;
	        return 0;
	    }
	}
}
