using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Syncfusion.Maui.Buttons;
using mobilewakala.Models;
using mobilewakala.Services;
using mobilewakala.Shared;

namespace mobilewakala.Agent;

public partial class Home : ContentPage
{
    private AgentStatusModel NewStatus;
    private AgentServices _AgentServices;

    private HubConnection? hubConnection;
    //    private List<string> stringList = new List<string>();
    private List<string> stringList = [];

    private string HubUrl = Constants.BaseAddress + "/signalHub";
    private LoginModel? loggedInUser;
    private CustomerLoginModel? loggedCust;
    private string agentCode;

    public Home()
    {
        InitializeComponent();
        NewStatus = new AgentStatusModel();
        _AgentServices = new AgentServices();
        checkAgentStatus();
        Login();
        

        //sinalRConnect();
    }
    private async void Login()
    {
        await logginPref();
    }
    async Task logginPref()
    {
        if ((LoggedData.LoggedUser.agentCode == null))
        {
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }
        else
        {

            loginSwitch.IsOn = true;
        }

 
    }

    private async Task checkAgentStatus()
    {
        if (LoggedData.LoggedUser.agentCode != null)
        {


            agentCode = LoggedData.LoggedUser.agentCode;
            string agentStat = await _AgentServices.checkAgentStatus(agentCode);
            if (agentStat == null)
            {
                DisplayAlert("Taarifa ", "Habari Wakala Status Haijapatikana..!!\n Tafadhali Washa Ili Kuruhusu Wateja Wapate Huduma Yako", "Sawa");
            }
            else
            {
                if (agentStat == "ONLINE")
                {
                    NewStatus.status = "ONLINE";
                    statuSwitch.IsOn = true;
                }
                else
                {
                    NewStatus.status = "OFFLINE";
                    statuSwitch.IsOn = false;
                }
            }
        }
        else
        {
            await Navigation.PushAsync(new MainPage());
        }
    }

    private async void sinalRConnect()
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl($"{HubUrl}?identity={LoggedData.LoggedUser.agentCode}")
            .WithAutomaticReconnect()
           .Build();
        hubConnection.On<string>("CallForAgent", (incomingMessage) =>
        {
            // stringList.Add(incomingMessage);
            // Device.BeginInvokeOnMainThread(() =>
            //{
            //    DisplayAlert("Maombi Ya Huduma", incomingMessage, "Sawa");
            //});
        });
        await ConnectToHub();
    }
    private async Task ConnectToHub()
    {
        try
        {
            await hubConnection.StartAsync();
            Console.WriteLine("Connected to SignalR Hub");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to SignalR Hub: {ex.Message}");
        }
    }
    private void MyOpenTicket(object sender, EventArgs e)
    {
        if (NewStatus.status == "ONLINE")
        {
            Navigation.PushAsync(new OpenTicket());
        }
        else
        {
            goToOnline();
        }
    }
    private async Task goToOnline()
    {
        bool golive = await DisplayAlert("Error", "Imeshindikana Kupokea Maombi Mapya \n Tafadhali Ingina (Mpangilio) kisha Washa (Badili Status) Yako kwa kuweka (ON) ili Kupata Maombi Yaliyokufikia", "Sawa", "Hapana Funga");
      
    }
    private void AllGroupOpenTicket(object sender, EventArgs e)
    {
        //Navigation.PushAsync(new AllGroupOpenTicket());
    }
    private void MyClosedTickets(object sender, EventArgs e)
    {
        Navigation.PushAsync(new TicketHistory());
    }
    private void Chatting(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Chat());
    }

    #region logout
    private async void LoginSfSwitch_StateChanged(object sender, SwitchStateChangedEventArgs e)
    {
        bool? newV = e.NewValue;
        bool? oldV = e.OldValue;

        if ((newV != oldV) && (loginSwitch.IsOn == false))
        {
            bool logOutAlert = await DisplayAlert("Wakala Plus", "Je, Unataka kuondoka Kwenye Mfumo Kwa Sasa?", "Ndio", "Hapana");
            if (logOutAlert)
            {
                SecureStorage.RemoveAll();
                LoggedCustomer.loggedCust.CustomerId = null;
                LoggedData.LoggedUser.agentCode = null;
                LoggedData.LoggedUser.password = null;

                // await Application.Current.MainPage.Navigation.PopToRootAsync();
                Application.Current.MainPage = new MainPage();

            }
            else
            {
                loginSwitch.IsOn = true;
            }
         
        }
    }
    #endregion
    #region Go Online /Offline
    private async void SfSwitch_StateChanged(object sender, SwitchStateChangedEventArgs e)
    {
        // Access the new and old values
        bool? newValue = e.NewValue;
        bool? oldValue = e.OldValue;

        if (newValue != oldValue)
        {
            if (newValue.Equals(true))
            {
                NewStatus.status = "ONLINE";
            }
            else
            {
                NewStatus.status = "OFFLINE";
            }
            NewStatus.agentCode = LoggedData.LoggedUser.agentCode;
            bool IsStatusChanged = await _AgentServices.ChangeStatus(NewStatus);
            if (IsStatusChanged)
            {
                DisplayAlert("Status yako Ni", NewStatus.status, "Sawa");
            }
            else
            {
                DisplayAlert("Imeshindikana....", "Imeshindikana Kubadili Hali (Status) Yako \n Tafadhali Jaribu Tena", "Sawa");
            }
        }
    }
    #endregion

}