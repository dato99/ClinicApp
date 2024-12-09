//using ClinicApp.Controllers;
//using ClinicApp.Packages;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity.Data;
//using Microsoft.AspNetCore.Mvc;
//using ClinicApp.Auth;
////using ClinicApp.Auth.ClinicApp.Auth;
//using ClinicApp.Models;
//using Oracle.ManagedDataAccess.Client;
//using Microsoft.AspNetCore.Authentication.OAuth.Claims;
//using ClinicApp.Models;


//namespace ClinicApp.Auth
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AutheController : MainController
//    {
//        IPKG_USERS_D package;
//        IPKG_LOGS logs;
//        private readonly IJwtManager _jwtManager;

//        public AutheController(IPKG_USERS_D package, IJwtManager jwtManager)
//        {
//            this.package = package;
//            _jwtManager = jwtManager;
//        }

//        [HttpPost("authenticate")]
//        public IActionResult Authenticate([FromBody] loginRequest loginRequest)
//        {
//            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
//            {
//                return BadRequest("Email and password are required.");
//            }

//            bool isAuthenticated = package.Authenticate(loginRequest.Email, loginRequest.Password);

//            if (isAuthenticated)
//            {
//                // Generate JWT token
//                var token = _jwtManager.GenerateToken(loginRequest.Email);

//                return Ok(new
//                {
//                    Message = "Authentication successful.",
//                    Token = token
//                });
//            }

//            return Unauthorized(new { Message = "Invalid email or password." });
//        }
//    }

    //private User ValidateUser(string email, string password)
    //{
    //    User user = null;


    //    using (var connection = new OracleConnection(ConnStr))
    //    {
    //        connection.Open();

    //        using (var command = new OracleCommand("SELECT id, first_name, last_name, email, personal_number, role FROM users_d WHERE email = :email AND passwordhash = :passwordhash", connection))
    //        {
    //            // Add parameters to prevent SQL injection
    //            command.Parameters.Add(new OracleParameter(":email", OracleDbType.Varchar2)).Value = email;
    //            command.Parameters.Add(new OracleParameter(":passwordhash", OracleDbType.Varchar2)).Value = (password); 

    //            using (var reader = command.ExecuteReader())
    //            {
    //                if (reader.Read()) // If a match is found
    //                {
    //                    user = new User
    //                    {
    //                        Id = reader.GetInt32(0),
    //                        FirstName = reader.GetString(1),
    //                        LastName = reader.GetString(2),
    //                        Email = reader.GetString(3),
    //                        PersonalNumber = reader.GetString(4)
    //                    };
    //                }
    //            }
    //        }
    //    }

    //    return user;
    //}
//    public class loginRequest
//    {
//        public string Email { get; set; }
//        public string Password { get; set; }
//    }
//}

//        private Doctor ValidateDoctor(string email, string password)
//        {

//            Doctor doctor = null;


//            using (var connection = new OracleConnection(ConnStr))
//            {
//                connection.Open();

//                using (var command = new OracleCommand("SELECT id, first_name, last_name, email, personal_number, role FROM users_d WHERE email = :email AND passwordhash = :passwordhash", connection))
//                {
//                    // Add parameters to prevent SQL injection
//                    command.Parameters.Add(new OracleParameter(":email", OracleDbType.Varchar2)).Value = email;
//                    command.Parameters.Add(new OracleParameter(":password", OracleDbType.Varchar2)).Value = (password); // Ensure passwords are hashed

//                    using (var reader = command.ExecuteReader())
//                    {
//                        if (reader.Read()) // If a match is found
//                        {
//                            doctor = new Doctor
//                            {
//                                ID = reader.GetInt32(0),
//                                FirstName = reader.GetString(1),
//                                LastName = reader.GetString(2),
//                                Email = reader.GetString(3),
//                                PersonalNumber = reader.GetString(4)
//                            };
//                        }
//                    }
//                }
//            }
//            return doctor;
//        }
//    }


//}
