using System.ComponentModel.DataAnnotations;

namespace HotelServer.Controllers.request
{
    public class RegistrationRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }

    }
}
