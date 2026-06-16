using UnitConversionApi.Core.Exceptions;
using UnitConversionApi.Core.Interfaces;

namespace UnitConversionApi.Services.Services;

public class ConversionService : IConversionService
{
    private readonly IUnitRegistry _unitRegistry;

    public ConversionService(IUnitRegistry unitRegistry)
    {
        _unitRegistry = unitRegistry;
    }

    public double Convert(double value, string fromSymbol, string toSymbol)
    {
        var fromUnit = _unitRegistry.GetUnitBySymbol(fromSymbol);
        var toUnit = _unitRegistry.GetUnitBySymbol(toSymbol);

        if (fromUnit == null)
            throw new ConversionException($"Source unit '{fromSymbol}' is not supported.");

        if (toUnit == null)
            throw new ConversionException($"Target unit '{toSymbol}' is not supported.");

        if (fromUnit.Category != toUnit.Category)
            throw new ConversionException($"Cannot convert between different categories ({fromUnit.Category} to {toUnit.Category}).");

        // Convert Source to Base
        double valueInBase = fromUnit.ToBase(value);

        // Convert Base to Target
        double result = toUnit.FromBase(valueInBase);

        return result;
    }
}
