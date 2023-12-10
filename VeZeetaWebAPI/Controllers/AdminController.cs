using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VezeetaDomainLayer.DTOs;
using VezeetaServiceLayer.Implementation;
using VezeetaServiceLayer.Interfaces;


namespace VezeetaAPI.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        public class AdminController : ControllerBase
        {
            private readonly IAdmin _AdminService;
            public AdminController(IAdmin AdminService)
            {
                _AdminService = AdminService;
            }
      

            [HttpPost("Admin Login")]
            public async Task<IActionResult> LoginAdmin([FromForm] LoginUserDTO loginAdminDto)
            {
                var loginResult = await _AdminService.LoginAdmin(loginAdminDto);

                if (loginResult)
                {
                    var tokenString = _AdminService.GenerateAdminTokenString(loginAdminDto);
                    return Ok(tokenString);
                }
                else
                {
                    return BadRequest("Login failed. Please check your credentials.");
                }
            }

            [HttpGet("get-all-doctors")]
            public IActionResult GetAllDoctors(int page, int pageSize, string search)
            {
                var doctors = _AdminService.GetAllDoctors(page, pageSize, search);
                return Ok(doctors);
            }

            [HttpGet("total-doctors-count")]
            public IActionResult GetTotalDoctorsCount()
            {
                var count = _AdminService.GetTotalDoctorsCount();
                return Ok(new { TotalDoctorsCount = count });
            }

            [HttpGet("total-patients-count")]
            public IActionResult GetTotalPatientsCount()
            {
                var count = _AdminService.GetTotalPatientsCount();
                return Ok(new { TotalPatientsCount = count });
            }

            [HttpGet("get-doctor-by-id/{doctorId}")]
            public IActionResult GetDoctorById(int doctorId)
            {
                var doctorDetails = _AdminService.GetDoctorById(doctorId);

                if (doctorDetails != null)
                {
                    return Ok(new[] { new { details = doctorDetails } });
                }
                else
                {
                    return NotFound($"Doctor with ID {doctorId} not found.");
                }
            }

            [HttpPost("add-doctor")]
            public async Task<IActionResult> AddDoctor([FromForm] DoctorDetailsDTO doctor)
            {
                if (await _AdminService.AddDoctor(doctor))
                {
                    return Ok(new { Message = "Doctor added successfully." });
                }
                else
                {
                    return BadRequest(new { Message = "Failed to add the doctor." });
                }
            }

            [HttpPut("edit-doctor/{doctorId}")]
            public async Task<IActionResult> EditDoctor(int doctorId, [FromForm] DoctorDetailsDTO doctor)
            {
                if (await _AdminService.EditDoctor(doctorId, doctor))
                {
                    return Ok(new { Message = "Doctor edited successfully." });
                }
                else
                {
                    return BadRequest(new { Message = "Failed to edit the doctor." });
                }
            }

            [HttpDelete("delete-doctor/{doctorId}")]
            public IActionResult DeleteDoctor(int doctorId)
            {
                if (_AdminService.DeleteDoctor(doctorId))
                {
                    return Ok(new { Message = "Doctor deleted successfully." });
                }
                else
                {
                    return BadRequest(new { Message = "Failed to delete the doctor." });
                }
            }

            [HttpGet("get-all-patients")]
            public IActionResult GetAllPatients(int page, int pageSize, string search )
            {
                var patients = _AdminService.GetAllPatients(page, pageSize, search );

                return Ok(patients);
            }

            [HttpGet("get-patient/{patientId}")]
            public IActionResult GetPatientById(int patientId)
            {
                var patient = _AdminService.GetPatientById(patientId);

                if (patient != null)
                {
                    return Ok(patient);
                }
                else
                {
                    return NotFound(new { Message = "Patient not found." });
                }
            }

            [HttpGet("request-statistics")]
            public IActionResult GetRequestStatistics()
            {
                var requestStatistics = _AdminService.GetRequestStatistics();
                return Ok(requestStatistics);
            }

            [HttpGet("top-specializations")]
            public IActionResult GetTopSpecializations()
            {
                 int topCount = 5;
                var topSpecializations = _AdminService.GetTopSpecializations(topCount);
                return Ok(topSpecializations);
            }

            [HttpGet("top-doctors")]
            public IActionResult GetTopDoctors()
            {
                int topCount = 10;
                var topDoctors = _AdminService.GetTopDoctors(topCount);
                return Ok(topDoctors);
            }

            [HttpPost("add-discount")]
            public IActionResult AddDiscount([FromForm] AddDiscountDTO discountDTO)
            {
                if (ModelState.IsValid)
                {
                    var result = _AdminService.AddDiscount(discountDTO);

                    if (result)
                    {
                        return Ok("Discount added successfully");
                    }
                    else
                    {
                        return BadRequest("Failed to add discount");
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }

            [HttpPut("update-discount/{id}")]
            public IActionResult UpdateDiscount(int id, [FromForm] AddDiscountDTO updateDiscountDTO)
            {
                if (id <= 0)
                {
                    return BadRequest(new { Message = "Invalid discount id" });
                }

                var success = _AdminService.UpdateDiscount(id, updateDiscountDTO);

                if (success)
                {
                    return Ok(new { Message = "Discount updated successfully" });
                }
                else
                {
                    return BadRequest(new { Message = "Failed to update discount" });
                }
            }


            [HttpDelete("delete-discount/{discountId}")]
            public IActionResult DeleteDiscount(int discountId)
            {
                var isDeleted = _AdminService.DeleteDiscount(discountId);

                if (isDeleted)
                {
                    return Ok(new { Message = "Discount deleted successfully" });
                }
                else
                {
                    return BadRequest(new { Message = "Failed to delete discount" });
                }
            }


            [HttpPut("deactivate-discount/{discountId}")]
            public IActionResult DeactivateDiscount(int discountId)
            {
                var isDeactivated = _AdminService.DeactivateDiscount(discountId);

                if (isDeactivated)
                {
                    return Ok(new { Message = "Discount deactivated successfully" });
                }
                else
                {
                    return BadRequest(new { Message = "Failed to deactivate discount" });
                }
            }
        }
}
