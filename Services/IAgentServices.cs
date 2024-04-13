using mobilewakala.Models;

namespace mobilewakala.Services
{
    public interface IAgentServices
    {
        Task<bool> ChangeStatus(AgentStatusModel NewStatus);
        Task<bool> RegisterAgent(AgentDataModel agent);
        Task<bool> LoginService(LoginModel loggedUser);
        Task<List<OpenTicketModel>> GetOPenTicket(string agentCode);
        Task<string> checkAgentStatus(string agentCode);
        Task<bool> AttendCustTicket(OpenTicketModel ticketDetails);
        Task<List<OpenTicketModel>> GetTicketHistory(string agentCode);
        Task<bool> DeliveryReport(NotificationMessages MessageData);
        Task<bool> SyncLoaction(AgentLocationSync currentLocation);
    }
}