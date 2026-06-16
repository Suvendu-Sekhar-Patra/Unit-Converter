using System.Text.Json;
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
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "units.json");
        
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"The configuration file '{filePath}' was not found.");
        }

        var json = File.ReadAllText(filePath);
        
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var root = JsonSerializer.Deserialize<UnitRoot>(json, options);
        
        if (root?.Units != null)
        {
            foreach (var unit in root.Units)
            {
                _units[unit.Symbol] = unit;
            }
        }
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

    // Helper class for JSON deserialization
    private class UnitRoot
    {
        public List<UnitDefinition> Units { get; set; } = new();
    }
}
