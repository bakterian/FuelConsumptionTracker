using FCT.Infrastructure.Interfaces;
using FCT.Infrastructure.Models;
using System.Collections.ObjectModel;
using System;

namespace FCT.Control.ViewModels
{
    public class CarDataViewModel : TabBaseViewModel, ICarDataViewModel
    {
        private readonly IDbReader _dbReader;

        private readonly IDbWriter _dbWriter;

        public override string HeaderName { get; set; } = "Car Data";

        public CarDataViewModel(IDbReader dbReader, IDbWriter dbWriter)
        {
            _dbReader = dbReader;
            _dbWriter = dbWriter;
        }

        private ObservableCollection<CarDescription> _carDescriptionCollection;
        public ObservableCollection<CarDescription> CarDescriptionCollection
        {
            get { return _carDescriptionCollection; }
            set
            {
                if (!value.Equals(_carDescriptionCollection))
                {
                    _carDescriptionCollection = value;
                    RaisePropertyChanged();
                }
            }
        }

        public override void Init()
        {
            CarDescriptionCollection = new ObservableCollection<CarDescription>(_dbReader.GetCarDescriptions());
        }
    }
}
