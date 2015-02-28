using System;
using System.Globalization;
using Xamarin.Forms;
using System.Diagnostics;
using Set.Resx;

namespace Set
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;

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

            // return date with default culture formatting
            return date.ToString();
        }


		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			Debug.WriteLine(value.ToString(), new []{ "DateTimeToStringConverter.ConvertBack"});
			throw new NotImplementedException ();
		}

    }
}
