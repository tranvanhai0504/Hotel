using HotelServer.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelServer.Model
{
    [Table("Room")]
    public class Room : Switchable
    {
        [Key]
        [MaxLength(50)]
        public string Id { get; set; }
        [Required]
        public string Bed { get; set; }
        [Required]
        public string Quantity { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string HotelId { get; set; }
        [ForeignKey("HotelId")]
        public Hotel Hotel { get; set; }
        [Required]
        public int Amount { get; set; }
        public string Image { get; set; }
    }
}
