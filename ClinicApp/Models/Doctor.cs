using System.Text.Json.Serialization;

namespace ClinicApp.Models
{
    public class Doctor : Category
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PersonalNumber { get; set; }
        public string Password { get; set; }
        //public string Category { get; set; }

        public byte[] CV { get; set; }
        public byte[] Profile_pic { get; set; }

        public DateTime? CreatedAt { get; set; }
        public int CategoryId { get; set; } 
        public string DoctorReview { get; set; } 
        //public int DoctorReview { get; set; }
        //public int RoleId { get; set; } // Assuming 2 is a doctor role ID
        //public int CategoryId { get; set; }
        //public Category category { get; set; }

   

    }
}
