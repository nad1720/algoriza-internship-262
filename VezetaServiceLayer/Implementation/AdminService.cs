using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VezeetaDomainLayer.Models;
using VezeetaRepositoryLayer;
using VezeetaServiceLayer.Interfaces;
using VezeetaDomainLayer.DTOs;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using VezeetaDomainLayer.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace VezeetaServiceLayer.Implementation
{
    public class AdminService : IAdmin
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _adminUserManager;
        private readonly IConfiguration _config;
        private readonly ILogger<AdminService> _logger;
        public AdminService(UserManager<IdentityUser> adminUserManager, ApplicationDbContext context,IConfiguration config, ILogger<AdminService> logger)
        {
            _context = context;
            _adminUserManager = adminUserManager;
            _config = config;
            _logger = logger;
        }
      
        public async Task<bool> LoginAdmin(LoginUserDTO loginAdminDto)
        {
            try
            {
                var identityUser = await _adminUserManager.FindByEmailAsync(loginAdminDto.Email);

                if (identityUser is null)
                {
                    _logger.LogInformation($"Admin not found for email: {loginAdminDto.Email}");
                    return false;
                }

                var passwordCorrect = await _adminUserManager.CheckPasswordAsync(identityUser, loginAdminDto.Password);

                if (passwordCorrect)
                {
                    _logger.LogInformation($"Login successful for admin with email: {loginAdminDto.Email}");
                }
                else
                {
                    _logger.LogInformation($"Login failed for admin with email: {loginAdminDto.Email}");
                }

                return passwordCorrect;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during admin login: {ex.Message}");
                return false;
            }
        }
        public string GenerateAdminTokenString(LoginUserDTO loginAdmintDto)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,loginAdmintDto.Email),
                new Claim(ClaimTypes.Role,"Admin"),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));

            var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                issuer: _config.GetSection("Jwt:Issuer").Value,
                audience: _config.GetSection("Jwt:Audience").Value,
                signingCredentials: signingCred);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return tokenString;
        }
        public bool AddDiscount(AddDiscountDTO discountDTO)
        {
            
                var discount = new Discount
                {
                    CouponCode = discountDTO.DiscountCode,
                    Type = discountDTO.DiscountType,
                    Amount = discountDTO.Amount
                };

                var requestsToUpdate = _context.Requests
                    .Where(request => request.Status == StatusEnum.Completed && !request.DiscountId.HasValue)
                    .OrderBy(request => request.Id)
                    .Take(discountDTO.CompletedRequestsThreshold)
                    .ToList();

                _context.Discounts.Add(discount);
                _context.SaveChanges();

                return true; 
            
           
        }

        public async Task<bool> AddDoctor(DoctorDetailsDTO doctor)
        {
            try
            {
                var identityUser = new IdentityUser
                {
                    UserName = doctor.Email,
                    Email = doctor.Email,
                };

                var result = await _adminUserManager.CreateAsync(identityUser, doctor.Password);

                using var dataStream = new MemoryStream();

               
                var existingSpecialization = _context.Specializations.FirstOrDefault(s => s.Name == doctor.Specialize);

                
                var newSpecialize = existingSpecialization ?? new Specialization
                {
                    Name = doctor.Specialize,
                };

                
                if (existingSpecialization == null)
                {
                    _context.Specializations.Add(newSpecialize);
                    _context.SaveChanges(); 
                }

                var newDoctor = new Doctor
                {
                    AdminId = 1,
                    Image = dataStream.ToArray(),
                    FullName = doctor.FullName,
                    Email = identityUser.Email,
                    Phone = doctor.Phone,
                    SpecializationId= newSpecialize.SpecializationId,
                    Specialize = newSpecialize.Name, 
                    Gender = (GenderEnum)Enum.Parse(typeof(GenderEnum), doctor.Gender),
                    DateOfBirth = doctor.DateOfBirth,
                };

                _context.Doctors.Add(newDoctor);
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred during doctor addition: {ex.Message}");
                return false;
            }
        }


        public bool DeactivateDiscount(int discountId)
        {
            
                var existingDiscount = _context.Discounts.Find(discountId);

                if (existingDiscount == null)
                {
                   
                    return false;
                }

                existingDiscount.IsDeactivated = true; 
                _context.SaveChanges();

                return true; 
            
           
        }
    

        public bool DeleteDiscount(int discountId)
        {
            
                var existingDiscount = _context.Discounts.Find(discountId);

                if (existingDiscount == null)
                {
                   
                    return false;
                }

                _context.Discounts.Remove(existingDiscount);
                _context.SaveChanges();

                return true; 
            
           
        }

        public bool DeleteDoctor(int doctorId)
        {
            try
            {
                var doctorToDelete = _context.Doctors.Find(doctorId);

                if (doctorToDelete == null)
                {
                    Debug.WriteLine($"Doctor with ID {doctorId} not found.");
                    return false;
                }

                _context.Doctors.Remove(doctorToDelete);
                _context.SaveChanges();

                return true; 
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred during doctor deletion: {ex.Message}");
                return false; 
            }
        }

        public async Task<bool> EditDoctor(int doctorId, DoctorDetailsDTO doctor)
        {
            if (string.IsNullOrEmpty(doctor.FullName) || string.IsNullOrEmpty(doctor.Email))
            {
                Debug.WriteLine("Invalid data: First name, last name, and email are required.");
                return false;
            }

            try
            {
                

                var existingDoctor = _context.Doctors
                    .Include(d => d.spcialization)
                    .FirstOrDefault(d => d.Id == doctorId);

                if (existingDoctor == null)
                {
                    Debug.WriteLine($"Doctor with ID {doctorId} not found.");
                    return false;
                }

                if (doctor.Image != null)
                {
                    using var dataStream = new MemoryStream();
                    doctor.Image.CopyTo(dataStream);
                    existingDoctor.Image = dataStream.ToArray();
                }

                var identityUser = new IdentityUser
                {
                    UserName = doctor.Email,
                    Email = doctor.Email,
                };

                var result = await _adminUserManager.CreateAsync(identityUser, doctor.Password);

                existingDoctor.FullName = doctor.FullName;
                existingDoctor.Email = identityUser.Email;
                existingDoctor.Phone = doctor.Phone;
                existingDoctor.DateOfBirth = doctor.DateOfBirth;

               
                var existingSpecialization = _context.Specializations
                    .FirstOrDefault(s => s.Name == doctor.Specialize);

               
                if (existingSpecialization == null)
                {
                    existingSpecialization = new Specialization { Name = doctor.Specialize };
                    _context.Specializations.Add(existingSpecialization);
                }

                existingDoctor.Specialize = existingSpecialization.Name;

                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred during doctor editing: {ex.Message}");
                return false;
            }
        }



        public IEnumerable<GetAllDoctorAdminDTO> GetAllDoctors(int page, int pageSize, string search)
        {
            
            
            var query = _context.Doctors.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(d =>
                    d.FullName.Contains(search) ||
                    d.Specialize.Contains(search) ||
                    d.Email.Contains(search)
                
                );
            }

            var doctorDTOs = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(doctor => new GetAllDoctorAdminDTO
                {
                    Image=doctor.Image,
                    FullName = doctor.FullName,
                    Email = doctor.Email,
                    Phone = doctor.Phone,
                    Specialize = doctor.Specialize,                    
                    Gender = doctor.Gender.ToString(),
                })
                .ToList();
          
            return doctorDTOs;
        }

        public IEnumerable<PatientDetailsDTO> GetAllPatients(int page, int pageSize, string search)
        {
            var query = _context.Patients.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(patient =>
                    patient.PatientName.Contains(search) ||
                    
                    patient.Phone.Contains(search)
               
                );
            }
            using var dataStream = new MemoryStream();
           
            var patients = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(patient => new PatientDetailsDTO
                {
                    
                    Image = patient.Image,
                    PatientName = patient.PatientName,
                    Email = patient.Email,
                    Phone = patient.Phone,
                    Gender = patient.Gender.ToString(),
                    DateOfBirth = patient.DateOfBirth,
                   
                })
                .ToList();

            return patients;
        }

        public GetDoctorDTO GetDoctorById(int doctorId)
        {
            var doctor = _context.Doctors
           .Where(d => d.Id == doctorId)
           .Select(doctor => new GetDoctorDTO
           {
               Image=doctor.Image,
               FullName = doctor.FullName,
               Email = doctor.Email,
               Phone = doctor.Phone,
               Specialize = doctor.Specialize,
               Gender = doctor.Gender.ToString(),
               DateOfBirth = doctor.DateOfBirth,
              
           })
           .FirstOrDefault();

            return doctor;
        }

        public PatientDiscountDTO GetPatientById(int patientId)
        {
            var patient = _context.Patients
             .Where(p => p.Id == patientId)
             .Select(p => new PatientDiscountDTO
             {
                 Image = p.Image,
                 FullName = p.PatientName,
                 Email = p.Email,
                 Phone = p.Phone,
                 Gender = p.Gender,
                 DateOfBirth = p.DateOfBirth,
                 Requests = p.Requests.Select(r => new PatientRequestDTO
                 {
                     Image = r.Doctor.Image,
                     DoctorName = r.Doctor.FullName,
                     Specialize = r.Doctor.Specialize,
                     Day = r.Appointment.Day.ToString(),
                     
                     Price = r.Doctor.Price,
                     DiscountCode = r.Discount.CouponCode,
                     FinalPrice = Calculations.CalculateFinalPrice(r.Doctor.Price, r.Discount),
                     Status = r.Status.ToString(),
                    
                 }).ToList(),
                 
             })
             .FirstOrDefault();

            return patient;
        }

        public RequestStatisticsDTO GetRequestStatistics()
        {
            
            
                var totalRequests = _context.Requests.Count();
                var pendingRequests = _context.Requests.Count(r => r.Status == StatusEnum.Pending);
                var completedRequests = _context.Requests.Count(r => r.Status == StatusEnum.Completed);
                var cancelledRequests = _context.Requests.Count(r => r.Status == StatusEnum.Cancelled);

                return new RequestStatisticsDTO
                {
                    TotalRequests = totalRequests,
                    PendingRequests = pendingRequests,
                    CompletedRequests = completedRequests,
                    CancelledRequests = cancelledRequests
                };
            
        }

        public List<TopDoctorDTO> GetTopDoctors(int topCount)
        {
            var topDoctors = _context.Doctors
                .OrderByDescending(doctor => doctor.Appointments.SelectMany(appointment => appointment.Requests).Count())
                .Take(topCount)
                .Select(doctor => new TopDoctorDTO
                {
                    Image = doctor.Image,
                    FullName = doctor.FullName,
                    Specialize = doctor.Specialize,
                    NumberOfRequests = doctor.Appointments.SelectMany(appointment => appointment.Requests).Count()
                })
                .ToList();

            return topDoctors;
        }


        public List<SpecializationStatisticsDTO> GetTopSpecializations(int topCount)
        {
            var specializationStatistics = _context.Requests
            .GroupBy(r => r.Doctor.Specialize)
            .OrderByDescending(group => group.Count())
            .Take(topCount)
            .Select(group => new SpecializationStatisticsDTO
            {
                SpecializationName = group.Key,
                NumberOfRequests = group.Count()
            })
            .ToList();

            return specializationStatistics;
        }

        public int GetTotalBookingRequestsCount()
        {
            return _context.Requests.Count();
        }

        public int GetTotalDoctorsCount()
        {
            return _context.Doctors.Count();
        }

        public int GetTotalPatientsCount()
        {
            return _context.Patients.Count();
        }

        public bool UpdateDiscount(int discountId, AddDiscountDTO updateDiscountDTO)
        {
            
                var existingDiscount = _context.Discounts.Find(discountId);

                if (existingDiscount == null)
                {
                    
                    return false;
                }

                
                var requestsToUpdate = _context.Requests
                   .Where(request => request.Status == StatusEnum.Completed && !request.DiscountId.HasValue)
                   .OrderBy(request => request.Id);

                existingDiscount.CouponCode = updateDiscountDTO.DiscountCode;
                existingDiscount.Type = updateDiscountDTO.DiscountType;
                existingDiscount.Amount = updateDiscountDTO.Amount;
                

                _context.SaveChanges();

                return true; 
        }


    }
}
