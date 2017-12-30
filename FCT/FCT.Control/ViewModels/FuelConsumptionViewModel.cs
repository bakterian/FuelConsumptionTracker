using FCT.Infrastructure.Enums;
using FCT.Infrastructure.Interfaces;
using FCT.Infrastructure.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FCT.Control.ViewModels
{
    public class FuelConsumptionViewModel : DbTabBaseViewModel<FuelConEntry>, IFuelConsumptionViewModel
    {
        private readonly IDbReader _dbReader;
        private readonly IDbWriter _dbWriter;

        public override string HeaderName { get; set; } = "Fuel Consumption";

        public FuelConsumptionViewModel
            (
            IDialogService dialogService,
            IAppClosingNotifier appClosingNotifier,
            IDbActionsNotifier dbActionsNotifier,
            IDataTableMapper dataTableMapper,
            IDbReader dbReader,
            IDbWriter dbWriter
            )
            : base(dialogService, appClosingNotifier, dbActionsNotifier, dataTableMapper)

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

                if(carDesId == 0)
                {
                    DialogService.ShowErrorMsg("Saving Fuel Consumption", $"No matching car described as \"{fuelConEntries[i].CarDescription}\" could be found.");
                    assginmentsDone = false;
                    break;
                }

                fuelConEntries[i].CarId = carDesId.Value;
            }

            return assginmentsDone;
        }
    }
}
