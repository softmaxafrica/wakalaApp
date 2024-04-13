using Newtonsoft.Json;
using mobilewakala.Models;
using Syncfusion.Maui.Buttons;

namespace mobilewakala.Customer;

public partial class CustomerHome : ContentPage
{
    private LoginModel? loggedInUser;
    private CustomerLoginModel? loggedCust;

    public CustomerHome()
    {
        InitializeComponent();
        Login();
        loginSwitch.IsOn = true;    
    }
    private async void Login()
    {
        await logginPref();
    }
    async Task logginPref()
    {

       

        string storedUserJson = await SecureStorage.Default.GetAsync("LoggedInUser");
        if ((storedUserJson != null) || (LoggedCustomer.loggedCust.CustomerId != null))
        {
            if (storedUserJson != null)
            {
                loggedCust = JsonConvert.DeserializeObject<CustomerLoginModel>(storedUserJson);
            }

        }
        else
        {
            await Navigation.PushAsync(new MainPage());
        }

    }


    private void NewRequest(object sender, EventArgs e)
    {
        Navigation.PushAsync(new CreateNewTicket());
        //Application.Current.MainPage = new NavigationPage(new CreateNewTicket());

    }
    private void MyHistory(object sender, EventArgs e)
    {
        Navigation.PushAsync(new TransactionsHistory());
        //Application.Current.MainPage = new NavigationPage(new TransactionsHistory());

    }



    #region logout
    private async void LoginSfSwitch_StateChanged(object sender, SwitchStateChangedEventArgs e)
    {
        bool? newV = e.NewValue;
        bool? oldV = e.OldValue;

        if (newV != oldV)
        {
            if (loginSwitch.IsOn == false)
            {
                bool logOutAlert = await DisplayAlert("Wakala Plus", "Je, Unataka kuondoka Kwenye Mfumo Kwa Sasa?", "Ndio", "Hapana");
                if (logOutAlert)
                {
                    SecureStorage.RemoveAll();
                    LoggedCustomer.loggedCust.CustomerId = null;
                    LoggedData.LoggedUser.agentCode = null;
                    LoggedData.LoggedUser.password = null;

                    //var stack = Shell.Current.Navigation.NavigationStack.ToArray();
                    //for (int i = stack.Length - 1; i > 0; i--)
                    //{
                    //    Shell.Current.Navigation.RemovePage(stack[i]);
                    //}
                    //Application.Current.Quit();

                    //Task.Delay(1000).ContinueWith(_ =>
                    //{
                    //     Device.BeginInvokeOnMainThread(() =>
                    //    {
                    //        Application.Current.MainPage = new NavigationPage(new MainPage());
                    //    });
                    //});

                    Application.Current.MainPage = new NavigationPage(new MainPage());

                }
                else
                {
                    loginSwitch.IsOn = true;
                }
            }
        }
    }
    #endregion

}