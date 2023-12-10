using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaDomainLayer.Enums;

namespace VezeetaDomainLayer.DTOs
{
    public class DocAppointmentDTO
    {

        public decimal Price { get; set; }
        public DayOfWeekEnum Days { get; set; }
        public List<TimeAppointmentDTO> TimeSlots { get; set; }
    }
    
}
