
using System;
using FCT.Infrastructure.Interfaces;

namespace FCT.Control.Services
{
    public class AutoCalculationsService : IAutoCalculationsService
    {
        public decimal GetFuelConspumption(decimal burnedLiterAmount, decimal distnaceCovered, int decimalPlaces = 2)
        {
            return decimal.Round(100.0M * (burnedLiterAmount / distnaceCovered), decimalPlaces);
        }

        public decimal GetFuelPrice(decimal literAmount, decimal pricePerLiter, int decimalPlaces = 2)
        {
            return decimal.Round(literAmount * pricePerLiter, decimalPlaces);
        }
    }
}
