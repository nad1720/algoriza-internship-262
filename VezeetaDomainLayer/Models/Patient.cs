using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using VezeetaDomainLayer.Enums;
using VezeetaDomainLayer.Models;

namespace VezeetaDomainLayer.Models
{
    public class Patient:IdentityUser<int>
    {
       
        [Required]
        public string? PatientName { get; set; }
        public int? AdminId { get; set; }
        public byte[]? Image { get; set; }
        public string? Phone { get; set; }
        public GenderEnum Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<Request>? Requests { get; set; } = new List<Request>();
        public List<Appointment>? Appointments { get; set; }
        
        public Admin? Admin { get; set; }
        
    }
}
