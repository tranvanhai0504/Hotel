namespace HotelServer.Controllers.request
{
    public class RoomRequest
    {
        public string Id { get; set; }
        public string Bed { get; set; }
        public string QuantityId { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public string Image { get; set; }
        public string HotelId { get; set; }
    }
}
