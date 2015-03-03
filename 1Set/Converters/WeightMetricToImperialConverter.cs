using System;
using System.Globalization;
using Xamarin.Forms;
using System.Diagnostics;

namespace Set
{

   
    /// <summary>
    /// http://stackoverflow.com/questions/12977021/best-practice-for-storing-weights-in-a-sql-database
    ///	https://msdn.microsoft.com/en-us/library/windows/apps/hh969150.aspx
    /// </summary>
	public class WeightMetricToImperialConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;
			if ((double) value == 0) return string.Empty;

            // metric to metric
            // we always store weight in metric (Kg), if we are already in metric no conversion is needed
			if (App.Settings.IsMetric == 1)
            {
                return value;
            }
            
            // metric to imperial
            // 1 Kg = 2.20462 lbs
            double kgs = (double)value;
            double lbs = kgs * 2.20462;
            return lbs;
        }
    

	public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
	{
		Debug.WriteLine(value.ToString(), new []{ "MetricToImperialConverter.ConvertBack"});
		throw new NotImplementedException ();
	}

	}
}
