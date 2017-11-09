using FCT.Infrastructure.AbstractionClasses;
using Ninject;

namespace FCT.Bootstrapper
{
    public interface IRegionConfigurator
    {
        void LoadRegions(RegionCollection regions);
        Shell Configure(Shell rootElement, IKernel ninjectKernel);
    }
}
