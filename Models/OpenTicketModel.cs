using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mobilewakala.Models
{
    public class OpenTicketModel
    {
        public string? phoneNumber { get; set; }
        public string? description { get; set; }
        public string network { get; set; }
        public string serviceRequested { get; set; }
        public double customerLatitude { get; set; }
        public double customerLongitude { get; set; }
        public string agentCode { get; set; }
        public double? agentLongitude { get; set; }
        public double? agentLatitude { get; set; }
        public string ticketStatus { get; set; }
        public DateTime ticketCreationDateTime { get; set; }



    } 
    public class OTApiResponse
        {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public int TotalCount { get; set; }
        public List<OpenTicketModel> DataList { get; set; }
        public string agentStatus { get; set; }
    }
}
