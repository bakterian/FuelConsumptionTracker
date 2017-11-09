using System.ComponentModel;

namespace FCT.Infrastructure.Models
{
    public class GenericModel<T> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private T _value;
        public T Value
        {
            get { return _value; }
            set
            {
                if(!_value.Equals(value))
                {
                    _value = value;
                    OnPropertyChanged("Value");
                }
            }
        }

        public GenericModel(T value)
        {
            _value = value;
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
