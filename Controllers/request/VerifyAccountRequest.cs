using System.ComponentModel.DataAnnotations;

namespace HotelServer.Controllers.request
{
    public class VerifyAccountRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
