using System.ComponentModel.DataAnnotations;

namespace HotelServer.Controllers.request
{
    public class SingleIdHotelRequest
    {
        [Required]
        public string Id { get; set; }
    }
}
