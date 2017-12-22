using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FCT.Infrastructure.Interfaces
{
    public interface IDataTableMapper
    {
        Task<DataTable> ConvertToDataTableAsync<T>(IEnumerable<T> data, Attribute[] supportedPropAttributes);

        DataTable ConvertToDataTable<T>(IEnumerable<T> data, Attribute[] supportedPropAttributes);

        IEnumerable<T> ConvertToEnumerable<T>(DataTable dataTable);
    }
}
