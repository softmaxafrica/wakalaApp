namespace mobilewakala
{

    public partial class MainPage : ContentPage
    {
        // private LoginModel? loggedInUser;
        //private CustomerLoginModel? loggedInCustomer;

        public MainPage()
        {
            InitializeComponent();
            // this.Loaded += MainPage_Loaded;

            // Login();
        }
        //private async void Login()
        //{
        //    await loginPref();
        //}

        //        private void MainPage_Loaded(object sender, EventArgs e)
        //        {
        //#if ANDROID
        //        if (!Platforms.Android.AndroidServiceManager.IsRunning)
        //        {
        //            Platforms.Android.AndroidServiceManager.StartMyService();
        //         }
        //        else{
        //         }
        //#endif
        //        }


        //async Task loginPref()
        //{
        //    string storedUserJson = await SecureStorage.Default.GetAsync("LoggedInUser");
        //    if (storedUserJson != null)
        //    {
        //        loggedInUser = JsonConvert.DeserializeObject<LoginModel>(storedUserJson);
        //        loggedInCustomer = JsonConvert.DeserializeObject<CustomerLoginModel>(storedUserJson);

        //        if(loggedInCustomer.customerId != null)
        //        {
        //            await Navigation.PushAsync(new Ui.Customer.CustomerHome());
        //        }
        //        else
        //        {
        //            LoggedData.LoggedUser.agentCode = loggedInUser.agentCode;
        //            LoggedData.LoggedUser.password = loggedInUser.password;
        //           await  Navigation.PushAsync(new Ui.Agent.Home());
        //        }
        //    }
        //    //else
        //    //{
        //    //    //LoggedData.LoggedUser.agentCode = null;
        //    //    //LoggedData.LoggedUser.password = null;
        //    //    //LoggedCustomer.loggedCust.CustomerId = null;
        //    //}
        //}

        private void MtejaLog(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NavigationPage(new Customer.CustomerLogin()));
        }
        private void WakalaLog(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Agent.AgentLogin());
        }
    }

}
