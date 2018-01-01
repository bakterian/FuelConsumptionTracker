using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using FCT.Infrastructure.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;

namespace FCT.Control.Services
{
    public class DataTableMapper : IDataTableMapper
    {
        private readonly ILogger _logger;

        public DataTableMapper(ILogger logger)
        {
            _logger = logger;
        }

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

        public Task<IEnumerable<T>> ConvertToDbEnumerableAsync<T>(DataTable dataTable, Attribute[] supportedPropAttributes) where T : new()
        {
            return Task.FromResult(ConvertToDbEnumerable<T>(dataTable, supportedPropAttributes));
        }

        public IEnumerable<T> ConvertToDbEnumerable<T>(DataTable dataTable, Attribute[] supportedPropAttributes) where T : new()
        {
            var supportedProperties = GetSupportedProperties<T>(supportedPropAttributes);

            var itemCollection = new List<T>();

            try
            {
                foreach (DataRow dataTableRow in dataTable.Rows)
                {
                    var newItem = new T();
                    for (var i = 0; i < supportedProperties.Count; i++)
                    {
                        var colValue = dataTableRow.ItemArray[i];
                        var targetProperty = supportedProperties[i];

                        try
                        {
                            targetProperty.SetValue(newItem, Convert.ChangeType(colValue, targetProperty.PropertyType));
                        }
                        catch (Exception e )
                        {
                            var test = 1;
                            throw;
                        }
                    }
                    itemCollection.Add(newItem);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "[TableDataMapper] Error during data table to IEnumerable conversion.");
                throw;
            }


            return itemCollection;
        }

        public bool IsConversionToEnumerablePossible<T>(DataTable dataTable, Attribute[] supportedPropAttributes)
        {
            var conversionPossible = false;

            if (dataTable.Rows.Count == 0 && dataTable.Columns.Count == 0)
            {
                _logger.Error("DataTable contains no elements.");
            }
            else if (dataTable.Rows.Count == 0)
            {
                _logger.Error("DataTable contains no elemet rows.");
            }
            else if (dataTable.Columns.Count != GetSupportedProperties<T>(supportedPropAttributes).Count)
            {
                _logger.Error($"DataTable column vs target properties count mismatch.\nDataTable: {dataTable.Columns.Count}, TargetProperties: {GetSupportedProperties<T>(supportedPropAttributes).Count}.");
            }
            else if (!ColumnNamesMatchTargetProperties<T>(dataTable, supportedPropAttributes))
            {
                _logger.Error("DataTable column names are not matching target property names.");
            }
            else if (!DataTableRowWidthIsConsistent(dataTable))
            {
                _logger.Error("DataTable row width is not consistent.");
            }
            else
            {
                conversionPossible = true;
            }

            return conversionPossible;
        }

        private static List<PropertyInfo> GetSupportedProperties<T>(Attribute[] supportedPropAttributes)
        {
            var suppAttributeTypes = supportedPropAttributes.Select(_ => _.GetType());

            Func<IEnumerable<CustomAttributeData>, bool> ContainsSuppAttributes =
                (customAttributes) => customAttributes.Any(_ => suppAttributeTypes.Contains(_.AttributeType));

            var supportedProperties = typeof(T)
                .GetProperties()
                .Where(x => ContainsSuppAttributes(x.CustomAttributes))
                .ToList();
            return supportedProperties;
        }

        private bool ColumnNamesMatchTargetProperties<T>(DataTable dataTable, Attribute[] supportedPropAttributes)
        {
            var isMatch = true;

            var suppPropertyNames = GetSupportedProperties<T>(supportedPropAttributes).Select(_ => _.Name).ToArray();
            var dataColumns= dataTable.Columns;

            for (int i = 0; i < suppPropertyNames.Count(); i++)
            {
                if(!dataColumns[i].ColumnName.Equals(suppPropertyNames[i]))
                {
                    isMatch = false;
                    break;
                }
            }
            return isMatch;
        }

        private bool DataTableRowWidthIsConsistent(DataTable dataTable)
        {
            var rowsAreEqaulSized = true;
            var colCount = dataTable.Columns.Count;

            foreach (DataRow dataRow in dataTable.Rows)
            {
                if(!dataRow.ItemArray.Count().Equals(colCount))
                {
                    rowsAreEqaulSized = false;
                    break;
                }
                foreach (var item in dataRow.ItemArray)
                {
                    if(!(item is string))
                    {
                        rowsAreEqaulSized = false;
                        break;
                    }
                }
            }

            return rowsAreEqaulSized;
        }
        
    }
}
