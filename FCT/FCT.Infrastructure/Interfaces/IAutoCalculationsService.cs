
namespace FCT.Infrastructure.Interfaces
{
    public interface IAutoCalculationsService
    {
        decimal GetFuelConspumption(decimal burnedLiterAmount, decimal distnaceCovered, int decimalPlaces = 2);
        decimal GetFuelPrice(decimal literAmount, decimal pricePerLiter, int decimalPlaces = 2);
    }
}
