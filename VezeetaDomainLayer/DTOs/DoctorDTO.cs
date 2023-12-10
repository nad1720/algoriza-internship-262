using Microsoft.AspNetCore.Http;

namespace VezeetaDomainLayer.DTOs
{
    public class DoctorDTO
    {
        public byte[] Image { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Specialize { get; set; }
        public List<AppointmentDTO> Appointments { get; set; }=new List<AppointmentDTO>();
        public string Gender { get; set; }
        public decimal Price { get; set; }
        
    }


    public class AppointmentDTO
    {
        public string Day { get; set; }
        public List<TimeSlotDTO> Times { get; set; } = new List<TimeSlotDTO>();
    }
    

    public class TimeAppointmentDTO
    {

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }


    }
    public class TimeSlotDTO
    {
        public int Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }


    }
}
