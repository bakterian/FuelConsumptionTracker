
namespace FCT.Infrastructure.Models
{
    public class CarDescription
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Manufucaturer { get; set; }
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
        public string Summary =>
            $"Id: {Id}\nDescription: {Description}\nManufucaturer: {Manufucaturer}\nModel: {Model}\nHorsePower: {HorsePower}";
    }
}
