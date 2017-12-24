using FCT.Infrastructure.Interfaces;
using System.Collections.ObjectModel;
using System.Linq;

namespace FCT.Control.ViewModels
{
    public class MainTabViewModel : RegionBaseViewModel, IMainTabViewModel
    {
        private readonly IFuelConsumptionViewModel _fuelConsumptonViewModel;
        private readonly ICarDataViewModel _carDataViewModel;
        private readonly IStatisticsViewModel _statisticsViewModel;
        private readonly IDbTabVmStore _dbTabVmStore;

        private ObservableCollection<ITabViewModel> _tabs;
        public ObservableCollection<ITabViewModel> Tabs
        {
            get { return _tabs; }
            set
            {
                if (!value.Equals(_tabs))
                {
                    _tabs = value;
                    RaisePropertyChanged();
                }
            }
        }

        private ITabViewModel _selectedTab;
        public ITabViewModel SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                if(!value.Equals(_selectedTab))
                {
                    _selectedTab = value;
                    RaisePropertyChanged();
                }
            }
        }

        public MainTabViewModel
            (
                IFuelConsumptionViewModel fuelConsumptionViewModel,
                ICarDataViewModel carDataViewModel,
                IStatisticsViewModel statisticsViewModel,
                IDbTabVmStore dbTabVmStore
            )
        {
            _fuelConsumptonViewModel = fuelConsumptionViewModel;
            _carDataViewModel = carDataViewModel;
            _statisticsViewModel = statisticsViewModel;
            _dbTabVmStore = dbTabVmStore;
        }

        public override void Initialize()
        {
            Tabs = new ObservableCollection<ITabViewModel>();

            AddInitilizedTab(_fuelConsumptonViewModel);
            AddInitilizedTab(_carDataViewModel);
            AddInitilizedTab(_statisticsViewModel);

            InitiliazeDbVmStore();

            SelectedTab = _fuelConsumptonViewModel;
        }
        
        private void AddInitilizedTab(ITabViewModel tabViewModel)
        {
            tabViewModel.Init();
            Tabs.Add(tabViewModel);
        }

        private void InitiliazeDbVmStore()
        {
            foreach (var tab in Tabs)
            {
                if (tab is IDbTabViewModel) _dbTabVmStore.Add((IDbTabViewModel)tab);
            }
        }

    }
}
