using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace ClinicApp.Models
{
    public class User 

    {
      
        public int Id { get; set; }
  
        public string FirstName { get; set; }
   
        public string LastName { get; set; }
    
        public string PersonalNumber { get; set; }
       
        public string Email { get; set; }
       
        public string Password { get; set; }

        public string Role { get; set; } // "User" or "Doctor"
    }

}
