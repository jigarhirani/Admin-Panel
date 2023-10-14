using Admin_Panel.Areas.MST_Branch.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace Admin_Panel.Areas.MST_Branch.Controllers
{
    [Area("MST_Branch")]
    [Route("MST_Branch/{Controller}/{Action}")]
    public class MST_BranchController : Controller
    {

        public IConfiguration Configuration;
        public MST_BranchController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IActionResult BranchList()
        {
            string connectionString = this.Configuration.GetConnectionString("MyConnection");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = "PR_Branch_SelectAll";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = sqlCommand.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            sqlConnection.Close();
            return View(dataTable);
        }

        public IActionResult BranchAddEdit(int? BranchID)
        {
            if (BranchID != null)
            {
                string connectionstr = this.Configuration.GetConnectionString("MyConnection");
                DataTable dt = new DataTable();
                SqlConnection sqlconnection = new SqlConnection(connectionstr);
                sqlconnection.Open();
                SqlCommand objcmd = sqlconnection.CreateCommand();
                objcmd.CommandType = CommandType.StoredProcedure;
                objcmd.CommandText = "SelectBy_PK_Branch";
                objcmd.Parameters.AddWithValue("BranchID", BranchID);
                SqlDataReader sqldatareader = objcmd.ExecuteReader();
                dt.Load(sqldatareader);
                MST_BranchModel model = new MST_BranchModel();
                foreach (DataRow dr in dt.Rows)
                {
                    model.BranchID = int.Parse(dr["BranchID"].ToString());
                    model.BranchName = dr["BranchName"].ToString();
                    model.BranchCode = dr["BranchCode"].ToString();
                }
                return View(model);
            }
            return View();
        }

        public IActionResult AddEditMethod(MST_BranchModel model)
        {
            string connectionstr = this.Configuration.GetConnectionString("MyConnection");
            DataTable dt = new DataTable();
            SqlConnection sqlconnection = new SqlConnection(connectionstr);
            sqlconnection.Open();
            SqlCommand objcmd = sqlconnection.CreateCommand();
            objcmd.CommandType = CommandType.StoredProcedure;
            if (model.BranchID == null)
            {
                objcmd.CommandText = "Insert_Branch";

                objcmd.Parameters.AddWithValue("BranchName", model.BranchName);
                objcmd.Parameters.AddWithValue("BranchCode", model.BranchCode);
                objcmd.ExecuteNonQuery();
            }
            else
            {
                objcmd.CommandText = "Update_Branch";
                objcmd.Parameters.AddWithValue("BranchID", model.BranchID);
                objcmd.Parameters.AddWithValue("BranchName", model.BranchName);
                objcmd.Parameters.AddWithValue("BranchCode", model.BranchCode);
                objcmd.ExecuteNonQuery();
            }
            return RedirectToAction("BranchList");
        }
        public IActionResult DeleteBranch(int BranchID)
        {
            string connectionString = this.Configuration.GetConnectionString("MyConnection");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            // string Query = "Delete From LOC_Country Where CountryID = @cid";//
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = "Delete_Branch";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@bid", BranchID);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return RedirectToAction("BranchList");
        }
        public IActionResult BranchSearch(string branchName, string branchCode)
        {
            string connectionstring = this.Configuration.GetConnectionString("MyConnection");

            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionstring);
            conn.Open();
            SqlCommand objcmd = conn.CreateCommand();
            objcmd.CommandType = CommandType.StoredProcedure;
            objcmd.CommandText = "PR_BranchSearch";
            objcmd.Parameters.AddWithValue("@BranchName", branchName);
            objcmd.Parameters.AddWithValue("@BranchCode", branchCode);
            SqlDataReader reader = objcmd.ExecuteReader();
            dt.Load(reader);
            conn.Close();

            return View("Branchlist", dt);
        }
    }
}
