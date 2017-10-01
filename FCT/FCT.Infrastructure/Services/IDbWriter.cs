using FCT.Infrastructure.Models;
using System.Collections.Generic;

namespace FCT.Infrastructure.Services
{
    public interface IDbWriter
    {
        int InsertCarDescriptions(IEnumerable<CarDescription> carDescriptions);
        int UpdateCarDescriptions(IEnumerable<CarDescription> carDescriptions);
    }
}
