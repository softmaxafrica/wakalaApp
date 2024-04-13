using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mobilewakala.Models
{
    public class AgentDataModel
    {
        public string agentCode { get; set; }
        public string password { get; set; }

        public string nida { get; set; }

        public string agentFullName { get; set; }

        public string agentPhone { get; set; }
        public string networksOperating { get; set; }
        public string serviceGroupCode { get; set; }
        public string? regstrationStatus { get; set; }
        public string? address { get; set; }
        public double? longitude { get; set; }
        public double? latitude { get; set; }

        public double? DistanceToCustomer { get; set; }
    }

    public class ApiResponse
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public int TotalCount { get; set; }
        public object Data { get; set; } // You can use specific type if needed
        public List<AgentDataModel> DataList { get; set; }
    }

}
