using System.ComponentModel.DataAnnotations;
using VezeetaDomainLayer.Enums;

namespace VezeetaDomainLayer.Models
{
    public class Request
    {
        public int Id { get; set; }
        
       
        public StatusEnum Status { get; set; }

        
        public string? DiscountCodeCoupon { get; set; }

        
        public int? DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public int? PatientId { get; set; }
        public Patient Patient { get; set; }

        public int? DiscountId { get; set; }
        public Discount Discount { get; set; }
        public virtual TimeSlot TimeSlot { get; set; }
        public int? AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }
        public int? TimeSlotId { get; set; }
        
       
    }
   
   
}
