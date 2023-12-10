        using System;
        using System.Collections.Generic;
        using System.Linq;
        using System.Text;
        using System.Threading.Tasks;
        using VezeetaRepositoryLayer;
        using VezeetaServiceLayer.Interfaces;
        using VezeetaDomainLayer.DTOs;
        using Microsoft.EntityFrameworkCore;
        using VezeetaDomainLayer.Enums;
        using VezeetaDomainLayer.Models;
        using Microsoft.AspNetCore.Identity;
        using Microsoft.Extensions.Configuration;
        using Microsoft.IdentityModel.Tokens;
        using System.IdentityModel.Tokens.Jwt;
        using System.Security.Claims;

        namespace VezeetaServiceLayer.Implementation
        {
            public class DoctorService : IDoctor
            {
                private readonly ApplicationDbContext _context;
                private readonly UserManager<IdentityUser> _DoctorUserManager;
                private readonly IConfiguration _config;
               
        
                public DoctorService(UserManager<IdentityUser> DoctorUserManager,ApplicationDbContext context,IConfiguration config)
                    {
                        _context = context;
                        _DoctorUserManager= DoctorUserManager;
                        _config = config;


                    }

                public bool ConfirmCheckUp(int requestId)
                    {
                        try
                        {
                            var request = _context.Requests.Find(requestId);

                            if (request == null || request.IsConfirmed)
                            {
                   
                                return false;
                            }

               
                            request.IsConfirmed = true;

                            _context.SaveChanges();

                            return true; // Confirmation successful
                        }
                        catch (Exception ex)
                        {
                            // Log or handle the exception as needed
                            return false; // Confirmation failed due to an error
                        }
                    }

                public IEnumerable<DoctorRequestDTO> GetAllRequests(int doctorId)
                    {
                        var requests = _context.Requests
                            .Where(r => r.DoctorId == doctorId)
                            .Include(r => r.Patient) 
                            .ToList();
                        using var datastream=new MemoryStream();
            
                        return requests.Select(r => new DoctorRequestDTO
                        {
                            PatientName = r.Patient.PatientName,
                            Image = r.Patient.Image,
                            Age = Calculations.CalculateAge(r.Patient.DateOfBirth), 
                            Gender = r.Patient.Gender.ToString(),
                            Phone = r.Patient.Phone,
                            Email = r.Patient.Email
                        });

                    }

                public bool AddDocAppointment(int doctorId, DocAppointmentDTO appointmentDTO)
                    {
                        var doctor = _context.Doctors.Include(d => d.Appointments).FirstOrDefault(d => d.Id == doctorId);

                        if (doctor == null)
                        {
                            return false;
                        }
                     doctor.Price = appointmentDTO.Price;



                  var newAppointment = new Appointment
                        {
                            DoctorId=doctor.Id,
                            Day = appointmentDTO.Days,
                            Times = appointmentDTO.TimeSlots.Select(ts => new TimeSlot
                            {
                                StartTime = ts.StartTime,
                                EndTime = ts.EndTime
                            }).ToList(),
                  
                        };


                        doctor.Appointments.Add(newAppointment);
                
                        _context.SaveChanges();

                        return true;
                    }

                public bool UpdateAppointment(int doctorId, int appointmentId, DocAppointmentDTO updatedAppointmentDto)
                    {
               

                        var doctor = _context.Doctors.Include(d => d.Appointments).FirstOrDefault(d => d.Id == doctorId);

                        if (doctor == null)
                        {
                            return false;
                        }

                        var existingAppointment = doctor.Appointments.FirstOrDefault(a => a.Id == appointmentId);

                        if (existingAppointment == null)
                        {
                            return false;
                        }

 
                        existingAppointment.Day = updatedAppointmentDto.Days;
                        existingAppointment.Times = updatedAppointmentDto.TimeSlots
                            .Select(ts => new TimeSlot
                            {
                                StartTime = ts.StartTime,
                                EndTime = ts.EndTime
                            }).ToList();

                       
                        _context.SaveChanges();

                        return true;
                    }


                public bool IsTimeSlotAvailable(DocAppointmentDTO updatedAppointmentDto)
                {

                    var overlappingAppointments = _context.Appointments
                        .Where(a => a.Day == updatedAppointmentDto.Days)
                        .SelectMany(a => a.Times)
                        .Any(existingTimeSlot =>
                            updatedAppointmentDto.TimeSlots.Any(updatedTimeSlot =>
                                Calculations.IsOverlap(existingTimeSlot.StartTime, existingTimeSlot.EndTime, updatedTimeSlot.StartTime, updatedTimeSlot.EndTime)
                            )
                        );

                    return !overlappingAppointments;
                }

                public bool DeleteTimeSlot(int appointmentId, int timeSlotId)
                {
                    var appointment = _context.Appointments
                        .Include(a => a.Times)
                        .Include(a => a.Requests) 
                        .SingleOrDefault(a => a.Id == appointmentId);

                    if (appointment == null)
                    {
                        return false;
                    }

                    var timeSlotToRemove = appointment.Times.FirstOrDefault(ts => ts.Id == timeSlotId);

                    if (timeSlotToRemove == null)
                    {
                        return false;
                    }

                    if (timeSlotToRemove.Requests.Any())
                    {
                        
                        return false;
                    }

                    
                    appointment.Times.Remove(timeSlotToRemove);

                   
                    if (!appointment.Times.Any())
                    {
                        _context.Appointments.Remove(appointment);
                    }

                    _context.SaveChanges();

                    return true;
                }


                public async Task<bool> LoginDoctor(LoginUserDTO loginDoctorDto)
                            {
                                var doctorIdentityUser = await _DoctorUserManager.FindByEmailAsync(loginDoctorDto.Email);
                                if (doctorIdentityUser == null)
                                {
                                    return false;
                                }
                                var result = await _DoctorUserManager.CheckPasswordAsync(doctorIdentityUser, loginDoctorDto.Password);
                                return result;
                            }

                public string GenerateDoctorTokenString(LoginUserDTO loginDoctortDto)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Email,loginDoctortDto.Email),
                            new Claim(ClaimTypes.Role,"Doctor"),
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


