using FCT.Infrastructure.Models;
using System.Collections.Generic;

namespace FCT.Infrastructure.Services
{
    public interface IDbReader
    {
        IEnumerable<CarDescription> GetCarDescriptions();
        IEnumerable<FuelConEntry> GetFuelConEntries();
        IEnumerable<FuelConEntry> GetFuelConEntries(int carId);
    }
}
