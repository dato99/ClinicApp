
using ClinicApp.Auth;
using ClinicApp.Models;
using ClinicApp.Packages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;


namespace ClinicApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UserController : MainController
    {
        IPKG_USERS_D package;
        private readonly IJwtManager jwtManager;
        IPKG_LOGS logs;


        public UserController(IPKG_USERS_D package, IJwtManager jwtManager, IPKG_LOGS logs)
        {
            this.package = package;
            this.jwtManager = jwtManager;
            this.logs = logs;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            List<User> users = new List<User>();
            try
            {
                users = package.get_users();
            }
            catch (Exception ex)

            {
                logs.add_log(ex.Message, AuthUser != null ? AuthUser.Email : null);
                return StatusCode(StatusCodes.Status500InternalServerError, "System error, Try Again.");
            }
            return Ok(users);
        }

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrEmpty(user.FirstName))
                    return BadRequest("First name is required!");

                if (string.IsNullOrEmpty(user.LastName))
                    return BadRequest("Last name is required!");

                if (string.IsNullOrEmpty(user.Email))
                    return BadRequest("Email is required!");

                if (string.IsNullOrEmpty(user.PersonalNumber))
                    return BadRequest("Personal ID is required!");
                if (string.IsNullOrEmpty(user.Password))
                    return BadRequest("Personal ID is required!");

                if (user.Id <= 0) // Assuming Id should be a positive integer
                    return BadRequest("Valid ID is required!");

                if (string.IsNullOrEmpty(user.Role) || !new[] { "User", "Doctor", "Admin" }.Contains(user.Role))
                {
                    return BadRequest("Valid role is required! Role should be 'User', 'Doctor', or 'Admin'.");
                }

                // Add user to the system
                package.add_user(user);

                return Ok("User added successfully!");
            }
            catch (Exception ex)
            {
                logs.add_log(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "System error. Try again.");
            }
        }


        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            try
            {
                // Validate input
                if (user == null)
                    return BadRequest("User data is required.");
                if (id <= 0 || id != user.Id)
                    return BadRequest("Invalid user ID.");
                if (string.IsNullOrEmpty(user.FirstName))
                    return BadRequest("First name is required.");
                if (string.IsNullOrEmpty(user.LastName))
                    return BadRequest("Last name is required.");
                if (string.IsNullOrEmpty(user.Email))
                    return BadRequest("Email is required.");
                if (string.IsNullOrEmpty(user.PersonalNumber))
                    return BadRequest("Personal number is required.");
                if (string.IsNullOrEmpty(user.Password))
                    return BadRequest("Password is required.");

                // Call the data layer to update the user
                package.update_user(user); // Assuming 'package' is your data layer class

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
        public IActionResult DeleteUser(User user)
        {
            try
            {
                package.delete_user(user);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "System error. Try again.");
            }
            return Ok();
        }

        [HttpPost("Login")]
        public IActionResult Authenticate(LoginRequest loginData)
        {
            Token? token = null;
            User? user = null;

            user = package.authenticate(loginData);
            if (user == null) return Unauthorized("Usrname or password is incorrect");


            token = jwtManager.GetToken(user);

            return Ok(token);


        }


    }
}



