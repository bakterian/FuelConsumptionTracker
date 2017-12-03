using FCT.Infrastructure.Enums;
using FCT.Infrastructure.Interfaces;
using FCT.Infrastructure.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace FCT.Control.ViewModels
{
    public class FuelConsumptionViewModel : TabBaseViewModel, IFuelConsumptionViewModel, INotifyAppClosing, INotifyDbActions
    {
        private readonly IDbReader _dbReader;
        private readonly IDbWriter _dbWriter;
        private readonly IDialogService _dialogService;

        private Dictionary<FuelConEntry, ItemAction> _fuelConEntriesActions;

        public override string HeaderName { get; set; } = "Fuel Consumption";

        private ObservableCollection<object> _fuelConEntries;
        public ObservableCollection<object> FuelConEntries
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
        public FuelConsumptionViewModel
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
            appClosingNotifier.RegisterForNotification(this); //TODO: dispose any listneres when exiting
            dbActionsNotifier.RegisterForNotification(this);
            _fuelConEntriesActions = new Dictionary<FuelConEntry, ItemAction>();
        }

        public override void Init()
        {
            var fuelConsumptionEntries = _dbReader.GetFuelConEntries();

            if (FuelConEntries != null)
            {
                UnregisterEventHandlers();
                FuelConEntries.Clear();

                foreach (var entry in fuelConsumptionEntries)
                {
                    FuelConEntries.Add(entry);
                }
            }
            else
            {
                FuelConEntries = new ObservableCollection<object>(fuelConsumptionEntries);
            }

            RegisterEventHandlers(fuelConsumptionEntries);
            _fuelConEntriesActions = new Dictionary<FuelConEntry, ItemAction>();
        }

        public void OnDbWrite()
        {
            if (_fuelConEntriesActions.Count > 0) PersistFuelConEnrties();
        }

        public void OnDbRead()
        {
            if (_fuelConEntriesActions.Count > 0)
            {
                var res = _dialogService.AskUser("Fuel Consumption Read",
                    "Fuel Consumption data was modified you will lose your data, do you want to continue with reading?");
                if (res == UserResponse.Negative) return;
            }

            Init();
        }

        public void OnApplicationClose()
        {
            if (_fuelConEntriesActions.Count > 0)
            {
                var res = _dialogService.AskUser("Fuel Consumption Changed",
                    "Fuel consumption was modified do you want to persist the changes?");
                if (res == UserResponse.Affirmative)
                {
                    PersistFuelConEnrties();
                }
            }
        }

        private void UnregisterEventHandlers()
        {
            FuelConEntries.CollectionChanged -= FuelConEntriesChanged;
        }

        private void RegisterEventHandlers(IEnumerable<FuelConEntry> fuelConsumptionEntries)
        {
            FuelConEntries.CollectionChanged += FuelConEntriesChanged;
            foreach (var fuelConsEntry in fuelConsumptionEntries)
            {
                fuelConsEntry.PropertyChanged += FuelConsEntryPropertyChanged;
            }
        }

        private void FuelConEntriesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                _fuelConEntriesActions.Add((FuelConEntry)e.NewItems[0], (ItemAction)e.Action);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var removedItem = (FuelConEntry)e.OldItems[0];

                if (_fuelConEntriesActions.Keys.Contains(removedItem))
                {
                    _fuelConEntriesActions.Remove(removedItem);
                }

                _fuelConEntriesActions.Add(removedItem, (ItemAction)e.Action);
            }
        }

        private void FuelConsEntryPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is FuelConEntry &&
                !(_fuelConEntriesActions.ContainsKey((FuelConEntry)sender)))
            {
                _fuelConEntriesActions.Add((FuelConEntry)sender, ItemAction.Update);
            }
        }

        private void PersistFuelConEnrties()
        {
            _dbWriter.InsertFuelConsumptions(GetFuelConEntriesByAction(ItemAction.Add));

            _dbWriter.UpdateFuelConsumptions(GetFuelConEntriesByAction(ItemAction.Update));

            _dbWriter.DeleteFuelConsumptions(GetFuelConEntriesByAction(ItemAction.Remove));

            _fuelConEntriesActions.Clear();
        }

        private IEnumerable<FuelConEntry> GetFuelConEntriesByAction(ItemAction action)
        {
            return _fuelConEntriesActions.Where(_ => _.Value.Equals(action)).Select(_ => _.Key);
        }
    }
}
