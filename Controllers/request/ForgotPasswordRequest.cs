using System.ComponentModel.DataAnnotations;

namespace HotelServer.Controllers.request
{
    public class ForgotPasswordRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
