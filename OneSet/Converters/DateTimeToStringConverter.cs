using System;
using System.Diagnostics;
using System.Globalization;
using OneSet.Resx;
using Xamarin.Forms;

namespace OneSet.Converters
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            var date = (DateTime)value;
			//var fullDay = date.ToString ("ddd") + ", " + date.ToString ("D");
			var dayStr = date.ToString ("ddd, d MMMM");

            // return today
            if (date.Date == DateTime.Today)
            {
				return AppResources.Today;
            }
            else
                // return tomorrow
                if (date.Date == DateTime.Today.AddDays(1))
                {
					return AppResources.Tomorrow;
                }
                // return yesterday
                else
                    if (date.Date == DateTime.Today.AddDays(-1))
                    {
						return AppResources.Yesterday;
                    }

			return dayStr;
        }


		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			Debug.WriteLine(value.ToString(), new []{ "DateTimeToStringConverter.ConvertBack"});
			throw new NotImplementedException ();
		}

    }
}
