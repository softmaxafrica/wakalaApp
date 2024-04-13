using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using mobilewakala.Models;
using mobilewakala.Services;
using mobilewakala.Shared;
 
namespace mobilewakala.Customer
{
    public partial class SelectAgent : ContentPage, INotifyPropertyChanged
    {
        private readonly ICustomerServices _customerServices;
        public List<AgentDataModel> _agentListLoaded;
        private List<string> messagesList = new List<string>();

        private HubConnection _hubConnection;
        private string HubUrl = Constants.BaseAddress + "/signalHub";



        public double cLatitude;
        public double cLongitude;
        LiveLocationService livelocation = new LiveLocationService();
        private Command tapCommand;

        public Command TapCommand
        {
            get { return tapCommand; }
            protected set { tapCommand = value; }
        }

        public SelectAgent()
        {
            InitializeComponent();
            _customerServices = new CustomerServices();
            _agentListLoaded = new List<AgentDataModel>();


            // Call LoadOnlineAgents() and bind the result to ActiveAgents.ItemsSource
            GetAgents();
        }

        private async Task GetAgents()
        {
           await LoadOnlineAgentsAsync();
        }
        #region SignalRhubConn
        private async Task InitializeSignalR()
        {
           // Loaded += MainPage_Loaded;
            try
            {
                _hubConnection = new HubConnectionBuilder()
                    .WithAutomaticReconnect()
                    .WithUrl(HubUrl)
                    .Build();

                //_hubConnection.Closed += async (error) =>
                //{
                //    await Task.Delay(new Random().Next(0, 5) * 1000);
                //    await _hubConnection.StartAsync();
                //};

            }
            catch (Exception ex)
            {
                // Handle exceptions
            }
        }

        #endregion

        #region Load Online Agents to collection
        public async Task LoadOnlineAgentsAsync()
        {
            Location currentLocation = await livelocation.GetCurrentLocation();
            double latitude = currentLocation.Latitude;
            double longitude = currentLocation.Longitude;
            cLatitude = latitude;
            cLongitude = longitude;

            _agentListLoaded = await _customerServices.GetOnlineAgents(PreparedCustomerTicket.Instance.network, PreparedCustomerTicket.Instance.serviceRequested);

            foreach (var agent in _agentListLoaded)
            {
                double distance = CalculateDistance(latitude, longitude, agent.latitude.Value, agent.longitude.Value);
                agent.DistanceToCustomer = Math.Round(distance, 2);
            }
            // Bind the list of agents to ActiveAgents.ItemsSource
            ActiveAgents.ItemsSource = _agentListLoaded;
        }
        #endregion

        #region Calculate Distance From Agent To Customer in Km
        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Radius of the earth in km
            double dLat = Deg2Rad(lat2 - lat1); // deg2rad below
            double dLon = Deg2Rad(lon2 - lon1);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(Deg2Rad(lat1)) * Math.Cos(Deg2Rad(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = R * c; // Distance in km
            return d;
        }

        private double Deg2Rad(double deg)
        {
            return deg * (Math.PI / 180);
        }
        #endregion


        #region connect Customer To Agent And Show the Map
        #endregion


        private void Frame_Tapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is AgentDataModel selectedAgent)
            {
                // SendAgentToMap(selectedAgent);

                PreparedCustomerTicket.Instance.custLatitude = cLatitude;
                PreparedCustomerTicket.Instance.custLongitude = cLongitude;
                PreparedCustomerTicket.Instance.agentCode = selectedAgent.agentCode;
                PreparedCustomerTicket.Instance.agentLatitude = selectedAgent.latitude;
                PreparedCustomerTicket.Instance.agentLongitude = selectedAgent.longitude;

                PreparedCustomerTicket.Instance.description = PreparedCustomerTicket.Instance.description.ToString();


                if (PreparedCustomerTicket.Instance.description == null)
                {
                    PreparedCustomerTicket.Instance.description = "Hakuna Maelezo yaliyotolewa na mteja";
                }
                else
                {
                    PreparedCustomerTicket.Instance.description = PreparedCustomerTicket.Instance.description.ToString();
                }

                if (PreparedCustomerTicket.Instance.phoneNumber == null)
                {
                    PreparedCustomerTicket.Instance.phoneNumber = "Hakuna Namba Ya mteja";
                }
                else
                {
                    PreparedCustomerTicket.Instance.phoneNumber = PreparedCustomerTicket.Instance.phoneNumber.ToString();
                }
                CreateTicketMethod();

            }

        }
        private async Task CreateTicketMethod()
        {
            NotificationMessages notificationMessages = new NotificationMessages();
            notificationMessages.message = "Maombi Ya :" + PreparedCustomerTicket.Instance.serviceRequested.ToString() + "Mtandao :" + PreparedCustomerTicket.Instance.network.ToString();
            notificationMessages.status = "PENDING";
            notificationMessages.senderIdentity = PreparedCustomerTicket.Instance.phoneNumber;
            notificationMessages.receiverIdentity = PreparedCustomerTicket.Instance.agentCode;

            InitializeSignalR();

            string ConnectionId = await _customerServices.SendMessageToClient(notificationMessages.senderIdentity, notificationMessages.receiverIdentity, notificationMessages.message);
            if (ConnectionId != null)
            {
                notificationMessages.connectionId = ConnectionId;
                bool response = await _customerServices.SendRequestMessage(notificationMessages);

                DisplayAlert(Title, response.ToString(), "Sawa");
            }

            bool respondedAgent = await _customerServices.CreateCustomerTicket(PreparedCustomerTicket.Instance);

            if (respondedAgent)
            {
                DisplayAlert("Ticket Info", "Hongera !! \n Maombi Yako Yamepokelewa", "Funga");
            }
            else
            {
                DisplayAlert("Ticket Info", " Maombi Yako Hayajafanikiwa!! \n Tafadhali Jaribu Tena", "Funga");
            }
        }

        #region SendTOOneClient

        private async Task SendToClient(string ConnId, string incommingAgentCallmessage)
        {

            await _hubConnection.StartAsync();
            await _hubConnection.SendAsync("CallForAgent", ConnId, incommingAgentCallmessage);


        }
        #endregion
    }
}
