using System.ComponentModel.DataAnnotations;

namespace ThePower.Models
{
    public class Membership
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }
    }
}
