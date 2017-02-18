using System;
using System.Globalization;
using Xamarin.Forms;
using System.Diagnostics;
using OneSet.Resx;

namespace OneSet
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
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
