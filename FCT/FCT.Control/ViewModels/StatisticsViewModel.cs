using System;
using FCT.Infrastructure.Interfaces;

namespace FCT.Control.ViewModels
{
    public class StatisticsViewModel : TabBaseViewModel, IStatisticsViewModel
    {
        public override string HeaderName { get; set; } = "Statistics";

        public override void Init()
        {
            
        }
    }
}
