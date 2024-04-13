using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using mobilewakala.Models;
using mobilewakala.Services;
using mobilewakala.Shared;

namespace mobilewakala.Agent;
public partial class AgentLogin : ContentPage
{
    public readonly IAgentServices _agentServices;
    public LoginModel loggedInUser;
    private string HubUrl = Constants.BaseAddress + "/signalHub";
    private HubConnection? hubConnection;
    public AgentLogin()
    {
        InitializeComponent();
        Login();
        loggedInUser = new LoginModel();
        _agentServices = new AgentServices();
    }

    private   void Login()
    {
          logginPref();
    }

    async Task logginPref()
    {
        string storedUserJson = await SecureStorage.Default.GetAsync("LoggedInUser");
        if (storedUserJson != null)
        {
            loggedInUser = JsonConvert.DeserializeObject<LoginModel>(storedUserJson);
            LoggedData.LoggedUser.agentCode = loggedInUser.agentCode;
            LoggedData.LoggedUser.password = loggedInUser.password;
            if(LoggedData.LoggedUser.agentCode != null)
            {
                 Application.Current.MainPage = new NavigationPage(new Home());
            }
        }
    }

    private async void sinalRConnect()
    {
        hubConnection = new HubConnectionBuilder()
             .WithUrl($"{HubUrl}?identity={LoggedData.LoggedUser.agentCode}")
             .WithAutomaticReconnect()
            .Build();
        hubConnection.On<string>("CallForAgent", async (incomingMessage) =>
        {

        });

       await  ConnectToHub();
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

    #region Check Connection
    public async Task<bool> IsServerReachable()
    {
        try
        {
            using (var client = new HttpClient())
            {
                // Replace "your-server-url" with the actual URL of your server
                var response = await client.GetAsync(Constants.LocalAddress);
                return response.IsSuccessStatusCode;
            }
        }
        catch (Exception)
        {
            return false;
        }
    }
    #endregion
    #region Login Process
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        

        if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Error", "Tafadhali Unganisha Simu Yako Kwenye Mtandao", "Funga");
                return;
            }

            LoggedData.LoggedUser.agentCode = AgentCodeEntry.Text;
            LoggedData.LoggedUser.password = AgentPassword.Text;

            loggedInUser.agentCode = AgentCodeEntry.Text;
            loggedInUser.password = AgentPassword.Text;


            var apiResponse = await _agentServices.LoginService(loggedInUser);

            if (apiResponse)
            {
                //sinalRConnect();
                var userJson = JsonConvert.SerializeObject(loggedInUser);
                SecureStorage.Default.SetAsync("LoggedInUser", userJson);
            
            Application.Current.MainPage = new NavigationPage(new Home());
        }
        else
            {
                await DisplayAlert("Error", "Taarifa Sio Sahihi", "OK");
            }
    }

    #endregion
    #region GoTo AgentRegistration
    private void OnRegisterLinkTapped(object sender, EventArgs e)
    {
    Application.Current.MainPage = new NavigationPage(new RegisterAgent());
    }
    #endregion
 
}
