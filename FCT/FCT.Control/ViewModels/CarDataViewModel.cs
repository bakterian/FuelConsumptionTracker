using FCT.Infrastructure.Interfaces;
using FCT.Infrastructure.Models;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using FCT.Infrastructure.Enums;

namespace FCT.Control.ViewModels
{
    public class CarDataViewModel : TabBaseViewModel, ICarDataViewModel, INotifyAppClosing, INotifyDbActions
    {
        private readonly IDbReader _dbReader;
        private readonly IDbWriter _dbWriter;
        private readonly IDialogService _dialogService;

        private Dictionary<CarDescription,ItemAction> _carDescCollectionActions;
        private ObservableCollection<object> _carDescriptionCollection;
        public ObservableCollection<object> CarDescriptionCollection
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

        public override string HeaderName { get; set; } = "Car Data";

        public CarDataViewModel
            (
            IDbReader dbReader, 
            IDbWriter dbWriter,
            IDialogService dialogService,
            IAppClosingNotifier appClosingNotifier,
            IDbActionsNotifier dbActionsNotifier
            )
        {
            _dbReader = dbReader;
            _dbWriter = dbWriter;
            _dialogService = dialogService;
            appClosingNotifier.RegisterForNotification(this);
            dbActionsNotifier.RegisterForNotification(this);
            _carDescCollectionActions = new Dictionary<CarDescription, ItemAction>();
        }

        public override void Init()
        {
            var carDescriptions = _dbReader.GetCarDescriptions();

            if (CarDescriptionCollection != null)
            {
                UnregisterEventHandlers();
                CarDescriptionCollection.Clear();

                foreach (var carDescription in carDescriptions)
                {
                    CarDescriptionCollection.Add(carDescription);
                }
            }
            else
            {
                CarDescriptionCollection = new ObservableCollection<object>(carDescriptions);
            }
            
            RegisterEventHandlers(carDescriptions);
            _carDescCollectionActions = new Dictionary<CarDescription, ItemAction>();
        }

        public void OnDbWrite()
        {
            if (_carDescCollectionActions.Count > 0) PersistCarData();
        }

        public void OnDbRead()
        {
            if (_carDescCollectionActions.Count > 0)
            {
                var res = _dialogService.AskUser("Car Data Read", "Car data information was modified you will lose your data, do you want to continue with reading?");
                if (res == UserResponse.Negative) return;
            }

            Init();
        }

        public void OnApplicationClose()
        {
            if (_carDescCollectionActions.Count > 0)
            {
                var res = _dialogService.AskUser("Car Data Changed",
                    "Car data information was modified do you want to persist the changes?");
                if (res == UserResponse.Affirmative)
                {
                    PersistCarData();
                }
            }
        }


        private void UnregisterEventHandlers()
        {
            CarDescriptionCollection.CollectionChanged -= carDescCollectionChanged;
        }

        private void RegisterEventHandlers(IEnumerable<CarDescription> carDescriptions)
        {
            CarDescriptionCollection.CollectionChanged += carDescCollectionChanged;
            foreach (var carDescription in carDescriptions)
            {
                carDescription.PropertyChanged += carDescPropertyChanged;
            }
        }

        private void carDescCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                _carDescCollectionActions.Add((CarDescription)e.NewItems[0], (ItemAction)e.Action);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var removedItem = (CarDescription)e.OldItems[0];

                if(_carDescCollectionActions.Keys.Contains(removedItem))
                {
                    _carDescCollectionActions.Remove(removedItem);
                }

                _carDescCollectionActions.Add(removedItem,(ItemAction)e.Action);
                //TODO: notify the FuelConViewModel that entries for this car id should not be dispayed
            }
        }

        private void carDescPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is CarDescription && 
                !(_carDescCollectionActions.ContainsKey((CarDescription)sender)))
            {
                _carDescCollectionActions.Add((CarDescription)sender, ItemAction.Update);
            }
        }

        private void PersistCarData()
        {
            _dbWriter.InsertCarDescriptions(GetCarDescByAction(ItemAction.Add));

            _dbWriter.UpdateCarDescriptions(GetCarDescByAction(ItemAction.Update));

            DeleteCarData();

            _carDescCollectionActions.Clear();
        }

        private IEnumerable<CarDescription> GetCarDescByAction(ItemAction action)
        {
            return _carDescCollectionActions.Where(_ => _.Value.Equals(action)).Select(_ => _.Key);
        }

        private void DeleteCarData()
        {
            var descForRemoval = GetCarDescByAction(ItemAction.Remove);
            foreach (var desc in descForRemoval)
            {
                if(_dbReader.GetFuelConEntries(desc.Id).FirstOrDefault() != null)
                {
                    var res = _dialogService.AskUser("Car Data Removing", 
                        $"The removed car {desc.Description} has asociated fuel entries, which will be also removed do you want to proceed?");
                    if (res == UserResponse.Negative) continue;
                }

                _dbWriter.DeleteCarDescriptions(new CarDescription[] { desc });                
            }
        }
    }
}
