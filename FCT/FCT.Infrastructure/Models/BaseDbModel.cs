﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FCT.Infrastructure.Models
{
    public abstract class BaseDbModel : INotifyPropertyChanged
    {
        public int Id { get; set; }

        public abstract string Summary { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
