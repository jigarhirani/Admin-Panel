using Admin_Panel.Areas.LOC_City.Models;
using Admin_Panel.Areas.LOC_Country.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Admin_Panel.Areas.LOC_State.Models;

namespace Admin_Panel.Areas.LOC_City.Controllers
{
    [Area("LOC_City")]
    [Route("LOC_City/[controller]/[action]")]
    public class LOC_CityController : Controller
    {
        public IConfiguration Configuration;
        public LOC_CityController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IActionResult CityList()
        {
            FillCountry_DropDownMenu();
            FillState_DropDownMenu();
            string connectionString = this.Configuration.GetConnectionString("MyConnection");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_City_SelectAll";
            SqlDataReader reader = sqlCommand.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            sqlConnection.Close();
            return View(dataTable);
        }

        public IActionResult CityAddEdit(int? CityID)
        {
            FillCountry_DropDownMenu();
            FillState_DropDownMenu();

            if (CityID != null)
            {
                string connectionString = this.Configuration.GetConnectionString("MyConnection");
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                DataTable dataTable = new DataTable();
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("CityID", CityID);
                sqlCommand.CommandText = "PR_City_SelectByPK";
                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                dataTable.Load(dataReader);
                LOC_CityModel model = new LOC_CityModel();
                foreach (DataRow data in dataTable.Rows)
                {
                    model.CountryID = int.Parse(data["CountryID"].ToString());
                    model.StateID = int.Parse(data["StateID"].ToString());
                    model.CityName = data["CityName"].ToString();
                    model.CityCode = data["CityCode"].ToString();
                }
                return View(model);
            }
            return View();
        }

        public IActionResult AddEditMethod(LOC_CityModel model)
        {
            string connectionstr = this.Configuration.GetConnectionString("MyConnection");
            DataTable dt = new DataTable();
            SqlConnection sqlConnection = new SqlConnection(connectionstr);
            sqlConnection.Open();
            SqlCommand ObjCmd = sqlConnection.CreateCommand();
            ObjCmd.CommandType = CommandType.StoredProcedure;
            if (model.CityID == null)
            {
                ObjCmd.CommandText = "PR_City_Insert";
                ObjCmd.Parameters.AddWithValue("@CountryID", model.CountryID);
                ObjCmd.Parameters.AddWithValue("@StateID", model.StateID);
                ObjCmd.Parameters.AddWithValue("@CityName", model.CityName);
                ObjCmd.Parameters.AddWithValue("@CityCode", model.CityCode);
                ObjCmd.ExecuteNonQuery();
            }
            else
            {
                ObjCmd.CommandText = "PR_City_UpdateByPK";
                ObjCmd.Parameters.AddWithValue("CityID", model.CityID);
                ObjCmd.Parameters.AddWithValue("CityName", model.CityName);
                ObjCmd.Parameters.AddWithValue("StateID", model.StateID);
                ObjCmd.Parameters.AddWithValue("CityCode", model.CityCode);
                ObjCmd.Parameters.AddWithValue("CountryID", model.CountryID);
                ObjCmd.ExecuteNonQuery();
            }
            return RedirectToAction("CityList");
        }


        public IActionResult DeleteCity(int CityID)
        {
            string connectionString = this.Configuration.GetConnectionString("MyConnection");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_City_DeleteByPK";
            sqlCommand.Parameters.AddWithValue("CityID", CityID);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return RedirectToAction("CityList");

        }

        public void FillCountry_DropDownMenu()
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

