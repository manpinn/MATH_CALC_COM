using MATH_CALC_COM.Services.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MATH_CALC_COM.Models
{
    [Table("request_data")]
    public class RequestData
    {
        [Key]
        public int id {  get; set; }

        public DateTime date_created { get; set; }
        
        public string url { get; set; }
        
        public string ip_address { get; set; }
    }
}
