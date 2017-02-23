using System;
using OneSet.Abstract;

namespace OneSet.Services
{
    public class UnitsService : IUnitsService
    {
        public double ImperialMetricFactor => 2.20462;
        public double WeightTolerance => 0.0001;
		
		public double GetWeight(bool isMetric, double value)
		{
			if (isMetric)
			{
				return Math.Round (value, 2);
			}

		    // metric to imperial
		    return Math.Round (value * ImperialMetricFactor, 2);
		}
	}
}

