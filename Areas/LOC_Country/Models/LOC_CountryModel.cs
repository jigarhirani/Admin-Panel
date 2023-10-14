using System.ComponentModel.DataAnnotations;

namespace Admin_Panel.Areas.LOC_Country.Models
{
    public class LOC_CountryModel
    {
        public int? CountryID { get; set; }

        [Required(ErrorMessage = "Name is Requried")]
        public string? CountryName { get; set; }

        [Required(ErrorMessage = "Code is Requried")]
        public string? CountryCode { get; set; }

    }
    public class LOC_CountryDropDownModel
    {

        public int CountryID { get; set; }

        public String? CountryName { get; set; }
    }
}
