namespace AvvalOnline.Shop.Api.Messaging.Vehicle
{
    public class VehicleDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string PlateNumber { get; set; }
        public string VIN { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public string EngineNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
