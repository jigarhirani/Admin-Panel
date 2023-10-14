using Admin_Panel.Areas.MST_Branch.Models;
using Admin_Panel.Areas.MST_Student.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Admin_Panel.Areas.LOC_City.Models;

namespace Admin_Panel.Areas.MST_Student.Controllers
{
    [Area("MST_Student")]
    [Route("MST_Student/{Controller}/{Action}")]
    public class MST_StudentController : Controller
    {

        public IConfiguration Configuration;
        public MST_StudentController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #region STUDENT LIST
        public IActionResult StudentList()
        {
            FillCity_DropDownMenu();
            FillBranch_DropDownMenu();
            string connectionString = this.Configuration.GetConnectionString("MyConnection");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = "SelectAll_Student";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = sqlCommand.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            sqlConnection.Close();
            return View(dataTable);
        }
        #endregion

        #region ADDEDIT
        public IActionResult StudentAddEdit(int? StudentID)
        {
            FillCity_DropDownMenu();
            FillBranch_DropDownMenu();
            if (StudentID != null)
            {
                FillCity_DropDownMenu();
                FillBranch_DropDownMenu();
                string connectionstr = this.Configuration.GetConnectionString("MyConnection");
                DataTable dt = new DataTable();
                SqlConnection sqlconnection = new SqlConnection(connectionstr);
                sqlconnection.Open();
                SqlCommand objcmd = sqlconnection.CreateCommand();
                objcmd.CommandType = CommandType.StoredProcedure;
                objcmd.CommandText = "SelectByPk_Student";
                objcmd.Parameters.AddWithValue("StudentID", StudentID);
                SqlDataReader sqldatareader = objcmd.ExecuteReader();
                dt.Load(sqldatareader);
                MST_StudentModel model = new MST_StudentModel();
                foreach (DataRow dr in dt.Rows)
                {
                    model.StudentID = Convert.ToInt32(dr["StudentID"]);
                    model.BranchID = Convert.ToInt32(dr["BranchID"]);
                    model.CityID = Convert.ToInt32(dr["CityID"]);
                    model.Gender = dr["Gender"].ToString();
                    model.StudentName = dr["StudentName"].ToString();
                    model.Email = dr["Email"].ToString();
                    model.MobileNoStudent = Convert.ToString(dr["MobileNoStudent"]);
                    model.MobileNoFather = Convert.ToString(dr["MobileNoFather"]);
                    model.Address = dr["Address"].ToString();
                    model.BirthDate = Convert.ToDateTime(dr["BirthDate"]);
                    model.Age = Convert.ToInt32(dr["Age"]);
                    model.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    model.Password = dr["Password"].ToString();
                }
                return View(model);
            }
            return View();
        }
        #endregion

        #region ADDEDIT METHOD
        public IActionResult AddEditMethod(MST_StudentModel model)
        {

            string connectionstr = this.Configuration.GetConnectionString("MyConnection");
            DataTable dt = new DataTable();
            SqlConnection sqlconnection = new SqlConnection(connectionstr);
            sqlconnection.Open();
            SqlCommand objcmd = sqlconnection.CreateCommand();
            objcmd.CommandType = CommandType.StoredProcedure;
            if (model.StudentID == null)
            {
                objcmd.CommandText = "Insert_Student";
                objcmd.Parameters.AddWithValue("StudentName", model.StudentName);
                objcmd.Parameters.AddWithValue("CityID", model.CityID);
                objcmd.Parameters.AddWithValue("BranchID", model.BranchID);
                objcmd.Parameters.AddWithValue("MobileNoStudent", model.MobileNoStudent);
                objcmd.Parameters.AddWithValue("Email", model.Email);
                objcmd.Parameters.AddWithValue("MobileNoFather", model.MobileNoFather);
                objcmd.Parameters.AddWithValue("Address", model.Address);
                objcmd.Parameters.AddWithValue("BirthDate", model.BirthDate);
                objcmd.Parameters.AddWithValue("Age", model.Age);
                objcmd.Parameters.AddWithValue("IsActive", model.IsActive);
                objcmd.Parameters.AddWithValue("Gender", model.Gender);
                objcmd.Parameters.AddWithValue("Password", model.Password);
                objcmd.ExecuteNonQuery();
            }
            else
            {
                objcmd.CommandText = "Update_Student";
                objcmd.Parameters.AddWithValue("StudentID", model.StudentID);
                objcmd.Parameters.AddWithValue("CityID", model.CityID);
                objcmd.Parameters.AddWithValue("BranchID", model.BranchID);
                objcmd.Parameters.AddWithValue("StudentName", model.StudentName);
                objcmd.Parameters.AddWithValue("MobileNoStudent", model.MobileNoStudent);
                objcmd.Parameters.AddWithValue("Email", model.Email);
                objcmd.Parameters.AddWithValue("MobileNoFather", model.MobileNoFather);
                objcmd.Parameters.AddWithValue("Address", model.Address);
                objcmd.Parameters.AddWithValue("BirthDate", model.BirthDate);
                objcmd.Parameters.AddWithValue("Age", model.Age);
                objcmd.Parameters.AddWithValue("IsActive", model.IsActive);
                objcmd.Parameters.AddWithValue("Gender", model.Gender);
                objcmd.Parameters.AddWithValue("Password", model.Password);
                objcmd.ExecuteNonQuery();
            }
            return RedirectToAction("StudentList");
        }
        #endregion

