using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaDomainLayer.Enums;
using VezeetaDomainLayer.Models;

namespace VezeetaDomainLayer.Models
{
    public class Appointment
    {
        public int Id { get; set; } 
        public DayOfWeekEnum Day { get; set; }
        public List<TimeSlot> Times { get; set; } = new List<TimeSlot>();
        public int TimeSlotId { get; set; }
        public ICollection<Request> Requests { get; set; }
        public int? DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        
        public int? PatientId { get; set; }
        public Patient Patient { get; set; }

    }
}
