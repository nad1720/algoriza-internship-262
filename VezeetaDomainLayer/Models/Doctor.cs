using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using VezeetaDomainLayer.Enums;

namespace VezeetaDomainLayer.Models
{
    public class Doctor:IdentityUser<int>
    {

        public byte[] Image { get; set; }
        public string FullName { get; set; }
       
        public string Phone { get; set; }
        public string Specialize { get; set; }
        public int? SpecializationId { get; set; }
        public int AdminId { get; set; }
        public GenderEnum Gender { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Specialization? spcialization { get;set; }
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
        public List<Request> Requests { get; set; } = new List<Request>();
    }
   
}