        #region DELETE STUDENT
        public IActionResult DeleteStudent(int StudentID)
        {
            string connectionString = this.Configuration.GetConnectionString("MyConnection");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = "Delete_Student";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@StudentID", StudentID);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return RedirectToAction("StudentList");
        }
        #endregion

        #region CITY DROPDOWN
        public void FillCity_DropDownMenu()
        {
            String ConnectionString = this.Configuration.GetConnectionString("MyConnection");
            List<LOC_CityDropDownModel> lOC_CityDropDowns = new List<LOC_CityDropDownModel>();
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "CityDropDownList";
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    LOC_CityDropDownModel loc_CityDropDownModel = new LOC_CityDropDownModel();
                    {
                        loc_CityDropDownModel.CityID = Convert.ToInt32(sqlDataReader["CityID"]);
                        loc_CityDropDownModel.CityName = sqlDataReader["CityName"].ToString();
                    };
                    lOC_CityDropDowns.Add(loc_CityDropDownModel);
                }
                sqlDataReader.Close();
            }
            sqlConnection.Close();
            ViewBag.CityList = lOC_CityDropDowns;
        }
        #endregion

        #region BRANCH DROPDOWN
        public void FillBranch_DropDownMenu()
        {
            String ConnectionString = this.Configuration.GetConnectionString("MyConnection");
            List<MST_BranchDropDownModel> mst_BranchDropDowns = new List<MST_BranchDropDownModel>();
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "BranchDropDownList";
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.HasRows)
            {
                while (sqlDataReader.Read())
                {
                    MST_BranchDropDownModel mst_BranchDropModel = new MST_BranchDropDownModel();
                    {
                        mst_BranchDropModel.BranchID = Convert.ToInt32(sqlDataReader["BranchID"]);
                        mst_BranchDropModel.BranchName = sqlDataReader["BranchName"].ToString();
                    };
                    mst_BranchDropDowns.Add(mst_BranchDropModel);
                }
                sqlDataReader.Close();
            }
            sqlConnection.Close();
            ViewBag.BranchList = mst_BranchDropDowns;
        }
        #endregion

        #region STUDENT SEARCH
        public IActionResult StudentSearch(string studentName = "", int BranchId = -1, int CityId = -1, string Email = "", string Gender = "")
        {
            FillCity_DropDownMenu();
            FillBranch_DropDownMenu();
            string connectionstring = this.Configuration.GetConnectionString("MyConnection");

            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connectionstring);
            conn.Open();
            SqlCommand objcmd = conn.CreateCommand();
            objcmd.CommandType = CommandType.StoredProcedure;
            objcmd.CommandText = "PR_StudentSearch";
            if (studentName != "") objcmd.Parameters.Add("@StudentName", SqlDbType.NVarChar).Value = studentName;
            if (BranchId != -1) objcmd.Parameters.Add("@BranchId", SqlDbType.NVarChar).Value = BranchId;
            if (CityId != -1) objcmd.Parameters.Add("@CityId", SqlDbType.NVarChar).Value = CityId;
            if (Email != "") objcmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = Email;
            if (Gender != "") objcmd.Parameters.Add("@Gender", SqlDbType.NVarChar).Value = Gender;
            SqlDataReader reader = objcmd.ExecuteReader();
            dt.Load(reader);
            conn.Close();

            return View("StudentList", dt);
        }
        #endregion
    }
}
