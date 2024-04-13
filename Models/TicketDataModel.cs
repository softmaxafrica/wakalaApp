using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace mobilewakala.Models
{
    public class TicketDataModel
    {
        [Display(Name = "Namba Ya Simu")]
        [Required(ErrorMessage = "Tafadhali Jaza Namba ya simu itakayotumika kuwasiliana na wakala")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, ErrorMessage = "Tafadhali ingiza namba ya simu katika muundo sahihi")]
        public string phoneNumber { get; set; }

        [Display(Name = "Maelezo Mafupi Kama Yapo")]
        public string description { get; set; }

        public enum NetworkType
        {
            [Display(Name = "AIRTEL")]
            AIRTEL,
            [Display(Name = "HALOTEL")]
            HALOTEL,
            [Display(Name = "TIGO")]
            TIGO,
            [Display(Name = "VODACOM")]
            VODACOM,
            [Display(Name = "TTCL")]
            TTCL
        }
        [Display(Name = "Chagua Mtandao")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Chagua Mtandao Unaohitaji Huduma")]
        public NetworkType network { get; set; }
        public enum ServiceType
        {
            [Display(Name = "Usajili Wa Laini")] USAJILI,            
            [Display(Name = "Miamala Ya Kifedha")]
            MIAMALA,       
        }
        [Display(Name = "Chagua Huduma")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Chagua Huduma Unayohitaji")]
        public ServiceType serviceRequested { get; set; }

     

    }
}
