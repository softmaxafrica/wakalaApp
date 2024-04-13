using mobilewakala.Models;
using mobilewakala.Services;
using mobilewakala.Shared;
using Newtonsoft.Json;

namespace mobilewakala.Customer;

public partial class CustomerLogin : ContentPage
{
   
        private readonly ICustomerServices _customerServices;
        public List<AgentDataModel> _AgentLists;
        public CustomerLoginModel? loggedInCustomer;
        public CustomerModel customerModel = new CustomerModel();
        string custCode;
        public CustomerLogin()
        {
            InitializeComponent();
            ICustomerServices customerServices = new CustomerServices();
            _customerServices = customerServices;

        }


        #region Check Connection
        public async Task<bool> IsServerReachable()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(Constants.BaseAddress);
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
            custCode = CustomerId.Text;
            if ((string.IsNullOrEmpty(custCode) || (custCode == "__________")))
            {
                DisplayAlert("Error", "Tafadhali Jaza Namba ya Simu Yako au Ya Mtu wa Karibu ili kuendelea ", "Funga");
            }
            else
            {
                SecureStorage.Default.RemoveAll();
                customerModel.customerId = custCode;
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await DisplayAlert("Error", "Tafadhali Unganisha Simu Yako Kwenye Mtandao", "Funga");
                    return;
                }
                var apiResponse = await _customerServices.LoginService(customerModel);
                if (apiResponse)
                {
                    var userJson = JsonConvert.SerializeObject(customerModel);
                    SecureStorage.Default.SetAsync("LoggedInUser", userJson);
                    LoggedCustomer.loggedCust.CustomerId = custCode;

                    loggedInCustomer = JsonConvert.DeserializeObject<CustomerLoginModel>(userJson);
                     if (LoggedCustomer.loggedCust.CustomerId != null)
                    {
                        //Navigation.PushAsync(new CustomerHome());
                        Application.Current.MainPage = new NavigationPage(new CustomerHome());

                }

            }
                else
                {
                    var registrationResponse = await _customerServices.RegisterCustomer(customerModel);
                    if (registrationResponse)
                    {
                        bool isLogged = await _customerServices.LoginService(customerModel);
                        if (isLogged)
                        {
                            Loaded += OnLoginClicked;
                        }
                    }
                }


            }
        }

        #endregion

    
}