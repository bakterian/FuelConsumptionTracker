using FCT.Infrastructure.Interfaces;
using FCT.Infrastructure.Models;
using FCT.WindowControls;
using System.Collections.ObjectModel;

namespace FCT.Control.ViewModels
{
    public class CarSelectionViewModel : RegionBaseViewModel, ICarSelectionViewModel
    {
        //this is WA to enforce copying of Caliburn Micro DLLs (a reference in the xaml is not enough)
        private Caliburn.Micro.Parameter _calParameter;
        private WaObject _waObject;
        private readonly IDbReader _dbReader;
        private readonly IDbWriter _dbWriter;
        private readonly IDbActionsNotifier _dbActionsNotifier;

        private GenericModel<string> _header;
        public GenericModel<string> Header
        {
            get { return _header; }
            set
            {
                if(!value.Equals(_header))
                {
                    _header = value;
                    RaisePropertyChanged();
                }
            }
        }

        private ObservableCollection<CarDescription> _carDescriptions;
        public ObservableCollection<CarDescription> CarDescriptions
        {
            get { return _carDescriptions; }
            set
            {
                if (!value.Equals(_carDescriptions))
                {
                    _carDescriptions = value;
                    RaisePropertyChanged();
                }
            }
        }

        private CarDescription _selectedCarDescription;
        public CarDescription SelectedCarDescription
        {
            get { return _selectedCarDescription; }
            set
            {
                if (!value.Equals(_selectedCarDescription))
                {
                    _selectedCarDescription = value;
                    RaisePropertyChanged();
                }
            }
        }

        public CarSelectionViewModel
            (
            IDbReader dbReader, 
            IDbWriter dbWriter,
            IDbActionsNotifier dbActionsNotifier
            )
        {
            _dbReader = dbReader;
            _dbWriter = dbWriter;
            _dbActionsNotifier = dbActionsNotifier;
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

        public void OnWrite()
        {
            _dbActionsNotifier.FireWriteNotification();
        }

        public void OnRead()
        {
            _dbActionsNotifier.FireReadNotification();
        }
    }
}
