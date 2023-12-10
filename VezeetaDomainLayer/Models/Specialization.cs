using System.ComponentModel.DataAnnotations;

namespace VezeetaDomainLayer.Models
{
    public class Specialization
    {
        [Key]
        public int SpecializationId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
       
        public ICollection<Doctor> Doctors { get; set; }
    }
}
