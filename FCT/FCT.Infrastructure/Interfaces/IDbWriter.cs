using FCT.Infrastructure.Models;
using System.Collections.Generic;

namespace FCT.Infrastructure.Interfaces
{
    public interface IDbWriter
    {
        int InsertCarDescriptions(IEnumerable<CarDescription> carDescriptions);
        int UpdateCarDescriptions(IEnumerable<CarDescription> carDescriptions);
        int DeleteCarDescriptions(IEnumerable<CarDescription> carDescriptions);
        int InsertFuelConsumptions(IEnumerable<FuelConEntry> fuelConsumptions);
        int UpdateFuelConsumptions(IEnumerable<FuelConEntry> fuelConsumptions);
        int DeleteFuelConsumptions(IEnumerable<FuelConEntry> fuelConsumptions);
    }
}
