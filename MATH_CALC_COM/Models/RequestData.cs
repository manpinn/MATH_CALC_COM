using MATH_CALC_COM.Services.Enums;

namespace MATH_CALC_COM.Models
{
    public class RequestData
    {
        public int id {  get; set; }

        public DateTime datetime { get; set; }
        
        public string url { get; set; }
        
        public string ip_adress { get; set; }

        public InternetProtocolType ip_type { get; set; }
    }
}
