using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaDomainLayer.Enums;

namespace VezeetaDomainLayer.DTOs
{
    public class BookingResponseDTO
    {
        public byte[] Image { get; set; }
        public string DoctorName { get; set; }
        public string Specialize { get; set; }
        public String Day { get; set; }
        public List<TimeSlotDTO> Time { get; set; }= new List<TimeSlotDTO>();
        public decimal Price { get; set; }
        public string DiscountCode { get; set; }
        public decimal FinalPrice { get; set; }
        public string Status { get; set; }
        public int AppointmentId { get; set; }
    }
}
