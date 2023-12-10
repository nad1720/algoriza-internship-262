using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaDomainLayer.Enums;

namespace VezeetaDomainLayer.DTOs
{
    public class PatientDiscountDTO
    {
        public byte[] Image { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public GenderEnum Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<PatientRequestDTO> Requests { get; set; } = new List<PatientRequestDTO>();
    }

    public class PatientRequestDTO
    {
        public byte[] Image { get; set; }
        public string DoctorName { get; set; }
        public string Specialize { get; set; }
        public String Day { get; set; }
       
        public decimal Price { get; set; }
        public string? DiscountCode { get; set; }
        public decimal FinalPrice { get; set; }
        public string Status { get; set; }
        // Include other properties as needed
    }
}