        public void FillState_DropDownMenu()
        {
            String ConnectionString = this.Configuration.GetConnectionString("MyConnection");
            List<LOC_StateDropDownModel> lOC_StateDropDowns = new List<LOC_StateDropDownModel>();
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "StateDropDownList";
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    LOC_StateDropDownModel loc_stateDropDownModel = new LOC_StateDropDownModel();
                    {
                        loc_stateDropDownModel.StateID = Convert.ToInt32(sqlDataReader["StateID"]);
                        loc_stateDropDownModel.StateName = sqlDataReader["StateName"].ToString();
                    };
                    lOC_StateDropDowns.Add(loc_stateDropDownModel);
                }
                sqlDataReader.Close();
            }
            sqlConnection.Close();
            ViewBag.StateList = lOC_StateDropDowns;
        }
        public IActionResult SearchMethod(String Keyword = "")
        {
            string connectionString = this.Configuration.GetConnectionString("MyConnection");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            DataTable dataTable = new DataTable();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "Country_Search";
            if (Keyword != "")
                sqlCommand.Parameters.AddWithValue("@CityData", Keyword);
            SqlDataReader reader = sqlCommand.ExecuteReader();
            dataTable.Load(reader);
            sqlConnection.Close();
            return View("CityList", dataTable);
        }
        public IActionResult SelectStateByCountry(int CountryID)
        {
            //Get Connection String 
            string str = this.Configuration.GetConnectionString("MyConnection");
            //Retrieve data in json from Database
            List<LOC_StateDropDownModel> loc_State = new List<LOC_StateDropDownModel>();
            SqlConnection conn1 = new SqlConnection(str);

            //open connection and create command object.
            conn1.Open();
            SqlCommand objCmd = conn1.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_State_SelectDropDownList";
            objCmd.Parameters.AddWithValue("@CountryID", CountryID);

            SqlDataReader objSDR = objCmd.ExecuteReader();
            //To check if any rows returns by datareader & binding these rows to model
            if (objSDR.HasRows)
            {
                while (objSDR.Read())
                {
                    LOC_StateDropDownModel vlst = new LOC_StateDropDownModel()
                    {
                        StateID = Convert.ToInt32(objSDR["StateID"]),
                        StateName = objSDR["StateName"].ToString()
                    };
                    loc_State.Add(vlst);
                }
                objSDR.Close();
            }
            conn1.Close();
            var vModel = loc_State;
            return Json(vModel);
        }


        public IActionResult DropdownByState(int StateID)
        {
            string str = this.Configuration.GetConnectionString("MyConnection");
            List<LOC_CityDropDownModel> loc_City = new List<LOC_CityDropDownModel>();
            SqlConnection conn1 = new SqlConnection(str);
            conn1.Open();
            SqlCommand objCmd = conn1.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_City_SelectDropDownList";
            objCmd.Parameters.AddWithValue("@StateID", StateID);
            SqlDataReader objSDR = objCmd.ExecuteReader();
            if (objSDR.HasRows)
            {
                while (objSDR.Read())
                {
                    LOC_CityDropDownModel vlst = new LOC_CityDropDownModel()
                    {
                        CityID = Convert.ToInt32(objSDR["CityID"]),
                        CityName = objSDR["CityName"].ToString()
                    };
                    loc_City.Add(vlst);
                }
                objSDR.Close();
            }
            conn1.Close();
            var vModel = loc_City;
            return Json(vModel);
        }
        public IActionResult CitySearch(string cityName = "", string cityCode = "", int StateId = -1, int CountryId = -1)
        {
            FillCountry_DropDownMenu();
            FillState_DropDownMenu();
            string connectionstring = this.Configuration.GetConnectionString("MyConnection");

            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionstring);
            conn.Open();
            SqlCommand objcmd = conn.CreateCommand();
            objcmd.CommandType = CommandType.StoredProcedure;
            objcmd.CommandText = "PR_CitySearch";
            if (cityName != "") objcmd.Parameters.Add("@cityName", SqlDbType.NVarChar).Value = cityName;
            if (cityCode != "") objcmd.Parameters.Add("@cityCode", SqlDbType.NVarChar).Value = cityCode;
            if (StateId != -1) objcmd.Parameters.Add("@StateId", SqlDbType.NVarChar).Value = StateId;
            if (CountryId != -1) objcmd.Parameters.Add("@CountryId", SqlDbType.NVarChar).Value = CountryId;
            SqlDataReader reader = objcmd.ExecuteReader();
            dt.Load(reader);
            conn.Close();

            return View("Citylist", dt);

        }
    }
}
