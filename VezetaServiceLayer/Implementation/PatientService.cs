    using Microsoft.EntityFrameworkCore;
    using System.Diagnostics;
    using VezeetaRepositoryLayer;
    using VezeetaServiceLayer.Interfaces;
    using VezeetaDomainLayer.DTOs;
    using VezeetaDomainLayer.Models;
    using VezeetaDomainLayer.Enums;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.Extensions.Configuration;
    using Microsoft.AspNetCore.Mvc;

    namespace VezeetaServiceLayer.Implementation
    {
        public class PatientService: IPatient
        {
            private readonly ApplicationDbContext _context;
            private readonly UserManager<IdentityUser> _patientUserManager;
            private readonly IConfiguration _config;

            public PatientService(UserManager<IdentityUser> patientUserManager,ApplicationDbContext context, IConfiguration config)
            {
                _patientUserManager = patientUserManager;

                _context = context;
                _config = config;

            }

            public async Task<bool> RegisterPatient(RegisterPatientDTO registerPatientDto)
            {
                using var dataStream=new MemoryStream();
                registerPatientDto.Image.CopyTo(dataStream);
                if (registerPatientDto == null)
                {
                    throw new ArgumentNullException(nameof(registerPatientDto));
                }

                try
                {
                    var identityUser = new IdentityUser
                    {
                        UserName = registerPatientDto.Email,
                        Email = registerPatientDto.Email,
                   
                    };

                    var result = await _patientUserManager.CreateAsync(identityUser, registerPatientDto.Password);

                    if (result.Succeeded)
                    {
                    var patient = new Patient
                    {
                            AdminId = 1,
                            Email = identityUser.Email,
                            PatientName = registerPatientDto.FullName,
                            Phone = registerPatientDto.Phone,
                            Image=dataStream.ToArray(),
                            Gender=registerPatientDto.Gender,
                            DateOfBirth=registerPatientDto.DateOfBirth,
                    };
                        _context.Patients.Add(patient);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
     
                        Console.WriteLine($"Error in RegisterPatient: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }

                    return result.Succeeded;
                }
                catch (Exception ex)
                {
               
                    Console.WriteLine($"Error in RegisterPatient: {ex.Message}");
                    throw;
                }
            }


            public IEnumerable<DoctorDTO> GetAllDoctors(int page, int pageSize, string search)
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
                 .Select(doctor => new DoctorDTO
                 {
                    Image=doctor.Image,
                    FullName = doctor.FullName,
                    Email = doctor.Email,
                    Phone = doctor.Phone,
                    Specialize = doctor.Specialize,
                    Price = doctor.Price,
                    Gender = doctor.Gender.ToString(),
                    Appointments = doctor.Appointments.Select(appointment => new AppointmentDTO
                    {
                        Day = appointment.Day.ToString(),
                        Times = appointment.Times.Select(timeSlot => new TimeSlotDTO
                        {
                            Id = timeSlot.Id,
                            StartTime = timeSlot.StartTime,
                            EndTime=timeSlot.EndTime
                        }).ToList()
                    }).ToList()
                })
                .ToList();

                return doctorDTOs;

            }

            public bool CancelBooking(int bookingId)
            {
                try
                {
                    var booking = _context.Requests.Find(bookingId);

                    if (booking == null)
                    {
                    
                        return false;
                    }

               
                    if (booking.Status == StatusEnum.Cancelled)
                    {
                    
                        return false;
                    }

               
                    booking.Status = StatusEnum.Cancelled;

                    _context.SaveChanges();

                    return true; 
                }
                catch (Exception ex)
                {
                    return false; 
                }
            }

            public bool BookAppointment(int patientId, BookingDTO bookAppointment)
            {
                if (patientId <= 0 || bookAppointment == null || bookAppointment.TimeId <= 0)
                {
                    return false;
                }

                var patient = _context.Patients.FirstOrDefault(p => p.Id == patientId);

                if (patient == null)
                {
                    return false;
                }

                var timeSlot = _context.TimeSlots
                    .Include(ts => ts.Appointment)
                    .ThenInclude(appointment => appointment.Doctor)
                    .FirstOrDefault(ts => ts.Id == bookAppointment.TimeId);

                if (timeSlot == null || timeSlot.Appointment == null || timeSlot.Appointment.Doctor == null)
                {
                    return false;
                }

                var discount = _context.Discounts.FirstOrDefault(d => d.CouponCode == bookAppointment.DiscountCodeCoupon);

           
                if (string.IsNullOrWhiteSpace(bookAppointment.DiscountCodeCoupon))
                {
                
                }

                var request = new Request
                {
                    AppointmentId=timeSlot.AppointmentId,
                    PatientId = patient.Id,
                    DoctorId = timeSlot.Appointment.Doctor.Id,
                    Status = StatusEnum.Pending,
                    TimeSlotId = timeSlot.Id,
                    DiscountCodeCoupon = bookAppointment.DiscountCodeCoupon,
                    DiscountId = discount?.Id  
                };

                _context.Requests.Add(request);
                _context.SaveChanges();

                return true;
            }

            public IEnumerable<BookingResponseDTO> GetPatientBookings(int patientId)
        {
            var bookings = _context.Requests
                .Where(r => r.PatientId == patientId)
                .Include(r => r.Doctor)
                .Include(r => r.Appointment)
                .ThenInclude(appointment => appointment.Times) 
                .Include(r => r.Discount)
                .ToList();

            return bookings.Select(booking => new BookingResponseDTO
            {
                Image = booking.Doctor.Image,
                DoctorName = booking.Doctor.FullName,
                Specialize = booking.Doctor.Specialize,
                Day = booking.Appointment.Day.ToString(),
                Time = booking.Appointment.Times?.Select(timeSlot => new TimeSlotDTO
                {
                    Id = timeSlot.Id,
                    StartTime = timeSlot.StartTime,
                    EndTime = timeSlot.EndTime
                }).ToList(),
                Price = booking.Appointment.Doctor.Price,
                DiscountCode = booking.Discount?.CouponCode ?? string.Empty,
                FinalPrice = Calculations.CalculateFinalPrice(booking.Appointment.Doctor.Price, booking.Discount),
                Status = booking.Status.ToString()
            });
        }


            public async Task<bool> LoginPatient(LoginUserDTO loginPatientDto)
            {

                var identityUser = await _patientUserManager.FindByEmailAsync(loginPatientDto.Email);
                if (identityUser is null)
                {
                    return false;
                }
                return await _patientUserManager.CheckPasswordAsync(identityUser, loginPatientDto.Password);
            }

            public string GeneratePatientTokenString(LoginUserDTO loginPatientDto)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email,loginPatientDto.Email),
                    new Claim(ClaimTypes.Role,"Patient"),
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
        }
    
    
    
    }
