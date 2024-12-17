using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ClinicApp.Packages
{
    public interface IPKG_LOGS
    {

        public void add_log(string message, string? email = null);
    }
    public class PKG_LOGS : PKG_BASE, IPKG_LOGS
    {
        IConfiguration config;
        public PKG_LOGS(IConfiguration config) : base(config) { }

        public void add_log(string message, string? email = null)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();
            OracleCommand cmd = new OracleCommand();

            cmd.Connection = conn;
            cmd.CommandText = "olerning.pkg_logs_d.add_log";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("p_error_message", OracleDbType.Varchar2).Value = message;
            cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = email;


            cmd.ExecuteNonQuery();
            conn.Close();
        }



    }
}
