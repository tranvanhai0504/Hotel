using System.ComponentModel.DataAnnotations;

namespace HotelServer.Controllers.request
{
    public class SingleIdRequest
    {
        [Required]
        public string Id { get; set; }
    }
}
