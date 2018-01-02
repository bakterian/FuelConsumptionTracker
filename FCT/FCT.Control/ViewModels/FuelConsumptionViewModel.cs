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

        public override string HeaderName { get; set; } = "Fuel Consumption";

        public bool AutoFuelConsCalc { get; set; } = true;

        public bool AutoFullPriceCalc { get; set; } = true;

        public FuelConsumptionViewModel
            (
            IDialogService dialogService,
            IAppClosingNotifier appClosingNotifier,
            IDbActionsNotifier dbActionsNotifier,
            IDataTableMapper dataTableMapper,
            IDbReader dbReader,
            IDbWriter dbWriter,
            IEventAggregator eventAggregator
            )
            : base(dialogService, appClosingNotifier, dbActionsNotifier, dataTableMapper, eventAggregator)

        {
            appClosingNotifier.RegisterForNotification(this, NotificationPriority.Low);
            dbActionsNotifier.RegisterForNotification(this, NotificationPriority.Low);
            _dbReader = dbReader;
            _dbWriter = dbWriter;
        }

        protected override IEnumerable<FuelConEntry> GetTableEntries()
        {
            return _dbReader.GetFuelConEntries();
        }

        protected override void TableDataChanged(object sender, PropertyChangedEventArgs e)
        {
            base.TableDataChanged(sender, e);

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
                persistingPossible = AssignCarIds(carDescAssignments);
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
            var fuelConEntry = (FuelConEntry)TableDataCollection[index];

            var newEntry = MemeberwiseCopy(fuelConEntry);
            newEntry.FullPrice = decimal.Round(fuelConEntry.LiterAmount * fuelConEntry.PricePerLiter, 2);

            fuelConEntry.PropertyChanged -= TableDataChanged;
            newEntry.PropertyChanged += TableDataChanged;

            TableDataCollection[index] = newEntry;
        }

        private void ReCalculateFuelConsumption(int index, string propertyName)
        {
            var fuelConEntry = (FuelConEntry)TableDataCollection[index];
            if (propertyName.Equals("DistanceMade")
                && (index < (TableDataCollection.Count() - 1))
                && (fuelConEntry.DistanceMade > 0))
            {
                var nextEntry = (FuelConEntry)TableDataCollection[index + 1];
                var newEntry = MemeberwiseCopy(fuelConEntry);
                newEntry.FuelConsumption = decimal.Round(100.0M * (nextEntry.LiterAmount / fuelConEntry.DistanceMade), 2);

                fuelConEntry.PropertyChanged -= TableDataChanged;
                newEntry.PropertyChanged += TableDataChanged;
                TableDataCollection[index] = newEntry;
            }
            if (propertyName.Equals("LiterAmount") && (index > 0))
            {
                var previousEntry = (FuelConEntry)TableDataCollection[index - 1];
                if (previousEntry.DistanceMade > 0)
                {
                    var newEntry = MemeberwiseCopy(previousEntry);
                    newEntry.FuelConsumption = decimal.Round(100.0M * (fuelConEntry.LiterAmount / previousEntry.DistanceMade), 2);

                    previousEntry.PropertyChanged -= TableDataChanged;
                    newEntry.PropertyChanged += TableDataChanged;
                    TableDataCollection[index - 1] = newEntry;
                }
            }

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
                FullPrice = decimal.Round(fuelConEntry.LiterAmount * fuelConEntry.PricePerLiter, 2),
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
            }
            else
            {
                FilterBy = "CarDescription";
                FilterPhrase = message.SelectedCar;
            }
        }
    }
}
