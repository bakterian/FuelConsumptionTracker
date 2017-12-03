using FCT.Infrastructure.Attributes;

namespace FCT.Infrastructure.Models
{
    public class FuelConEntry : BaseDbModel
    {
        private int _carId;
        private string _petrolStationDesc;
        private string _petrolType;
        private string _fuelingDate;
        private decimal _literAmount;
        private decimal _pricePerLiter;
        private decimal _fullPrice;
        private decimal _distanceMade;
        private decimal _fuelConsumption;
        private string _terrain;

        public int Id { get; set; }

        [PresentableItem]
        public int CarId
        {
            get { return _carId; }
            set
            {
                if(!value.Equals(_carId))
                {
                    _carId = value;
                    RaisePropertyChanged();
                }
            }
        }

        [PresentableItem]
        public string PetrolStationDesc
        {
            get { return _petrolStationDesc; }
            set
            {
                if (!value.Equals(_petrolStationDesc))
                {
                    _petrolStationDesc = value;
                    RaisePropertyChanged();
                }
            }
        }

        [PresentableItem]
        public string PetrolType
        {
            get { return _petrolType; }
            set
            {
                if (!value.Equals(_petrolType))
                {
                    _petrolType = value;
                    RaisePropertyChanged();
                }
            }
        }

        [PresentableItem]
        public string FuelingDate
        {
            get { return _fuelingDate; }
            set
            {
                if (!value.Equals(_fuelingDate))
                {
                    _fuelingDate = value;
                    RaisePropertyChanged();
                }
            }
        }

        [PresentableItem]
        public decimal LiterAmount
        {
            get { return _literAmount; }
            set
            {
                if (!value.Equals(_literAmount))
                {
                    _literAmount = value;
                    RaisePropertyChanged();
                }
            }
        }

        [PresentableItem]
        public decimal PricePerLiter
        {
            get { return _pricePerLiter; }
            set
            {
                if (!value.Equals(_pricePerLiter))
                {
                    _pricePerLiter = value;
                    RaisePropertyChanged();
                }
            }
        }

        [PresentableItem]
        public decimal FullPrice
        {
            get { return _fullPrice; }
            set
            {
                if (!value.Equals(_fullPrice))
                {
                    _fullPrice = value;
                    RaisePropertyChanged();
                }
            }
        }

        [PresentableItem]
        public decimal DistanceMade
        {
            get { return _distanceMade; }
            set
            {
                if (!value.Equals(_distanceMade))
                {
                    _distanceMade = value;
                    RaisePropertyChanged();
                }
            }
        }

        [PresentableItem]
        public decimal FuelConsumption
        {
            get { return _fuelConsumption; }
            set
            {
                if (!value.Equals(_fuelConsumption))
                {
                    _fuelConsumption = value;
                    RaisePropertyChanged();
                }
            }
        }

        [PresentableItem]
        public string Terrain
        {
            get { return _terrain; }
            set
            {
                if (!value.Equals(_terrain))
                {
                    _terrain = value;
                    RaisePropertyChanged();
                }
            }
        }

        public override string Summary =>
            $"Id: {Id}\nCarId: {CarId}\nPetrolStationDesc: {PetrolStationDesc}\nPetrolType: {PetrolType}\nLiterAmount: {LiterAmount}";
    }
}
