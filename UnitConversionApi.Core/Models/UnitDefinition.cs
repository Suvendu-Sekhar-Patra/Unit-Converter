namespace UnitConversionApi.Core.Models;

public class UnitDefinition
{
    public string Name { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public UnitCategory Category { get; set; }
    public double BaseOffset { get; set; } = 0.0;
    public double BaseMultiplier { get; set; } = 1.0;

    public double ToBase(double value)
    {
        return (value + BaseOffset) * BaseMultiplier;
    }

    public double FromBase(double value)
    {
        return (value / BaseMultiplier) - BaseOffset;
    }
}
