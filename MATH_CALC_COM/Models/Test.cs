using System.ComponentModel.DataAnnotations;

namespace MATH_CALC_COM.Models
{
    public class Test
    {
        public string? Teststring { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime Testdate { get; set; }
    }
}
