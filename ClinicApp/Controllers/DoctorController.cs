using ClinicApp.Models;
using ClinicApp.Packages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;

namespace ClinicApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : MainController
    {
        IPKG_DOCTORS_D package;
        public DoctorController(IPKG_DOCTORS_D package)
        {
            this.package = package;
        }

        [HttpGet]
        public IActionResult GetDoctors()
        {
            List<Doctor> doctors = new List<Doctor>();
            try
            {
                doctors = package.get_doctors();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "System error, Try Again.");
            }
            return Ok(doctors);
        }

        [HttpPost]
        public async Task<IActionResult> AddDoctor(Doctor doctor)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrEmpty(doctor.FirstName)) return BadRequest("First name is required!");
                if (string.IsNullOrEmpty(doctor.LastName)) return BadRequest("Last name is required!");
                if (string.IsNullOrEmpty(doctor.Email)) return BadRequest("Email is required!");
                if (string.IsNullOrEmpty(doctor.PersonalNumber)) return BadRequest("Personal ID is required!");
                if (string.IsNullOrEmpty(doctor.Password)) return BadRequest("Password is required!");
                //if (string.IsNullOrEmpty(doctor.CategoryId)) return BadRequest("Category is required!");
                //doctor.CreatedAt = DateTime.Now;

                package.add_doctor(doctor);


                return Ok("Doctor added successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"System error: {ex.Message}");
            }
        }
     

        [HttpPut("{id}")]
        public IActionResult UpdateDoctor(int id, [FromBody] Doctor doctor)
        {
            try
            {
                // Validate input
                if (doctor == null)
                    return BadRequest("User data is required.");
                if (id <= 0 || id != doctor.ID)
                    return BadRequest("Invalid user ID.");
                if (string.IsNullOrEmpty(doctor.FirstName))
                    return BadRequest("First name is required.");
                if (string.IsNullOrEmpty(doctor.LastName))
                    return BadRequest("Last name is required.");
                if (string.IsNullOrEmpty(doctor.Email))
                    return BadRequest("Email is required.");
                if (string.IsNullOrEmpty(doctor.PersonalNumber))
                    return BadRequest("Personal number is required.");
                if (string.IsNullOrEmpty(doctor.Password))
                    return BadRequest("Password is required.");
                //if (string.IsNullOrEmpty(doctor.Category))
                //    return BadRequest("Password is required.");

                // Call the data layer to update the user
                package.update_doctor(doctor); // Assuming 'package' is your data layer class

                return Ok("User updated successfully.");
            }
            catch (OracleException ex)
            {
                // Log Oracle-specific errors
                Console.WriteLine($"Oracle error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log general errors
                Console.WriteLine($"General error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }
        [HttpDelete]
        public IActionResult DeleteUser(Doctor doctor)
        {
            try
            {
                package.delete_doctor(doctor);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "System error. Try again.");
            }
            return Ok();
        }
    }

}
