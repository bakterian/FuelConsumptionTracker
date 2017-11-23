using FCT.Infrastructure.Interfaces;
using FCT.Infrastructure.Models;
using System.Collections.ObjectModel;

namespace FCT.Control.ViewModels
{
    public class FuelConsumptionViewModel : TabBaseViewModel, IFuelConsumptionViewModel
    {
        private readonly IDbReader _dbReader;
        private readonly IDbWriter _dbWriter;

        public override string HeaderName { get; set; } = "Fuel Consumption";
        //TODO: alter existing data on dataGrid change
        private ObservableCollection<FuelConEntry> _fuelConEntries;
        public ObservableCollection<FuelConEntry> FuelConEntries
        {
            get { return _fuelConEntries; }
            set
            {
                if(!value.Equals(_fuelConEntries))
                {
                    _fuelConEntries = value;
                    RaisePropertyChanged();
                }
            }
        }
        public FuelConsumptionViewModel(IDbReader dbReader, IDbWriter dbWriter)
        {
            _dbReader = dbReader;
            _dbWriter = dbWriter;
        }

        public override void Init()
        {
            FuelConEntries = new ObservableCollection<FuelConEntry>(_dbReader.GetFuelConEntries());
        }
    }
}
