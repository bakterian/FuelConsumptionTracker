using FCT.Infrastructure.Interfaces;
using FCT.Infrastructure.Models;
using System.Collections.ObjectModel;
using PostSharp.Patterns.Model;

namespace FCT.Control.ViewModels
{
    [NotifyPropertyChanged]
    public class CarSelectionViewModel : RegionBaseViewModel, ICarSelectionViewModel
    {
        private readonly IDbReader _dbReader;

        private readonly IDbWriter _dbWriter;

        public GenericModel<string> Header { get; set; }

        public ObservableCollection<CarDescription> CarDescriptions {get; set;}

        public CarDescription SelectedCarDescription { get; set; }

        public CarSelectionViewModel
            (
            IDbReader dbReader, 
            IDbWriter dbWriter
            )
        {
            _dbReader = dbReader;
            _dbWriter = dbWriter;
        }

        public override void Initialize()
        {
            Header = new GenericModel<string>("Fule Consumption Tracker");
            InitilizeCarDescriptions();
        }

        private void InitilizeCarDescriptions()
        {
            var carDescList = _dbReader.GetCarDescriptions();
            CarDescriptions = new ObservableCollection<CarDescription>(carDescList);
            if (CarDescriptions.Count > 0) SelectedCarDescription = CarDescriptions[0];
        }

        public void OnCarSelectionChange()
        {
            var test = true;
            //TODO: fire event aggregator to notify subscribed view models
            //persist the user setting in the database
        }
    }
}
