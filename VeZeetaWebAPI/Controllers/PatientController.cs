using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VezeetaServiceLayer.Implementation;
using VezeetaServiceLayer.Interfaces;
using VezeetaDomainLayer.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace VezeetaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class PatientController : ControllerBase
    {
        private readonly IPatient _patientService;
        public PatientController(IPatient patientService)
        {
            _patientService = patientService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterPatient([FromForm] RegisterPatientDTO registerPatientDto)
        {
            bool registrationResult = await _patientService.RegisterPatient(registerPatientDto);

            if (registrationResult)
            {
                return Ok("Patient registered successfully.");
            }
            else
            {

                return BadRequest("Failed to register patient. Please check your input.");
            }
        }

        [HttpPost("login")]
        public async Task <IActionResult> LoginPatient([FromForm] LoginUserDTO loginPatientDto)
        {
            bool loginResult = await _patientService.LoginPatient(loginPatientDto);

            if (loginResult)
            {
                var tokenString = _patientService.GeneratePatientTokenString(loginPatientDto);
                return Ok( tokenString);
            }
            else
            {
                return BadRequest("Login failed. Please check your credentials.");
            }
        }


        [HttpGet("GetAllDoctors/{page}/{pageSize}/{search}")]
        //[Authorize(Roles = "Patient")]
        public IActionResult GetDoctors(int page, int pageSize, string search)
        {
            try
            {
                var doctorsDTO = _patientService.GetAllDoctors(page, pageSize, search);
                return Ok(doctorsDTO);
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, "Internal server error");
            }

        }
        //[Authorize(Roles = "Patient")]
        [HttpPost("book-appointment/{patientId}")]
        public IActionResult BookAppointment(int patientId, [FromForm] BookingDTO bookingRequestDto)
        {
            // Validate the patientId and other inputs
            if (bookingRequestDto == null || string.IsNullOrWhiteSpace(bookingRequestDto.DiscountCodeCoupon) || bookingRequestDto.TimeId <= 0)
            {
                return BadRequest("Invalid request data");
            }

            // Attempt to book the appointment
            var success = _patientService.BookAppointment(patientId, bookingRequestDto);

            if (success)
            {
                return Ok("Appointment booked successfully");
            }

            return BadRequest("Failed to book the appointment");
        }

        [HttpPost("cancel-booking/{bookingId}")]
        //[Authorize(Roles = "Patient")]
        public IActionResult CancelBooking(int bookingId)
        {
            var result = _patientService.CancelBooking(bookingId);

            if (result)
            {
                return Ok(new { Message = "Booking canceled successfully" });
            }
            else
            {
                return BadRequest(new { Message = "Failed to cancel booking" });
            }
        }

        [HttpGet("bookings/{patientId}")]
        //[Authorize(Roles = "Patient")]
        public IActionResult GetAllBookings(int patientId)
        {
            try
            {
                var bookings = _patientService.GetPatientBookings(patientId);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}



