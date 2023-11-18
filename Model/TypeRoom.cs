using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HotelServer.Model
{
    public class TypeRoom
    {
        [Required]
        [Key]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public IEnumerable<Room> Rooms { get; set; }
    }
}
