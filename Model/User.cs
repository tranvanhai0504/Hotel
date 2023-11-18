using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelServer.Model
{
    [Table("User")]
    public class User : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Role { get; set; }
        public string? Address { get; set; }
    }
}
