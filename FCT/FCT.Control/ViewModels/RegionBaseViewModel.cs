using Caliburn.Micro;
using FCT.Infrastructure.Interfaces;

namespace FCT.Control.ViewModels
{
    public abstract class RegionBaseViewModel : PropertyChangedBase, IRegionViewModel
    {
        public abstract void Initialize();
    }
}
