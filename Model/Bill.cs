using HotelServer.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelServer.Model
{
    [Table("Bill")]
    public class Bill : Switchable
    {
        [Key]
        [MaxLength(50)]
        public string Id { get; set; }
        [Required]
        public double Total { get; set; }
        [Required]
        public int Period { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string RoomId { get; set; }
        [ForeignKey("RoomId")]
        public Room Room { get; set; }
        [Required]
        [MaxLength(450)]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
