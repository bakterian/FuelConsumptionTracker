
namespace FCT.Infrastructure.Events
{
    public class CarSelectionChangedEvent
    {
        public string SelectedCar { get; private set; }

        public CarSelectionChangedEvent(string selectedCar)
        {
            SelectedCar = selectedCar;
        }
    }
}
