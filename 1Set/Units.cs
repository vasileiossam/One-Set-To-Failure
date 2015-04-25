using System;

namespace Set
{
	public class Units
	{
		public const double ImperialMetricFactor = 2.20462;
		public const double WeightTolerance = 0.0001;
			
		public static double GetWeight(double value)
		{
			if (App.Settings.IsMetric)
			{
				return Math.Round (value, 2);
			} else
			{
				// metric to imperial
				return Math.Round (value * ImperialMetricFactor, 2);
			}
		}
	}
}

