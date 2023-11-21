namespace HotelServer.Controllers.request
{
    public class BillRequest
    {
        public string Id { get; set; }
        public int Period { get; set; }
        public string RoomId { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public double Total { get; set; }
        public int amountRoom { get; set; }
    }
}
