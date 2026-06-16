using UnitConversionApi.Core.Interfaces;
using UnitConversionApi.Core.Models;

namespace UnitConversionApi.Services.Registry;

public class UnitRegistry : IUnitRegistry
{
    private readonly Dictionary<string, UnitDefinition> _units;

    public UnitRegistry()
    {
        _units = new Dictionary<string, UnitDefinition>(StringComparer.OrdinalIgnoreCase);
        InitializeUnits();
    }

    private void InitializeUnits()
    {
        // Length (Base Unit: Meter)
        AddUnit(new UnitDefinition { Name = "Meter", Symbol = "m", Category = UnitCategory.Length, ToBase = x => x, FromBase = x => x });
        AddUnit(new UnitDefinition { Name = "Kilometer", Symbol = "km", Category = UnitCategory.Length, ToBase = x => x * 1000, FromBase = x => x / 1000 });
        AddUnit(new UnitDefinition { Name = "Centimeter", Symbol = "cm", Category = UnitCategory.Length, ToBase = x => x / 100, FromBase = x => x * 100 });
        AddUnit(new UnitDefinition { Name = "Millimeter", Symbol = "mm", Category = UnitCategory.Length, ToBase = x => x / 1000, FromBase = x => x * 1000 });
        AddUnit(new UnitDefinition { Name = "Mile", Symbol = "mi", Category = UnitCategory.Length, ToBase = x => x * 1609.344, FromBase = x => x / 1609.344 });
        AddUnit(new UnitDefinition { Name = "Yard", Symbol = "yd", Category = UnitCategory.Length, ToBase = x => x * 0.9144, FromBase = x => x / 0.9144 });
        AddUnit(new UnitDefinition { Name = "Foot", Symbol = "ft", Category = UnitCategory.Length, ToBase = x => x * 0.3048, FromBase = x => x / 0.3048 });
        AddUnit(new UnitDefinition { Name = "Inch", Symbol = "in", Category = UnitCategory.Length, ToBase = x => x * 0.0254, FromBase = x => x / 0.0254 });

        // Mass (Base Unit: Kilogram)
        AddUnit(new UnitDefinition { Name = "Kilogram", Symbol = "kg", Category = UnitCategory.Mass, ToBase = x => x, FromBase = x => x });
        AddUnit(new UnitDefinition { Name = "Gram", Symbol = "g", Category = UnitCategory.Mass, ToBase = x => x / 1000, FromBase = x => x * 1000 });
        AddUnit(new UnitDefinition { Name = "Milligram", Symbol = "mg", Category = UnitCategory.Mass, ToBase = x => x / 1000000, FromBase = x => x * 1000000 });
        AddUnit(new UnitDefinition { Name = "Pound", Symbol = "lb", Category = UnitCategory.Mass, ToBase = x => x * 0.45359237, FromBase = x => x / 0.45359237 });
        AddUnit(new UnitDefinition { Name = "Ounce", Symbol = "oz", Category = UnitCategory.Mass, ToBase = x => x * 0.02834952, FromBase = x => x / 0.02834952 });

        // Temperature (Base Unit: Celsius)
        AddUnit(new UnitDefinition { Name = "Celsius", Symbol = "c", Category = UnitCategory.Temperature, ToBase = x => x, FromBase = x => x });
        AddUnit(new UnitDefinition { Name = "Fahrenheit", Symbol = "f", Category = UnitCategory.Temperature, ToBase = x => (x - 32) * 5.0 / 9.0, FromBase = x => x * 9.0 / 5.0 + 32 });
        AddUnit(new UnitDefinition { Name = "Kelvin", Symbol = "k", Category = UnitCategory.Temperature, ToBase = x => x - 273.15, FromBase = x => x + 273.15 });
    }

    private void AddUnit(UnitDefinition unit)
    {
        _units[unit.Symbol] = unit;
    }

    public IEnumerable<UnitDefinition> GetAllUnits()
    {
        return _units.Values;
    }

    public UnitDefinition? GetUnitBySymbol(string symbol)
    {
        _units.TryGetValue(symbol, out var unit);
        return unit;
    }
}
