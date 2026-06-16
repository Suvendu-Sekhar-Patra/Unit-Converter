namespace UnitConversionApi.Core.Models;

public class UnitDefinition
{
    public string Name { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public UnitCategory Category { get; set; }
    
    // Function to convert from this unit to the base unit of the category
    public Func<double, double> ToBase { get; set; } = x => x;

    // Function to convert from the base unit of the category to this unit
    public Func<double, double> FromBase { get; set; } = x => x;
}
