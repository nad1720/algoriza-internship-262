using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VezeetaDomainLayer.Enums;

namespace VezeetaDomainLayer.DTOs
{
    public class RegisterPatientDTO
    {
       
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }

        public IFormFile Image { get; set; } 
        public string? FullName { get; set; }

        public string Phone { get; set; }
        public GenderEnum Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }


    
}
