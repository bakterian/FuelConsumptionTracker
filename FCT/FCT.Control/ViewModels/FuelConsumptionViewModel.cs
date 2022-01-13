using Caliburn.Micro;
using FCT.Infrastructure.Enums;
using FCT.Infrastructure.Events;
using FCT.Infrastructure.Interfaces;
using FCT.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace FCT.Control.ViewModels
{
    public class FuelConsumptionViewModel : DbTabBaseViewModel<FuelConEntry>, IFuelConsumptionViewModel
    {
        private readonly IDbReader _dbReader;
        private readonly IDbWriter _dbWriter;
        private readonly IAutoCalculationsService _autoCalcService;
        private string _lastSelectedCart = string.Empty;
        private bool _recalculationOngoing = false;
        private bool _carIdAssignmentOngoing = false;
        public override string HeaderName { get; set; } = "Fuel Consumption";

        public bool AutoFuelConsCalc { get; set; } = true;

        public bool AutoFullPriceCalc { get; set; } = true;

        private FuelConEntry _newEntry;
        public FuelConEntry NewEntry
        {
            get { return _newEntry; }
            set
            {
                if(!value.Equals(_newEntry))
                {
                    _newEntry = value;
                    RaisePropertyChanged();
                }
            }
        }

        private decimal _distanceOnBurnedFuel = 0;
        public  decimal DistanceOnBurnedFuel
        {
            get { return _distanceOnBurnedFuel;  }
            set
            {
                if(!value.Equals(_distanceOnBurnedFuel))
                {
                    _distanceOnBurnedFuel = value;
                    RaisePropertyChanged();
                }
            }
        }
    
        public FuelConsumptionViewModel
            (
            IDialogService dialogService,
            IAppClosingNotifier appClosingNotifier,
            IDbActionsNotifier dbActionsNotifier,
            IDataTableMapper dataTableMapper,
            IDbReader dbReader,
            IDbWriter dbWriter,
            IEventAggregator eventAggregator, 
            IAutoCalculationsService autoCalcService
            )
            : base(dialogService, appClosingNotifier, dbActionsNotifier, dataTableMapper, eventAggregator)

        {
            appClosingNotifier.RegisterForNotification(this, NotificationPriority.Low);
            dbActionsNotifier.RegisterForNotification(this, NotificationPriority.Low);
            _dbReader = dbReader;
            _dbWriter = dbWriter;
            _autoCalcService = autoCalcService;
            InitNewFuelConsumptionEntry();
        }

        protected override IEnumerable<FuelConEntry> GetTableEntries()
        {
            return _dbReader.GetFuelConEntries();
        }

        protected override void TableDataChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_recalculationOngoing == false && _carIdAssignmentOngoing == false)
            { // When recalculation is stated this callback will be re-entered.
              // Limiting the amount of update opeations and improving processing time
                if (sender is FuelConEntry)
                {
                    var entryIndex = TableDataCollection.IndexOf(sender as FuelConEntry);
                    if (AutoFullPriceCalc && (e.PropertyName.Equals("LiterAmount") || e.PropertyName.Equals("PricePerLiter")))
                    {
                        ReCalculateFullPrice(entryIndex);
                    }
                    if (AutoFuelConsCalc && (e.PropertyName.Equals("LiterAmount") || e.PropertyName.Equals("DistanceMade")))
                    {
                        ReCalculateFuelConsumption(entryIndex, e.PropertyName);
                    }
                }

                base.TableDataChanged(sender, e);
            }
        }

        protected override void PersistDisplayedData()
        {
            if (IsPersistingPossible())
            {
                _dbWriter.DeleteFuelConsumptions(GetTableEntriesByAction(ItemAction.Remove));

                _dbWriter.InsertFuelConsumptions(GetTableEntriesByAction(ItemAction.Add));

                _dbWriter.UpdateFuelConsumptions(GetTableEntriesByAction(ItemAction.Update));

                TableDataCollectionActions.Clear();
            }
        }

        private bool IsPersistingPossible()
        {
            var persistingPossible = true;
            var addActions = GetTableEntriesByAction(ItemAction.Add);
            var updateActions = GetTableEntriesByAction(ItemAction.Update);

            var carDescAssignments = addActions.Concat(updateActions).ToList();

            if (carDescAssignments.Count() > 0)
            {
                _carIdAssignmentOngoing = true;
                persistingPossible = AssignCarIds(carDescAssignments);
                _carIdAssignmentOngoing = false;
            }

            return persistingPossible;
        }

        private bool AssignCarIds(IList<FuelConEntry> fuelConEntries)
        {
            var assginmentsDone = true;
            var carDescriptions = _dbReader.GetCarDescriptions();

            for (int i = 0; i < fuelConEntries.Count; i++)
            {
                var carDesId = carDescriptions
                    .Where(_ => _.Description.Equals(fuelConEntries[i].CarDescription))
                    ?.Select(_ => _.Id)
                    ?.FirstOrDefault();

                if (carDesId == 0)
                {
                    DialogService.ShowErrorMsg("Saving Fuel Consumption", $"No matching car described as \"{fuelConEntries[i].CarDescription}\" could be found.");
                    assginmentsDone = false;
                    break;
                }

                fuelConEntries[i].CarId = carDesId.Value;
            }

            return assginmentsDone;
        }

        private void ReCalculateFullPrice(int index)
        {
            _recalculationOngoing = true;
            var fuelConEntry = (FuelConEntry)TableDataCollection[index];
            fuelConEntry.FullPrice = _autoCalcService.GetFuelPrice(fuelConEntry.LiterAmount, fuelConEntry.PricePerLiter);
            _recalculationOngoing = false;
        }

        private void ReCalculateFuelConsumption(int index, string propertyName)
        {
            _recalculationOngoing = true;
            var fuelConEntry = (FuelConEntry)TableDataCollection[index];
            if (propertyName.Equals("DistanceMade")
                && (index < (TableDataCollection.Count() - 1))
                && (fuelConEntry.DistanceMade > 0))
            {
                var nextEntry = (FuelConEntry)TableDataCollection[index + 1];
                fuelConEntry.FuelConsumption = _autoCalcService.GetFuelConspumption(nextEntry.LiterAmount, fuelConEntry.DistanceMade);
            }
            if (propertyName.Equals("LiterAmount") && (index > 0))
            {
                var previousEntry = (FuelConEntry)TableDataCollection[index - 1];
                if (previousEntry.DistanceMade > 0)
                {
                    previousEntry.FuelConsumption = _autoCalcService.GetFuelConspumption(fuelConEntry.LiterAmount, previousEntry.DistanceMade);
                }
            }
            _recalculationOngoing = false;
        }

        private FuelConEntry MemeberwiseCopy(FuelConEntry fuelConEntry)
        {
            return new FuelConEntry()
            {
                Id = fuelConEntry.Id,
                CarId = fuelConEntry.CarId,
                CarDescription = fuelConEntry.CarDescription,
                PetrolStationDesc = fuelConEntry.PetrolStationDesc,
                PetrolType = fuelConEntry.PetrolType,
                FuelingDate = fuelConEntry.FuelingDate,
                LiterAmount = fuelConEntry.LiterAmount,
                PricePerLiter = fuelConEntry.PricePerLiter,
                FullPrice = _autoCalcService.GetFuelPrice(fuelConEntry.LiterAmount, fuelConEntry.PricePerLiter),
                DistanceMade = fuelConEntry.DistanceMade,
                FuelConsumption = fuelConEntry.FuelConsumption,
                Terrain = fuelConEntry.Terrain
            };
        }

        public override void Handle(CarSelectionChangedEvent message)
        {
            if(string.IsNullOrEmpty(message.SelectedCar))
            {
                FilterBy = string.Empty;
                FilterPhrase = string.Empty;
                NewEntry.CarDescription = string.Empty;
                _lastSelectedCart = string.Empty;
            }
            else
            {
                FilterBy = "CarDescription";
                FilterPhrase = message.SelectedCar;
                NewEntry.CarDescription = message.SelectedCar;
                _lastSelectedCart = message.SelectedCar;
            }
        }

        public void OnSubmitNewEntry()
        {
            EnrichTableWithNewEntry();
            InitNewFuelConsumptionEntry();
        }

        private void EnrichTableWithNewEntry()
        {
            NewEntry.PropertyChanged -= OnNewEntryPropertyChaned;

            TableDataCollection.Add(MemeberwiseCopy(NewEntry));
            var lastElement = (FuelConEntry)TableDataCollection.Last();
            lastElement.PropertyChanged += TableDataChanged;

            if (TableDataCollection.Count > 1)
            {
                var previousEntry  = (FuelConEntry) TableDataCollection.ElementAt(TableDataCollection.Count - 2);
                previousEntry.DistanceMade = DistanceOnBurnedFuel;
            }
        }

        private void InitNewFuelConsumptionEntry()
        {
            NewEntry = new FuelConEntry();
            NewEntry.CarDescription = _lastSelectedCart;
            NewEntry.FuelingDate = DateTime.Now;
            NewEntry.PropertyChanged -= OnNewEntryPropertyChaned;
            NewEntry.PropertyChanged += OnNewEntryPropertyChaned;
            DistanceOnBurnedFuel = 0;
        }

        private void OnNewEntryPropertyChaned(object sender, PropertyChangedEventArgs e)
        {
            if((sender is FuelConEntry) && (e.PropertyName.Equals("LiterAmount") || e.PropertyName.Equals("PricePerLiter")))
            {
                NewEntry.FullPrice = _autoCalcService.GetFuelPrice(NewEntry.LiterAmount, NewEntry.PricePerLiter);
            }
        }
    }
}
