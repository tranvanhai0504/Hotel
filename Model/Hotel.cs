using HotelServer.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelServer.Model
{
    [Table("Hotel")]
    public class Hotel : Switchable
    {
        [Key]
        [MaxLength(50)]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string Type { get; set; }
        public string PayType { get; set; }
        [Required]
        public string Image { get; set; }
        public ICollection<Room> Rooms { get; set; }
    }
}
 