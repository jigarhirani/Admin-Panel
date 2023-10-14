using Admin_Panel.Areas.LOC_Country.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Admin_Panel.Areas.LOC_Country.Controllers
{
    [Area("LOC_Country")]
    [Route("LOC_Country/{Controller}/{Action}")]
    public class LOC_CountryController : Controller
    {
        public IConfiguration Configuration;
        public LOC_CountryController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IActionResult CountryList()
        {
            string connectionString = this.Configuration.GetConnectionString("MyConnection");

            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = "PR_Country_SelectAll";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = sqlCommand.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            sqlConnection.Close();
            return View(dataTable);


        }
        public IActionResult CountryAddEdit(int? CountryID)
        {
            if (CountryID != null)
            {
                string connectionstr = this.Configuration.GetConnectionString("MyConnection");
                DataTable dt = new DataTable();
                SqlConnection sqlConnection = new SqlConnection(connectionstr);
                sqlConnection.Open();
                SqlCommand ObjCmd = sqlConnection.CreateCommand();
                ObjCmd.CommandType = CommandType.StoredProcedure;
                ObjCmd.CommandText = "PR_Country_SelectByPK";
                ObjCmd.Parameters.AddWithValue("CountryID", CountryID);
                SqlDataReader sqlDataReader = ObjCmd.ExecuteReader();
                dt.Load(sqlDataReader);
                LOC_CountryModel model = new LOC_CountryModel();
                foreach (DataRow dr in dt.Rows)
                {
                    model.CountryID = int.Parse(dr["CountryId"].ToString());
                    model.CountryName = dr["CountryName"].ToString();
                    model.CountryCode = dr["CountryCode"].ToString();
                }
                return View(model);
            }
            return View();
        }

        public IActionResult AddEditMethod(LOC_CountryModel model)
        {
            string connectionstr = this.Configuration.GetConnectionString("MyConnection");
            DataTable dt = new DataTable();
            SqlConnection sqlConnection = new SqlConnection(connectionstr);
            sqlConnection.Open();
            SqlCommand ObjCmd = sqlConnection.CreateCommand();
            ObjCmd.CommandType = CommandType.StoredProcedure;
            if (model.CountryID == null)
            {
                ObjCmd.CommandText = "PR_Country_Insert";
                ObjCmd.Parameters.AddWithValue("CountryName", model.CountryName);
                ObjCmd.Parameters.AddWithValue("CountryCode", model.CountryCode);
                ObjCmd.ExecuteNonQuery();
            }
            else
            {
                ObjCmd.CommandText = "PR_Country_UpdateByPK";
                ObjCmd.Parameters.AddWithValue("CountryID", model.CountryID);
                ObjCmd.Parameters.AddWithValue("CountryName", model.CountryName);
                ObjCmd.Parameters.AddWithValue("CountryCode", model.CountryCode);
                ObjCmd.ExecuteNonQuery();
            }
            return RedirectToAction("CountryList");
        }

        public IActionResult DeleteCountry(int CountryID)
        {
            string connectionString = this.Configuration.GetConnectionString("MyConnection");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            // string Query = "Delete From LOC_Country Where CountryID = @cid";//
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_Country_DeleteByPK";
            sqlCommand.Parameters.AddWithValue("CountryID", CountryID);
            TempData["ID"] = CountryID;
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return RedirectToAction("CountryList");
        }

        public IActionResult SearchMethod(string countryName, string countryCode)
        {
            string connectionstring = this.Configuration.GetConnectionString("MyConnection");

            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionstring);
            conn.Open();
            SqlCommand objcmd = conn.CreateCommand();
            objcmd.CommandType = CommandType.StoredProcedure;
            objcmd.CommandText = "PR_CountrySearch";
            objcmd.Parameters.AddWithValue("@CountryName", countryName);
            objcmd.Parameters.AddWithValue("@CountryCode", countryCode);
            SqlDataReader reader = objcmd.ExecuteReader();
            dt.Load(reader);
            conn.Close();

            return View("CountryList", dt);
        }

    }
}
