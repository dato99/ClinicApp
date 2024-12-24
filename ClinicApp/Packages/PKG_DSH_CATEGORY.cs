using ClinicApp.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ClinicApp.Packages
{
    public interface IPKG_DSH_CATEGORY
    {
        public List<Category> get_category();
    }
    public class PKG_DSH_CATEGORY : PKG_BASE, IPKG_DSH_CATEGORY
    {
        IConfiguration config;
        public PKG_DSH_CATEGORY(IConfiguration config) : base(config) { }

        public List<Category> get_category()
        {
            List<Category> categories = new List<Category>();
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_dsh_category.Get_category";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;



            OracleDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Category category = new Category();
                category.ID = int.Parse(reader["ID"].ToString());
                category.CategoryName =(reader["name"].ToString());
                
                categories.Add(category);
            }
            conn.Close();

            return categories;
        }
    }
}
