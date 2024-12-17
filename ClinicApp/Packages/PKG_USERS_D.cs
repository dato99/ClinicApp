using ClinicApp.Models;
using Microsoft.AspNetCore.Identity;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using ClinicApp.Packages;
using System.Data.Common;
using Oracle.ManagedDataAccess.Types;
using ClinicApp.Auth;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ClinicApp.Packages
{
    public interface IPKG_USERS_D
    {
        public List<User> get_users();
        public void add_user(User user);
        public void delete_user(User user);
        public void update_user(User user);
        public User? authenticate(LoginRequest loginData);

        //public void get_user_by_id(User user);
    }

    public class PKG_USERS_D : PKG_BASE, IPKG_USERS_D
    {
        IConfiguration config;
        public PKG_USERS_D(IConfiguration config) : base(config) { }


        public void add_user(User user)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_dsh_users.add_user";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_first_name", OracleDbType.Varchar2).Value = user.FirstName;
            cmd.Parameters.Add("p_last_name", OracleDbType.Varchar2).Value = user.LastName;
            cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = user.Email;
            cmd.Parameters.Add("p_personal_number", OracleDbType.Varchar2).Value = user.PersonalNumber;
            cmd.Parameters.Add("p_passwordhash", OracleDbType.Varchar2).Value = user.Password;
            cmd.Parameters.Add(new OracleParameter("Role", user.Role)); // Should be 'User', 'Doctor', or 'Admin'


            cmd.ExecuteNonQuery();

            conn.Close();

        }
        public List<User> get_users()
        {
            List<User> users = new List<User>();
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_dsh_users.get_users";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;



            OracleDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                User user = new User();
                user.Id = int.Parse(reader["id"].ToString());
                user.FirstName = reader["first_name"].ToString();
                user.LastName = reader["last_name"].ToString();
                user.Email = reader["email"].ToString();
                user.PersonalNumber = reader["personal_number"].ToString();
                user.Password = reader["passwordhash"].ToString();

                users.Add(user);
            }
            conn.Close();

            return users;
        }

        public void delete_user(User user)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_dsh_users.delete_user";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = user.Id;

            cmd.ExecuteNonQuery();
            conn.Close();

        }

        //public void update_user(User user)
        //{
        //    OracleConnection conn = new OracleConnection();
        //    conn.ConnectionString = ConnStr;
        //    conn.Open();

        //    OracleCommand cmd = new OracleCommand();
        //    cmd.Connection = conn;
        //    cmd.CommandText = "olerning.PKG_users_d.update_user";
        //    cmd.CommandType = CommandType.StoredProcedure;

        //    cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = user.Id;

        //    cmd.ExecuteNonQuery();
        //    conn.Close();

        //}
        public void update_user(User user)
        {
            using (OracleConnection conn = new OracleConnection(ConnStr))
            {
                conn.Open();

                using (OracleCommand cmd = new OracleCommand("olerning.PKG_dsh_users.update_user", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add all required parameters
                    cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = user.Id;
                    cmd.Parameters.Add("p_first_name", OracleDbType.Varchar2).Value = user.FirstName;
                    cmd.Parameters.Add("p_last_name", OracleDbType.Varchar2).Value = user.LastName;
                    cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = user.Email;
                    cmd.Parameters.Add("p_personal_number", OracleDbType.Varchar2).Value = user.PersonalNumber;
                    cmd.Parameters.Add("p_passwordhash", OracleDbType.Varchar2).Value = user.Password;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public User? authenticate(LoginRequest loginData)
        {
           

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_dsh_users.authenticate";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = loginData.Email;
            cmd.Parameters.Add("p_password", OracleDbType.Varchar2).Value = loginData.Password;
            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            OracleDataReader reader = cmd.ExecuteReader();
          

            //OracleParameter resultParam = new OracleParameter("p_result", OracleDbType.Int32)
            //{
            //    Direction = ParameterDirection.Output
            //};
            //cmd.Parameters.Add(resultParam);

            //// Execute the procedure
            //cmd.ExecuteNonQuery();

            //// Handle the result parameter
            //int result = 0;
            //if (resultParam.Value != DBNull.Value)
            //{
            //    // Safely cast OracleDecimal to int
            //    result = ((OracleDecimal)resultParam.Value).ToInt32();
            //}

            //// If the result is 0 (failure), return null
            //if (result == 0)
            //{
            //    conn.Close();
            //    return null; // Authentication failed
            //}

            //// If successful, fetch the user details
            //OracleCommand cmdUser = new OracleCommand("SELECT id, email, passwordhash FROM users_d WHERE email = :email", conn);
            //cmdUser.Parameters.Add(":email", OracleDbType.Varchar2).Value = loginData.Email;

            //OracleDataReader reader = cmdUser.ExecuteReader();
            //User? user = null;

            //if (reader.Read()) // Only create the user object if a record is found
            //{
            //    user = new User
            //    {
            //        Id = Convert.ToInt32(reader["id"]),
            //        Email = reader["email"].ToString(),
            //        Password = reader["passwordhash"].ToString()
            //    };
            //}
            

            //reader.Close();
            //conn.Close();

            //return user; // Return the authenticated u



            User? user = null;
            if(reader.Read())
            {
                user = new User();
                user.Id = int.Parse(reader["id"].ToString());
                user.Email = (reader["email"].ToString());
                //user.Password = (reader["passwordhash"].ToString());
                
            }

            cmd.ExecuteNonQuery();

            conn.Close();
            return user;

        }




        //public User get_user_by_id(User user)
        //{
        //    OracleConnection conn = new OracleConnection();
        //    conn.ConnectionString = ConnStr;
        //    conn.Open();

        //    OracleCommand cmd = new OracleCommand();
        //    cmd.Connection = conn;
        //    cmd.CommandText = "olerning.PKG_users_d.add_user";

        //    cmd.CommandType = CommandType.StoredProcedure;

        //    cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = user.Id;
        //    cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;



        //    OracleDataReader reader = cmd.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        user.FirstName = reader["first_name"].ToString();
        //        user.LastName = reader["last_name"].ToString();
        //        user.Email = reader["email"].ToString();
        //        user.PersonalNumber = reader["personal_number"].ToString();
        //        user.Password = reader["passwordhash"].ToString();
        //    }
        //    conn.Close();

        //    return user;

        //}
    }
}

