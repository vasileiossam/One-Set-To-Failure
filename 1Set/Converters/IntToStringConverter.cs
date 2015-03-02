using System;
using Xamarin.Forms;
using System.Diagnostics;

namespace Set
{
    public class IntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;
            if (value == 0) return string.Empty;
            return ((int)value).ToString();
        }
    
	    public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
	    {
            Debug.WriteLine(value.ToString(), new[] { "IntToStringConverter.ConvertBack" });
		    throw new NotImplementedException ();
	    }
	}
}
