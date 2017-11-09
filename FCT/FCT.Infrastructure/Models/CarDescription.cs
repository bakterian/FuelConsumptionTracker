using System.ComponentModel;

namespace FCT.Infrastructure.Models
{
    public class CarDescription : BaseDbModel, INotifyPropertyChanged
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                if(!_id.Equals(value))
                {
                    _id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        private string _description = "";
        public string Description
        {
            get { return _description; }
            set
            {
                if (!_description.Equals(value))
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }
        //TODO: finish INPC backing fields adding
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int HorsePower { get; set; }
        public int EngineSize { get; set; }
        public string PetrolType { get; set; }
        public int FuelTankSize { get; set; }
        public int Weight { get; set; }
        public float TopSpeed { get; set; }
        public float Acceleration { get; set; }
        public float AvgFuelConsumption { get; set; }
        public int ProductionYear { get; set; }
        public override string Summary =>
            $"Id: {Id}\nDescription: {Description}\nManufucaturer: {Manufacturer}\nModel: {Model}\nHorsePower: {HorsePower}";

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
