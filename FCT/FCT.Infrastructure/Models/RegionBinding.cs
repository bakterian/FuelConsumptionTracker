using FCT.Infrastructure.Enums;
using System;

namespace FCT.Infrastructure.Models
{
    public class RegionBinding
    {
        public Region Name { get; set; }

        public Type DataContext { get; set; }

        public RegionBinding(Region name, Type dataContext)
        {
            Name = name;
            DataContext = dataContext;
        }
    }
}
