using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelServer.Model
{
    [Table("TypeHotel")]
    public class TypeHotel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<Hotel> Hotels { get; set; }
    }
}
