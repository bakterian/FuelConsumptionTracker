using FCT.Infrastructure.Attributes;
using System.Collections.Generic;

namespace FCT.Infrastructure.Models
{
    public class CarDescription : BaseDbModel
    {
        private string _description;
        private string _manufacturer;
        private string _model;
        private int _horsePower;
        private int _engineSize;
        private string _petrolType;
        private int _fuelTankSize;
        private int _weight;
        private decimal _topSpeed;
        private decimal _acceleration;
        private decimal _avgFuelConsumption;
        private int _productionYear;


        [PresentableItem]
        public string Description
        {
            get { return _description; }
            set
            {
                if(!value.Equals(_description))
                {
                    _description = value;
                    RaisePropertyChanged();
                }
            }
        }

        [PresentableItem]
        public string Manufacturer
        {
            get { return _manufacturer; }
            set
            {
                if (!value.Equals(_manufacturer))
                {
                    _manufacturer = value;
                    RaisePropertyChanged();
                }
            }
        }

        [PresentableItem]
        public string Model
        {
            get { return _model; }
            set
            {
                if(!value.Equals(_model))
                {
                    _model = value;
                    RaisePropertyChanged();
                }
            }
        }

        [PresentableItem]
        public int HorsePower
        {
            get { return _horsePower; }
            set
            {
                if(!value.Equals(_horsePower))
                {
                    _horsePower = value;
                    RaisePropertyChanged();
                }
            }
        }

        [PresentableItem]
        public int EngineSize
        {
            get { return _engineSize; }
            set
            {
                if(!value.Equals(_engineSize))
                {
                    _engineSize = value;
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
                if(!value.Equals(_petrolType))
                {
                    _petrolType = value;
                    RaisePropertyChanged();
                }
            }
        }

        [PresentableItem]
        public int FuelTankSize
        {
            get { return _fuelTankSize; }
            set
            {
                if (!value.Equals(_fuelTankSize))
                {
                    _fuelTankSize = value;
                    RaisePropertyChanged();
                }
            }
        }

        [PresentableItem]
        public int Weight
        {
            get { return _weight; }
            set
            {
                if (!value.Equals(_weight))
                {
                    _weight = value;
                    RaisePropertyChanged();
                }
            }
        }
        [PresentableItem]
        public decimal TopSpeed
        {
            get { return _topSpeed; }
            set
            {
                if (!value.Equals(_topSpeed))
                {
                    _topSpeed = value;
                    RaisePropertyChanged();
                }
            }
        }

        [PresentableItem]
        public decimal Acceleration
        {
            get { return _acceleration; }
            set
            {
                if (!value.Equals(_acceleration))
                {
                    _acceleration = value;
                    RaisePropertyChanged();
                }
            }
        }

        [PresentableItem]
        public decimal AvgFuelConsumption
        {
            get { return _avgFuelConsumption; }
            set
            {
                if (!value.Equals(_avgFuelConsumption))
                {
                    _avgFuelConsumption = value;
                    RaisePropertyChanged();
                }
            }
        }

        [PresentableItem]
        public int ProductionYear
        {
            get { return _productionYear; }
            set
            {
                if (!value.Equals(_productionYear))
                {
                    _productionYear = value;
                    RaisePropertyChanged();
                }
            }
        }

        public override string Summary =>
            $"Id: {Id}\nDescription: {Description}\nManufucaturer: {Manufacturer}\nModel: {Model}\nHorsePower: {HorsePower}";

        public override bool Equals(object obj)
        {
            var isEqual = false;
            var item = obj as CarDescription;

            if (item != null &&
                Description == item.Description &&
                Manufacturer == item.Manufacturer &&
                Model == item.Model &&
                HorsePower == item.HorsePower &&
                EngineSize == item.EngineSize &&
                PetrolType == item.PetrolType &&
                FuelTankSize == item.FuelTankSize &&
                Weight == item.Weight &&
                TopSpeed == item.TopSpeed &&
                Acceleration == item.Acceleration &&
                AvgFuelConsumption == item.AvgFuelConsumption &&
                ProductionYear == item.ProductionYear 
                )
            {

                isEqual = true;
            }

            return isEqual;
        }

        public override int GetHashCode()
        {
            var hashCode = 988141777;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Manufacturer);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Model);
            hashCode = hashCode * -1521134295 + HorsePower.GetHashCode();
            hashCode = hashCode * -1521134295 + EngineSize.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PetrolType);
            hashCode = hashCode * -1521134295 + FuelTankSize.GetHashCode();
            hashCode = hashCode * -1521134295 + Weight.GetHashCode();
            hashCode = hashCode * -1521134295 + TopSpeed.GetHashCode();
            hashCode = hashCode * -1521134295 + Acceleration.GetHashCode();
            hashCode = hashCode * -1521134295 + AvgFuelConsumption.GetHashCode();
            hashCode = hashCode * -1521134295 + ProductionYear.GetHashCode();
            return hashCode;
        }
    }
}
