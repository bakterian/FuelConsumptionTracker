using FCT.Infrastructure.Enums;
using FCT.Infrastructure.Interfaces;
using FCT.Infrastructure.Models;
using System.Collections.Generic;

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
            _dbReader = dbReader;
            _dbWriter = dbWriter;
        }

        protected override IEnumerable<FuelConEntry> GetTableEntries()
        {
            return _dbReader.GetFuelConEntries();
        }

        protected override void PersistDisplayedData()
        {
            _dbWriter.DeleteFuelConsumptions(GetTableEntriesByAction(ItemAction.Remove));

            _dbWriter.InsertFuelConsumptions(GetTableEntriesByAction(ItemAction.Add));

            _dbWriter.UpdateFuelConsumptions(GetTableEntriesByAction(ItemAction.Update));

            TableDataCollectionActions.Clear();
        }
    }
}
