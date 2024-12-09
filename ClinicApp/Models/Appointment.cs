namespace ClinicApp.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string UserId{ get; set; }
        public string DoctorId { get; set; }
        public string Problem { get; set; }                                                  
        public DateTime DateTime { get; set; }

    }
}
