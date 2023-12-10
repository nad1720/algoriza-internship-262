using Microsoft.AspNetCore.Mvc;
using VezeetaDomainLayer.DTOs;
using VezeetaServiceLayer.Implementation;
using VezeetaServiceLayer.Interfaces;

namespace VeZeetaWebAPI.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctor _doctorService;

        public DoctorController(IDoctor doctorService)
        {
            _doctorService = doctorService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginDoctor([FromForm] LoginUserDTO loginDoctorDto)
        {
            bool loginResult = await _doctorService.LoginDoctor(loginDoctorDto);

            if (loginResult)
            {
                var tokenString = _doctorService.GenerateDoctorTokenString(loginDoctorDto);
                return Ok(tokenString);
            }
            else
            {
                return BadRequest("Login failed. Please check your credentials.");
            }
        }
        [HttpGet("{doctorId}/requests")]
        public IActionResult GetAllRequests(int doctorId)
        {
            try
            {
                var requests = _doctorService.GetAllRequests(doctorId);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("confirm-checkup/{requestId}")]
        public IActionResult ConfirmCheckUp(int requestId)
        {
            var result = _doctorService.ConfirmCheckUp(requestId);

            if (result)
            {
                return Ok(new { Message = "Check-up confirmed successfully." });
            }
            else
            {
                return BadRequest(new { Message = "Failed to confirm check-up." });
            }
        }

        // Inside your DoctorController class
        [HttpPost("add-appointment")]
        public IActionResult AddDocAppointment(int doctorId, [FromBody] DocAppointmentDTO appointmentDTO)
        {
            if (appointmentDTO == null)
            {
                return BadRequest("Invalid request payload");
            }

            var result = _doctorService.AddDocAppointment(doctorId, appointmentDTO);

            if (result)
            {
                return Ok("Appointment added successfully");
            }
            else
            {
                return BadRequest("Failed to add appointment");
            }
        }

        [HttpPut("update-appointment/{doctorId}/{appointmentId}")]
        public IActionResult UpdateAppointment(int doctorId, int appointmentId, [FromBody] DocAppointmentDTO updatedAppointmentDto)
        {
            var success = _doctorService.UpdateAppointment(doctorId, appointmentId, updatedAppointmentDto);

            if (success)
            {
                return Ok("Appointment updated successfully");
            }

            return NotFound("Appointment not found or could not be updated");
        }


        [HttpDelete("{appointmentId}/times/{timeSlotId}")]
        public IActionResult DeleteTimeSlot(int doctorId, int appointmentId, int timeSlotId)
        {
            // You might want to add authorization logic here to ensure the doctor can perform this action

            var isDeleted = _doctorService.DeleteTimeSlot(appointmentId, timeSlotId);

            if (isDeleted)
            {
                return Ok(new { message = "Time slot deleted successfully." });
            }

            return NotFound(new { message = "Time slot not found or already booked. Deletion failed." });
        }
    }
}
