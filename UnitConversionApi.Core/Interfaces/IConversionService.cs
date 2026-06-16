namespace UnitConversionApi.Core.Interfaces;

public interface IConversionService
{
    double Convert(double value, string fromSymbol, string toSymbol);
}
