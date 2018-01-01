using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FCT.Infrastructure.Interfaces
{
    public interface IDbTabViewModel : ITabViewModel
    {
        Task<DataTable> GetDataTableAsync();

        Task UpdateDataAsync(IList<DataTable> data);
    }
}
