using FCT.Infrastructure.AbstractionClasses;
using FCT.Infrastructure.Enums;
using FCT.Infrastructure.Interfaces;
using FCT.Infrastructure.Models;
using System;


namespace FCT.Control.RegionModules
{
    public class WindowRegionCollection : RegionCollection
    {
        public WindowRegionCollection()
        {
            Regions.Add(new RegionBinding(Region.FileMenu, typeof(IFileMenuViewModel)));
            Regions.Add(new RegionBinding(Region.CarSelection, typeof(ICarSelectionViewModel)));
        }
    }
}
