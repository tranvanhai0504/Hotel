namespace HotelServer.Controllers.request
{
    public class AddBillRequest
    {
        
        public int Period { get; set; }
        public string RoomId { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public double Total { get; set; }
    }
}
