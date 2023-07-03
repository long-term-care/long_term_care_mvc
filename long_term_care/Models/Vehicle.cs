using System.ComponentModel.DataAnnotations;

namespace long_term_care.Models
{
    public class Vehicle
    {
        [Key]
        public string CarNum { get; set; }

        [Required]
        [StringLength(30)]
        public string CarType { get; set; }
    }

}
