using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaDomainLayer.Models;

namespace VezeetaDomainLayer.Models
{
    public class TimeSlot
    {
        public int Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public Appointment Appointment { get; set; }
        public int AppointmentId { get; set; }
        public int? DiscountId { get; set; }
        public Discount Discount { get; set; }
        public List<Request> Requests { get; set; } = new List<Request>();
    }
}
