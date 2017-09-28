

namespace FCT.Infrastructure.Models
{
    public class FuelConEntry
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public string PetrolStationDesc { get; set; }
        public string PetrolType { get; set; }
        public string FuelingDate { get; set; }
        public float LiterAmount { get; set; }
        public float PricePerLiter { get; set; }
        public float FullPrice { get; set; }
        public float DistanceMade { get; set; }
        public float FuelConsumption { get; set; }
        public string Terrain { get; set; }
        public string Summary =>
            $"Id: {Id}\nCarId: {CarId}\nPetrolStationDesc: {PetrolStationDesc}\nPetrolType: {PetrolType}\nLiterAmount: {LiterAmount}";
    }
}
