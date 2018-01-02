using FCT.Infrastructure.Interfaces;
using FCT.Infrastructure.Models;
using System.Linq;
using System.Collections.Generic;
using FCT.Infrastructure.Enums;
using Caliburn.Micro;
using FCT.Infrastructure.Events;

namespace FCT.Control.ViewModels
{
    public class CarDataViewModel : DbTabBaseViewModel<CarDescription>, ICarDataViewModel, INotifyAppClosing, INotifyDbActions
    {
        private readonly IDbReader _dbReader;
        private readonly IDbWriter _dbWriter;

        public override string HeaderName { get; set; } = "Car Data";

        public CarDataViewModel
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
            appClosingNotifier.RegisterForNotification(this, NotificationPriority.High);
            dbActionsNotifier.RegisterForNotification(this, NotificationPriority.High);
            _dbReader = dbReader;
            _dbWriter = dbWriter;
        }

        protected override IEnumerable<CarDescription> GetTableEntries()
        {
            return _dbReader.GetCarDescriptions();
        }

        protected override void PersistDisplayedData()
        {
            DeleteCarData();

            _dbWriter.InsertCarDescriptions(GetTableEntriesByAction(ItemAction.Add));

            _dbWriter.UpdateCarDescriptions(GetTableEntriesByAction(ItemAction.Update));

            TableDataCollectionActions.Clear();
        }

        private void DeleteCarData()
        {
            var descForRemoval = GetTableEntriesByAction(ItemAction.Remove);
            foreach (var desc in descForRemoval)
            {
                if(_dbReader.GetFuelConEntries(desc.Id).FirstOrDefault() != null)
                {
                    var res = DialogService.AskUser("Car Data Removing", 
                        $"The removed car {desc.Description} has asociated fuel entries, which will be also removed do you want to proceed?");
                    if (res == UserResponse.Negative) continue;
                }

                _dbWriter.DeleteCarDescriptions(new CarDescription[] { desc });                
            }
        }

        public override void Handle(CarSelectionChangedEvent message)
        {
            if (string.IsNullOrEmpty(message.SelectedCar))
            {
                FilterBy = string.Empty;
                FilterPhrase = string.Empty;
            }
            else
            {
                FilterBy = "Description";
                FilterPhrase = message.SelectedCar;
            }
        }
    }
}
