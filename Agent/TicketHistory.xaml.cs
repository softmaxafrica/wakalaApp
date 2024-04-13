using mobilewakala.Models;
using mobilewakala.Services;

namespace mobilewakala.Agent;

public partial class TicketHistory : ContentPage
{
    private readonly IAgentServices _AgentServices;
    public OpenTicketModel mapData;
    private List<OpenTicketModel> TicketListLoaded;

    public TicketHistory()
    {
        InitializeComponent();
        TicketListLoaded = new List<OpenTicketModel>();
        _AgentServices = new AgentServices();

        LoadTicketHistory();
    }


    #region Load OpenTicket to collection

    private async void LoadTicketHistory()
    {
        try
        {
            if (LoggedData.LoggedUser == null)
            {
                Console.WriteLine("Logged user is null.");
                return;
            }

            string agentCode = LoggedData.LoggedUser.agentCode.ToString();

            if (_AgentServices == null)
            {
                Console.WriteLine("_AgentServices is null.");
                return;
            }

            var openTickets = await _AgentServices.GetTicketHistory(agentCode);

            if (openTickets == null)
            {
                Console.WriteLine("Open tickets collection is null.");
                return;
            }

            TicketListLoaded.Clear();

            foreach (var ticket in openTickets)
            {
                TicketListLoaded.Add(ticket);
            }
            TicketHistoryListLoaded.ItemsSource = TicketListLoaded;

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading open tickets: {ex.Message}");
        }
    }
    #endregion

}

