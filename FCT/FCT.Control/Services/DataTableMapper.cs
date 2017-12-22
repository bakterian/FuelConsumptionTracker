using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using FCT.Infrastructure.Interfaces;
using System.Threading.Tasks;

namespace FCT.Control.Services
{
    public class DataTableMapper : IDataTableMapper
    {
        public Task<DataTable> ConvertToDataTableAsync<T>(IEnumerable<T> data, Attribute[] supportedPropAttributes)
        {
            return Task.FromResult<DataTable>(ConvertToDataTable(data, supportedPropAttributes));
        }

        public DataTable ConvertToDataTable<T>(IEnumerable<T> data, Attribute[] supportedPropAttributes)
        {
            var properties = TypeDescriptor.GetProperties(typeof(T), supportedPropAttributes);
            var dataTable = new DataTable();

            for (int i = 0; i < properties.Count; i++)
            {
                var property = properties[i];
                dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            object[] values = new object[properties.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }

                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public IEnumerable<T> ConvertToEnumerable<T>(DataTable dataTable)
        {
            throw new NotImplementedException();
        }
    }
}
