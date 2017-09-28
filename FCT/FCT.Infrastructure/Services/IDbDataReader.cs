using FCT.Infrastructure.Models;
using System.Collections.Generic;

namespace FCT.Infrastructure.Services
{
    public interface IDbDataReader
    {
        List<CarDescription> GetCarDescriptions();
        List<FuelConEntry> GetFuelConEntries();
        List<FuelConEntry> GetFuelConEntries(int carId);
    }
}
