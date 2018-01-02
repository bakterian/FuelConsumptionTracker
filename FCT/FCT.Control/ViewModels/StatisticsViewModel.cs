using System;
using Caliburn.Micro;
using FCT.Infrastructure.Events;
using FCT.Infrastructure.Interfaces;

namespace FCT.Control.ViewModels
{
    public class StatisticsViewModel : IStatisticsViewModel, IHandle<CarSelectionChangedEvent>
    {
        public string HeaderName { get; set; } = "Statistics";

        public StatisticsViewModel
            (
            IEventAggregator eventAggregator
            )
        {
            eventAggregator.Subscribe(this);
        }
        public void Init()
        {
            
        }

        public void Handle(CarSelectionChangedEvent message)
        {
            if(string.IsNullOrEmpty(message.SelectedCar))
            {
                //TODO: display general statistics
            }
            else
            {
                //TODO: display statistics of chosen car
            }
        }
    }
}
