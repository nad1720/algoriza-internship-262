using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace VezeetaDomainLayer.Models
{
    public class Admin:IdentityUser<int>
    {
       
        
        
        public ICollection<Doctor> Doctors { get; set; }
        
        public ICollection<Patient> Patients { get; set; }

    }
}
