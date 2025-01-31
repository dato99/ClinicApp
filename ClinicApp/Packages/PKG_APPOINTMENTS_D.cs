﻿using ClinicApp.Models;
using ClinicApp.Packages;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ClinicApp.Packages
{
    public interface IPKG_APPOINTMENTS_D
    {
        public List<Appointment> get_appointments();
        public void add_appointment(Appointment appointment);
        public void delete_appointment(Appointment appointment);
        //public void update_appoitment(Appointment appointmnet);
    }
    public class PKG_APPOINTMENTS_D : PKG_BASE, IPKG_APPOINTMENTS_D
    {

        IConfiguration config;
        public PKG_APPOINTMENTS_D(IConfiguration config) : base(config) { }
        public void add_appointment(Appointment appointment)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_dsh_booking.add_booking";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_user_id", OracleDbType.Varchar2).Value = appointment.UserId;
            cmd.Parameters.Add("p_doctor_id", OracleDbType.Varchar2).Value = appointment.DoctorId;
            cmd.Parameters.Add("p_booking_date", OracleDbType.Date).Value = appointment.DateTime;
            cmd.Parameters.Add("p_description", OracleDbType.Varchar2).Value = appointment.Problem;


            cmd.ExecuteNonQuery();

            conn.Close();

        }
        public void delete_appointment(Appointment appointment)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_dsh_booking.delete_booking";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = appointment.Id;

            cmd.ExecuteNonQuery();
            conn.Close();

        }
        public List<Appointment> get_appointments()
        {
            List<Appointment> appointments = new List<Appointment>();
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_dsh_booking.get_booking";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Appointment appointment = new Appointment();
                appointment.Id = int.Parse(reader["id"].ToString());
                appointment.UserId = int.Parse(reader["user_id"].ToString());
                appointment.DoctorId = int.Parse(reader["doctor_id"].ToString());
                appointment.Problem = reader["description"].ToString();

                appointments.Add(appointment);
            }
            conn.Close();

            return appointments;
            
            
        }
    }
}
