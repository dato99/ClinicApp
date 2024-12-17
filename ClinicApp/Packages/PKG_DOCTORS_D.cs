using ClinicApp.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using ClinicApp.Packages;
using System.Numerics;


namespace ClinicApp.Packages
{
    public interface IPKG_DOCTORS_D
    {
        public List<Doctor> get_doctors();
        public void add_doctor(Doctor doctor);
        public void delete_doctor(Doctor doctor);
        public void update_doctor(Doctor doctor);
    }

    public class PKG_DOCTORS_D : PKG_BASE, IPKG_DOCTORS_D
    {
        IConfiguration config;
        public PKG_DOCTORS_D(IConfiguration config) : base(config) { }
        public void add_doctor(Doctor doctor)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_dsh_doctors.add_doctor";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("v_first_name", OracleDbType.Varchar2).Value = doctor.FirstName;
            cmd.Parameters.Add("v_last_name", OracleDbType.Varchar2).Value = doctor.LastName;
            cmd.Parameters.Add("v_email", OracleDbType.Varchar2).Value = doctor.Email;
            cmd.Parameters.Add("v_personal_number", OracleDbType.Varchar2).Value = doctor.PersonalNumber;
            cmd.Parameters.Add("v_profile_pic", OracleDbType.Blob).Value =
               doctor.Profile_pic != null ? doctor.Profile_pic : DBNull.Value;

            cmd.Parameters.Add("v_cv", OracleDbType.Blob).Value =
                doctor.CV != null ? doctor.CV : DBNull.Value;
            cmd.Parameters.Add("v_category_id", OracleDbType.Int32).Value = doctor.CategoryId;  // Use CategoryId as FK

            cmd.Parameters.Add("v_password", OracleDbType.Varchar2).Value = doctor.Password;
            //cmd.Parameters.Add("p_category", OracleDbType.Varchar2).Value = doctor.Category;
           
            //cmd.Parameters.Add("v_profile_pic", OracleDbType.Int32).Value = doctor.CategoryId;  // Use CategoryId as FK
            //cmd.Parameters.Add("v_cv", OracleDbType.Int32).Value = doctor.CategoryId;  // Use CategoryId as FK
            

          





            //cmd.Parameters.Add("p_created_at", OracleDbType.TimeStamp).Value = doctor.CreatedAt ?? DateTime.Now;



            cmd.ExecuteNonQuery();

            conn.Close();

        }
        public List<Doctor> get_doctors()
        {
            List<Doctor> doctors = new List<Doctor>();
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_dsh_DOCTORs.get_doctors";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;



            OracleDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Doctor doctor = new Doctor();
                doctor.ID = int.Parse(reader["ID"].ToString());
                doctor.FirstName = reader["firstname"].ToString();
                doctor.LastName = reader["LASTNAME"].ToString();
                doctor.Email = reader["EMAIL"].ToString();
                //doctor.PersonalNumber = reader["PERSONAL_NUMBER"].ToString();
                doctor.CategoryName = reader["name"].ToString();
                doctor.DoctorReview = reader["doctor_review"].ToString();

                //doctor.Password = reader["PASSWORD"].ToString();
                //doctor.CategoryId =int.Parse( reader["category_id"].ToString());
                if (reader["PROFILE_PIC"] != DBNull.Value)
                {
                    doctor.Profile_pic = (byte[])reader["PROFILE_PIC"];
                }

                //if (reader["created_at"] != DBNull.Value)
                //{
                //    // Correctly parse the 'created_at' field as DateTime
                //    doctor.CreatedAt = Convert.ToDateTime(reader["created_at"]);
                //}
                //else
                //{
                //    doctor.CreatedAt = null;  // Handle null case if 'created_at' is nullable
                //}
                doctors.Add(doctor);
            }
            conn.Close();

            return doctors;
        }

        public void delete_doctor(Doctor doctor)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_dsh_doctors.delete_doctor";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = doctor.ID;

            cmd.ExecuteNonQuery();
            conn.Close();

        }
        public void update_doctor(Doctor doctor)
        {
            using (OracleConnection conn = new OracleConnection(ConnStr))
            {
                conn.Open();

                using (OracleCommand cmd = new OracleCommand("olerning.PKG_dsh_doctors.update_doctor", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add all required parameters
                    cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = doctor.ID;
                    cmd.Parameters.Add("p_firstname", OracleDbType.Varchar2).Value = doctor.FirstName;
                    cmd.Parameters.Add("p_lastname", OracleDbType.Varchar2).Value = doctor.LastName;
                    cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = doctor.Email;
                    cmd.Parameters.Add("p_personal_number", OracleDbType.Varchar2).Value = doctor.PersonalNumber;
                    cmd.Parameters.Add("p_password", OracleDbType.Varchar2).Value = doctor.Password;
                    //cmd.Parameters.Add("p_category", OracleDbType.Varchar2).Value = doctor.Category;

                    cmd.Parameters.Add("p_created_at", OracleDbType.TimeStamp).Value = doctor.CreatedAt;

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
