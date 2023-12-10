using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaDomainLayer.Enums;

namespace VezeetaDomainLayer.DTOs
{
    public class UpdateAppointmentDTO
    {
        public int AppointmentId { get; set; }
        public DayOfWeekEnum Day { get; set; }
        public DateTime NewTime { get; set; }
    }
}
