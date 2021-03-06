﻿using Caliburn.Micro;
using FCT.Infrastructure.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FCT.Control.ViewModels
{
    public abstract class RegionBaseViewModel : IRegionViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public abstract void Initialize();

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
