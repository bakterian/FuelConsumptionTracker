using System;
using FCT.Infrastructure.AbstractionClasses;
using Ninject;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using FCT.Infrastructure.Interfaces;

namespace FCT.Bootstrapper
{
    public class RegionConfigurator : IRegionConfigurator
    {
        RegionCollection RegionBindings;

        public Shell Configure(Shell rootElement, IKernel ninjectKernel)
        {
            foreach (var regionBidning in RegionBindings)
            {
                var element = GetElementByName(rootElement.RootTopGrid, regionBidning.Name.ToString());
                if(element != null)
                {
                    var regionViewModel = ninjectKernel.Get(regionBidning.DataContext);
                    (regionViewModel as IRegionViewModel).Initialize();
                    ((ContentControl)element).Content = regionViewModel;
                }
            }

            return rootElement;
        }

        public void LoadRegions(RegionCollection regions)
        {
            if (RegionBindings == null) RegionBindings = regions;
            else
            {
                foreach (var regionBinding in regions)
                {
                    RegionBindings.Add(regionBinding);
                }
            }
        }

        private FrameworkElement GetElementByName(FrameworkElement element, string elementName)
        {
            FrameworkElement foundElement = null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                var obj = VisualTreeHelper.GetChild(element, i);

                if (obj is ContentControl && ((FrameworkElement)obj).Name.Equals(elementName))
                {
                    foundElement = (FrameworkElement)obj;
                    break;
                }
                else if (obj is FrameworkElement && VisualTreeHelper.GetChildrenCount((FrameworkElement)obj) > 0)
                {
                    foundElement = GetElementByName((FrameworkElement)obj, elementName);
                    if (foundElement != null) break;
                }
            }
            return foundElement;
        }


    }
}
