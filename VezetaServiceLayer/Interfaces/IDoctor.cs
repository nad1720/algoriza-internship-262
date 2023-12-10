using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaDomainLayer.DTOs;
using VezeetaDomainLayer.Enums;

namespace VezeetaServiceLayer.Interfaces
{
    public interface IDoctor
    {
        string GenerateDoctorTokenString(LoginUserDTO loginDoctortDto);
        Task<bool> LoginDoctor(LoginUserDTO loginDoctorDto);
        IEnumerable<DoctorRequestDTO> GetAllRequests(int doctorId, int page, int pageSize, DayOfWeekEnum searchbydate);
        bool ConfirmCheckUp(int requestId);
        bool AddDocAppointment(int doctorId, DocAppointmentDTO appointmentDTO);
        bool UpdateAppointment(int doctorId, int appointmentId, DocAppointmentDTO updatedAppointmentDto);
        bool DeleteTimeSlot(int appointmentId, int timeSlotId);

    }
}
