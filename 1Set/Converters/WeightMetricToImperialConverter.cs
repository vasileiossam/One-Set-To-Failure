using System;
using System.Globalization;
using Xamarin.Forms;

namespace Set
{

    - always store in known format
    - do the conversion

    // think save or convert?


    http://stackoverflow.com/questions/12977021/best-practice-for-storing-weights-in-a-sql-database
    https://msdn.microsoft.com/en-us/library/windows/apps/hh969150.aspx

    public class MetricToImperialConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            // value always stored in metric
            if (App.IsMetric)
            {
                return value;
            }

            // convert to imperial
            float metric = (float)value;

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
    }
}
