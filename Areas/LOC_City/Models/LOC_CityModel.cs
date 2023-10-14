using System.ComponentModel.DataAnnotations;

namespace Admin_Panel.Areas.LOC_City.Models
{
    public class LOC_CityModel
    {
        public int? CityID { get; set; }

        [Required(ErrorMessage = "Please Enter City Name")]
        public string? CityName { get; set; }

        [Required(ErrorMessage = "Please Enter City Code")]

        public string? CityCode { get; set; }

        [Required(ErrorMessage = "Please State Select")]
        public int? StateID { get; set; }

        [Required(ErrorMessage = "Please Country Select")]
        public int? CountryID { get; set; }


    }
    public class LOC_CityDropDownModel
    {

        public int CityID { get; set; }
        public String CityName { get; set; }

    }
}
