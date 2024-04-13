using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mobilewakala.Models;

namespace mobilewakala.Services
{
    public  interface ICustomerServices
    {

        Task<List<AgentDataModel>> GetOnlineAgents(string network,string serviceRequested);
        Task<string> PushMessage(string message);
        Task <string> ConfigureDeviceIdInDatabase(string deviceId, string userId);
        Task<string> SendMessageToClient(string sender, string Identity, string message);
        Task<bool> CreateCustomerTicket(PreparedCustomerTicket NewTicket);
        Task<string> CallForAgent(string code, string newMessage);
        Task<bool> SendRequestMessage(NotificationMessages MessageData);
        Task<bool> LoginService(CustomerModel customer);
        Task<bool> RegisterCustomer(CustomerModel CustomerId);
        Task<List<OpenTicketModel>> GetTransactionsHistory(string customerId);
    }
}
