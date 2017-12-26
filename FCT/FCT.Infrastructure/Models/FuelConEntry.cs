using FCT.Infrastructure.Attributes;
using System;
using System.Collections.Generic;

namespace FCT.Infrastructure.Models
{
    public class FuelConEntry : BaseDbModel
    {
        private int _carId;
        private string _petrolStationDesc;
        private string _petrolType;
        private DateTime _fuelingDate;
        private decimal _literAmount;
        private decimal _pricePerLiter;
        private decimal _fullPrice;
        private decimal _distanceMade;
        private decimal _fuelConsumption;
        private string _terrain;

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
        public DateTime FuelingDate
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

        public override bool Equals(object obj)
        {
            var isEqual = false;
            var item = obj as FuelConEntry;

            if (item != null &&
                CarId == item.CarId &&
                PetrolStationDesc == item.PetrolStationDesc &&
                PetrolType == item.PetrolType &&
                FuelingDate == item.FuelingDate &&
                LiterAmount == item.LiterAmount &&
                PricePerLiter == item.PricePerLiter &&
                FullPrice == item.FullPrice &&
                DistanceMade == item.DistanceMade &&
                FuelConsumption == item.FuelConsumption &&
                Terrain == item.Terrain
                )
            {

                isEqual = true;
            }

            return isEqual;
        }

        public override int GetHashCode()
        {
            var hashCode = -1402654746;
            hashCode = hashCode * -1521134295 + CarId.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PetrolStationDesc);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PetrolType);
            hashCode = hashCode * -1521134295 + EqualityComparer<DateTime>.Default.GetHashCode(FuelingDate);
            hashCode = hashCode * -1521134295 + LiterAmount.GetHashCode();
            hashCode = hashCode * -1521134295 + PricePerLiter.GetHashCode();
            hashCode = hashCode * -1521134295 + FullPrice.GetHashCode();
            hashCode = hashCode * -1521134295 + DistanceMade.GetHashCode();
            hashCode = hashCode * -1521134295 + FuelConsumption.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Terrain);
            return hashCode;
        }

        public override string Summary =>
            $"Id: {Id}\nCarId: {CarId}\nPetrolStationDesc: {PetrolStationDesc}\nPetrolType: {PetrolType}\nLiterAmount: {LiterAmount}";
    }
}
