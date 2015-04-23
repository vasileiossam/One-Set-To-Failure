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
			//var fullDay = date.ToString ("ddd") + ", " + date.ToString ("D");
			var fullDay = date.ToString ("dddd d MMMM");

            // return today
            if (date.Date == DateTime.Today)
            {
				return string.Format("{0} ({1})", fullDay, AppResources.Today);
            }
            else
                // return tomorrow
                if (date.Date == DateTime.Today.AddDays(1))
                {
					return string.Format("{0} ({1})", fullDay, AppResources.Tomorrow);
                }
                // return yesterday
                else
                    if (date.Date == DateTime.Today.AddDays(-1))
                    {
						return string.Format("{0} ({1})", fullDay, AppResources.Yesterday);
                    }

            // return date with default culture formatting
			return fullDay;
        }


		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			Debug.WriteLine(value.ToString(), new []{ "DateTimeToStringConverter.ConvertBack"});
			throw new NotImplementedException ();
		}

    }
}
