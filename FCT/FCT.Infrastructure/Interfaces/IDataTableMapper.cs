﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FCT.Infrastructure.Interfaces
{
    public interface IDataTableMapper
    {
        Task<DataTable> ConvertToDataTableAsync<T>(IEnumerable<T> data, Attribute[] supportedPropAttributes);

        DataTable ConvertToDataTable<T>(IEnumerable<T> data, Attribute[] supportedPropAttributes);

        bool IsConversionToEnumerablePossible<T>(DataTable dataTable, Attribute[] supportedPropAttributes);

        Task<IEnumerable<T>> ConvertToDbEnumerableAsync<T>(DataTable dataTable, Attribute[] supportedPropAttributes) where T : new(); 

        IEnumerable<T> ConvertToDbEnumerable<T>(DataTable dataTable, Attribute[] supportedPropAttributes) where T : new();
    }
}
