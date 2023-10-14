using System.ComponentModel.DataAnnotations;

namespace Admin_Panel.Areas.MST_Branch.Models
{
    public class MST_BranchModel
    {
        public int? BranchID { get; set; }

        [Required(ErrorMessage = "Name is Requried")]
        public string BranchName { get; set; }

        [Required(ErrorMessage = "Code is Requried")]
        public string BranchCode { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

    }

    public class MST_BranchDropDownModel
    {

        public int BranchID { get; set; }
        public String? BranchName { get; set; }

    }
}
