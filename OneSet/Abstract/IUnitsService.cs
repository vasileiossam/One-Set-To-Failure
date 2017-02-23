namespace OneSet.Abstract
{
    public interface IUnitsService
    {
        double ImperialMetricFactor { get; }
        double WeightTolerance { get; }
        double GetWeight(bool isMetric, double value);
    }
}
