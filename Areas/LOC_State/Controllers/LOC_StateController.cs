using Admin_Panel.Areas.LOC_Country.Models;
using Admin_Panel.Areas.LOC_State.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace Admin_Panel.Areas.LOC_State.Controllers
{
    [Area("LOC_State")]
    [Route("LOC_State/[controller]/[action]")]
    public class LOC_StateController : Controller
    {
        public IConfiguration Configuration;
        public LOC_StateController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IActionResult StateList()
        {
            FillDropDownMenu();
            string connectionString = this.Configuration.GetConnectionString("MyConnection");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_State_SelectAll";
            SqlDataReader reader = sqlCommand.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            sqlConnection.Close();
            return View(dataTable);
        }

        public IActionResult StateAddEdit(int? StateID)
        {
            FillDropDownMenu();
            if (StateID != null)
            {
                string connectionString = this.Configuration.GetConnectionString("MyConnection");
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                DataTable dataTable = new DataTable();
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("StateID", StateID);
                sqlCommand.CommandText = "PR_State_SelectByPK";
                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                dataTable.Load(dataReader);
                LOC_StateModel model = new LOC_StateModel();
                foreach (DataRow data in dataTable.Rows)
                {
                    model.CountryID = int.Parse(data["CountryID"].ToString());
                    model.StateID = int.Parse(data["StateID"].ToString());
                    model.StateName = data["StateName"].ToString();
                    model.StateCode = data["StateCode"].ToString();
                }
                return View(model);
            }
            return View();
        }

        public IActionResult AddEditMethod(LOC_StateModel model)
        {
            string connectionstr = this.Configuration.GetConnectionString("MyConnection");
            DataTable dt = new DataTable();
            SqlConnection sqlConnection = new SqlConnection(connectionstr);
            sqlConnection.Open();
            SqlCommand ObjCmd = sqlConnection.CreateCommand();
            ObjCmd.CommandType = CommandType.StoredProcedure;
            if (model.StateID == null)
            {
                ObjCmd.CommandText = "PR_State_Insert";
                ObjCmd.Parameters.AddWithValue("CountryID", model.CountryID);
                ObjCmd.Parameters.AddWithValue("@StateName", model.StateName);
                ObjCmd.Parameters.AddWithValue("@StateCode", model.StateCode);
                ObjCmd.ExecuteNonQuery();
            }
            else
            {
                ObjCmd.CommandText = "PR_State_UpdateByPK";
                ObjCmd.Parameters.AddWithValue("StateID", model.StateID);
                ObjCmd.Parameters.AddWithValue("@StateName", model.StateName);
                ObjCmd.Parameters.AddWithValue("StateCode", model.StateCode);
                ObjCmd.Parameters.AddWithValue("CountryID", model.CountryID);
                ObjCmd.ExecuteNonQuery();
            }
            return RedirectToAction("StateList");
        }

        public IActionResult DeleteState(int StateID)
        {
            string connectionString = this.Configuration.GetConnectionString("MyConnection");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_State_DeleteByPK";
            sqlCommand.Parameters.AddWithValue("StateID", StateID);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return RedirectToAction("StateList");
        }

        public void FillDropDownMenu()
        {
            String ConnectionString = this.Configuration.GetConnectionString("MyConnection");
            List<LOC_CountryDropDownModel> lOC_CountryDropDowns = new List<LOC_CountryDropDownModel>();
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "CountryDropDownList";
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    LOC_CountryDropDownModel lOC_CountryDropDownModel = new LOC_CountryDropDownModel();
                    {
                        lOC_CountryDropDownModel.CountryID = Convert.ToInt32(sqlDataReader["CountryID"]);
                        lOC_CountryDropDownModel.CountryName = sqlDataReader["CountryName"].ToString();
                    };
                    lOC_CountryDropDowns.Add(lOC_CountryDropDownModel);
                }
                sqlDataReader.Close();
            }
            sqlConnection.Close();
            ViewBag.CountryList = lOC_CountryDropDowns;
        }
        public IActionResult Statesearch(string StateName = "", string StateCode = "", int CountryID = -1)
        {
            FillDropDownMenu();
            string connectionstring = this.Configuration.GetConnectionString("MyConnection");

            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionstring);
            conn.Open();
            SqlCommand objcmd = conn.CreateCommand();
            objcmd.CommandType = CommandType.StoredProcedure;
            objcmd.CommandText = "PR_StateSearch";
            if (StateName != "") objcmd.Parameters.Add("@StateName", SqlDbType.NVarChar).Value = StateName;
            if (StateCode != "") objcmd.Parameters.Add("@StateCode", SqlDbType.NVarChar).Value = StateCode;
            if (CountryID != -1) objcmd.Parameters.Add("@CountryID", SqlDbType.NVarChar).Value = CountryID;
            SqlDataReader reader = objcmd.ExecuteReader();
            dt.Load(reader);
            conn.Close();
            return View("StateList", dt);
        }

    }
}
