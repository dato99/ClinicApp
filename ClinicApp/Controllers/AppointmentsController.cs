using ClinicApp.Models;
using ClinicApp.Packages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        IPKG_APPOINTMENTS_D package;
        public AppointmentsController(IPKG_APPOINTMENTS_D package)
        {
            this.package = package;
        }
        [HttpPost]
        public async Task<IActionResult> AddAppointment(Appointment appointment)
        {
            try
            {
                // Validate required fields
                //if (string.IsNullOrEmpty(appointment.UserId)) return BadRequest("not valid userId");
                //if (string.IsNullOrEmpty(appointment.DoctorId)) return BadRequest("not valid doctorId");
                //if (string.IsNullOrEmpty(appointment.Problem)) return BadRequest("error!");
                //if (string.IsNullOrEmpty(appointment.DateTime)) return BadRequest("Personal ID is required!");
            
               

                package.add_appointment(appointment);


                return Ok("Appointment Add seccesfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"System error: {ex.Message}");
            }
        }


        [HttpDelete]
        public IActionResult DeleteAppointment(Appointment appointment)
        {
            try
            {
                package.delete_appointment(appointment);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "System error. Try again.");
            }
            return Ok();
        }
    }
}
