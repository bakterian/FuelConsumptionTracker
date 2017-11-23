using FCT.Infrastructure.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FCT.Control.ViewModels
{
    public abstract class TabBaseViewModel : ITabViewModel
    {
        public abstract string HeaderName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public abstract void Init();

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
