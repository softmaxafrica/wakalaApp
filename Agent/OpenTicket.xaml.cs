 using Syncfusion.Maui.Popup;
using System.Diagnostics;
using mobilewakala.Models;
using mobilewakala.Services;

namespace mobilewakala.Agent;

public partial class OpenTicket : ContentPage
{

    LiveLocationService livelocation = new LiveLocationService();
    public double cLatitude;
    public double cLongitude;
    private readonly IAgentServices _AgentServices;
    public OpenTicketModel mapData;
    private List<OpenTicketModel> TicketListLoaded;


    public OpenTicket()
    {
        InitializeComponent();

        TicketListLoaded = new List<OpenTicketModel>();
        _AgentServices = new AgentServices();

        LoadOpenTickets();

    }

    #region Load OpenTicket to collection

    public async void LoadOpenTickets()
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

            var openTickets = await _AgentServices.GetOPenTicket(agentCode);

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
            TicketList.ItemsSource = TicketListLoaded;

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading open tickets: {ex.Message}");
        }
    }
    #endregion


    private void Frame_Tapped(object sender, EventArgs e)
    {
        mapData = new OpenTicketModel();
        // Check if an item is selected
        if (sender is Frame frame && frame.BindingContext is OpenTicketModel selectedTicket)
        {
            // Access the selected ticket details
            string phoneNumber = selectedTicket.phoneNumber;
            string network = selectedTicket.network;
            string serviceRequested = selectedTicket.serviceRequested;
            string description = selectedTicket.description;
            string ticketStatus = selectedTicket.ticketStatus;
            mapData.ticketCreationDateTime = selectedTicket.ticketCreationDateTime;

            mapData.customerLatitude = selectedTicket.customerLatitude;
            mapData.customerLongitude = selectedTicket.customerLongitude;
            mapData.phoneNumber = phoneNumber;
            mapData.network = network;
            mapData.serviceRequested = serviceRequested;
            mapData.description = description;
            mapData.ticketStatus = "IPO KWENYE MACHAKATO";
            mapData.agentCode = selectedTicket.agentCode;
            respondToTicket(mapData);
        }

    }
    private async void respondToTicket(OpenTicketModel mapData)
    {

        bool answer = await DisplayAlert("Maombi Ya Huduma", "Je, Upo Tayari Kutoa Huduma Hii Kwa Sasa", "Ndio", "Hapana");

        if (answer)
        {
            Location currentLocation = await livelocation.GetCurrentLocation();

            mapData.agentLatitude = currentLocation.Latitude;
            mapData.agentLongitude = currentLocation.Longitude;

            Navigation.PushAsync(new AgentCustMap(mapData));
        }
        else
        {
            if (answer == false)
            {
                DisplayAlert("Wakala +", "Umefanikiwa Kusitisha Huduma \n Tafadhali Kubali Maombi haya Kabla hajapokea Wakala Mwengine.", "Funga");
            }

        }
    }

}