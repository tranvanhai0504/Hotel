using HotelServer.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelServer.Controllers.dataFormat
{
    public class RoomDataResponse
    {
        public string Id { get; set; }
        public string Bed { get; set; }
        public string QuantityId { get; set; }
        public TypeRoom TypeRoom { get; set; }
        public double Price { get; set; }
        public string HotelId { get; set; }
        public int Amount { get; set; }
        public string Image { get; set; }

        public RoomDataResponse(Room room)
        {

        }
    }
}
