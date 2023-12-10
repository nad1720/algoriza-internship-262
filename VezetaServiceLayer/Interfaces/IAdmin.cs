using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaDomainLayer.DTOs;


namespace VezeetaServiceLayer.Interfaces
{
    public interface IAdmin
    {
        
        int GetTotalDoctorsCount();
        int GetTotalPatientsCount();
        int GetTotalBookingRequestsCount();
        IEnumerable<GetAllDoctorAdminDTO> GetAllDoctors(int page, int pageSize, string search);
        GetDoctorDTO GetDoctorById(int doctorId);
       Task< bool> AddDoctor(DoctorDetailsDTO doctor);
        Task<bool> EditDoctor(int doctorId, DoctorDetailsDTO doctor);
        bool DeleteDoctor(int doctorId);
        IEnumerable<PatientDetailsDTO> GetAllPatients(int page, int pageSize, string search);
        PatientDiscountDTO GetPatientById(int patientId);
        RequestStatisticsDTO GetRequestStatistics();
        List<SpecializationStatisticsDTO> GetTopSpecializations(int topCount);
        List<TopDoctorDTO> GetTopDoctors(int topCount);
        bool AddDiscount(AddDiscountDTO discountDTO);
        bool UpdateDiscount(int discountId, AddDiscountDTO updateDiscountDTO);
        bool DeleteDiscount(int discountId);
        bool DeactivateDiscount(int discountId);
         Task<bool> LoginAdmin(LoginUserDTO loginAdmintDto);
        string GenerateAdminTokenString(LoginUserDTO loginAdmintDto);
        
    }
}
