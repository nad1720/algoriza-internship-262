using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaDomainLayer.Models;
using VezeetaDomainLayer.DTOs;

namespace VezeetaServiceLayer.Interfaces
{
    public interface IPatient
    {
        Task<bool> RegisterPatient(RegisterPatientDTO registerPatientDto);
       Task<bool> LoginPatient(LoginUserDTO loginPatientDto);
        IEnumerable<DoctorDTO> GetAllDoctors(int page, int pageSize, string search);
        bool BookAppointment(int patientId, BookingDTO bookAppointment);
        IEnumerable<BookingResponseDTO> GetPatientBookings(int patientId);
        bool CancelBooking(int bookingId);
        String GeneratePatientTokenString(LoginUserDTO loginPatientDto);
    }
}
