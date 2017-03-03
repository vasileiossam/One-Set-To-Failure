namespace OneSet.Abstract
{
    public interface IUnitsService
    {
        double ImperialMetricFactor { get; }
        double WeightTolerance { get; }

        double GetMetric(bool systemIsMetric, double value);
        double GetWeight(bool systemIsMetric, double weightInMetric);
    }
}
