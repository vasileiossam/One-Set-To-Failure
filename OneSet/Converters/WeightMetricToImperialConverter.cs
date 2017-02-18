using System;
using System.Globalization;
using Xamarin.Forms;

namespace OneSet
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
			return GetWeight (value);
        }
    
		public object GetWeight(object value)
		{
			// metric to metric
			// we always store weight in metric (Kg), if we are already in metric no conversion is needed
			if (App.Settings.IsMetric)
			{
				value = Math.Round ((double) value, 2);
				return value;
			}

			// metric to imperial
			// 1 Kg = 2.20462 lbs
			double kgs = (double)value;
			double lbs = kgs * 2.20462;
			return Math.Round (lbs, 2);
		}

		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
				return value;
		}

		public static object GetWeight(double weight)
		{
			var converter = new WeightMetricToImperialConverter ();
			return converter.Convert (weight, typeof(double), null, CultureInfo.CurrentCulture);
		}

		public static double GetWeightAsDouble(double weight)
		{
			var converter = new WeightMetricToImperialConverter ();
			var value = converter.Convert (weight, typeof(double), null, CultureInfo.CurrentCulture);
			if (value is double) return (double) value;
			return 0;
		}
	}

}
