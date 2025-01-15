namespace ClinicApp.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int UserId{ get; set; }
        public int DoctorId { get; set; }
        public string Problem { get; set; }                                                  
        public DateTime DateTime { get; set; }

    }
}
