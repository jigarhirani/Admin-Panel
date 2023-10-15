using System.ComponentModel.DataAnnotations;

namespace Admin_Panel.Areas.LOC_State.Models
{
    public class LOC_StateModel
    {
        public int? StateID { get; set; }

        [Required(ErrorMessage = "Please Enter State Name")]
        public string? StateName { get; set; }

        [Required(ErrorMessage = "Please Enter State Code")]
        public string? StateCode { get; set; }

        [Required(ErrorMessage = "Please Country Select")]
        public int? CountryID { get; set; }
    }

    public class LOC_StateDropDownModel
    {

        public int StateID { get; set; }
        public String? StateName { get; set; }

    }
}
