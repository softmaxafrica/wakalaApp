 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace mobilewakala.Models
{
    public class PreparedCustomerTicket
    {
        
        public string? phoneNumber { get; set; }
         public string? description { get; set; }
         public string network { get; set; }
        public string serviceRequested { get; set; }
         public double  custLatitude { get; set; }
         public double custLongitude { get; set; }
         public string agentCode { get; set; } 
         public double? agentLongitude { get; set; }  
        public double? agentLatitude { get; set; }


        #region Singleton pattern
        private static PreparedCustomerTicket _instance;

        public static PreparedCustomerTicket Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PreparedCustomerTicket();
                }
                return _instance;
            }
        }

        private PreparedCustomerTicket() { }

        #endregion

    }




    public class TApiResponse
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public int TotalCount { get; set; }
        public object Data { get; set; } // You can use specific type if needed
        public List<PreparedCustomerTicket> DataList { get; set; }
    }
}
