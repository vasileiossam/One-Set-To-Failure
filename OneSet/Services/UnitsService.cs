using System;
using OneSet.Abstract;

namespace OneSet.Services
{
    public class UnitsService : IUnitsService
    {
        public double ImperialMetricFactor => 2.20462;
        public double WeightTolerance => 0.0001;

        public double GetMetric(bool systemIsMetric, double value)
        {
            // value is in metric
            if (systemIsMetric)
            {
                return value;
            }

            // value is in imperial, convert to metric 
            return value / ImperialMetricFactor;
        }

        /// <summary>
        /// Convert metric weight to the current units system
        /// </summary>
        public double GetWeight(bool systemIsMetric, double weightInMetric)
		{
            // no need to convert to metric because weightInMetric should already be in metric
            if (systemIsMetric)
			{
				return Math.Round (weightInMetric, 2);
			}

		    // convert metric to imperial
		    return Math.Round (weightInMetric * ImperialMetricFactor, 2);
		}
	}
}

