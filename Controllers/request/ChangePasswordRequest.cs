using System.ComponentModel.DataAnnotations;

namespace HotelServer.Controllers.request
{
    public class ChangePasswordRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string OldPassword { get; set; }
    }
}
