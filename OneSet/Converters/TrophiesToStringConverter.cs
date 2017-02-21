using System;
using System.Globalization;
using Xamarin.Forms;

namespace OneSet
{
    /// <summary>
    /// http://stackoverflow.com/questions/12977021/best-practice-for-storing-weights-in-a-sql-database
    ///	https://msdn.microsoft.com/en-us/library/windows/apps/hh969150.aspx
    /// </summary>
	public class TrophiesToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
			if (value == null) return string.Empty;
			if ((int) value == 0) return string.Empty;

			if ((int)value > 0)
			{
				return "+" + ((int)value);
			}
            return ((int)value).ToString ();
        }
    

		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}
