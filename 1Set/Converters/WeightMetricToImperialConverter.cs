using System;
using System.Globalization;
using Xamarin.Forms;
using System.Diagnostics;

namespace Set
{

   
    public class MetricToImperialConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
//            // value always stored in metric
//            if (App.IsMetric)
//            {
//                return value;
//            }
//
//            // convert to imperial
//            float metric = (float)value;
//
//            // return today
//            if (date.Date == DateTime.Today)
//            {
//                return AppResources.Today;
//            }
//            else
//                // return tomorrow
//                if (date.Date == DateTime.Today.AddDays(1))
//                {
//                    return AppResources.Tomorrow;
//                }
//                // return yesterday
//                else
//                    if (date.Date == DateTime.Today.AddDays(-1))
//                    {
//                        return AppResources.Yesterday;
//                    }
//
//            // return date with default culture formatting
//            return date.ToString();
			return null;
        }
    

	public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
	{
		Debug.WriteLine(value.ToString(), new []{ "MetricToImperialConverter.ConvertBack"});
		throw new NotImplementedException ();
	}

	}
}
