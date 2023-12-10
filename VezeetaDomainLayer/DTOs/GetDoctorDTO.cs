using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VezeetaDomainLayer.DTOs
{
    public class GetDoctorDTO
    {
        public byte[] Image { get; set; }
        public string FullName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
       
        public string Phone { get; set; }
        public string Specialize { get; set; }
        public String Gender { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
