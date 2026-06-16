using UnitConversionApi.Core.Models;

namespace UnitConversionApi.Core.Interfaces;

public interface IUnitRegistry
{
    IEnumerable<UnitDefinition> GetAllUnits();
    UnitDefinition? GetUnitBySymbol(string symbol);
}
