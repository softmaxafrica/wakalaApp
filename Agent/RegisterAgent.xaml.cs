
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using mobilewakala.Shared;
using mobilewakala.Models;
 
using System.Diagnostics;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using mobilewakala.Services;

namespace mobilewakala.Agent;


    public partial class RegisterAgent : ContentPage
    {
        public static string baseAddress = Constants.BaseAddress;

        public readonly IAgentServices _agentServices;
        LiveLocationService livelocation = new();
//    LiveLocationService livelocation = new LiveLocationService();

    public string networkCode;
        public string serviceCode;

        #region Constructor

        public RegisterAgent()
        {
            InitializeComponent();
            IAgentServices agentServices = new AgentServices();
            _agentServices = agentServices;
        }

        #endregion
        #region ServiceCode And Network Code
        private void network_StateChanged(object sender, Syncfusion.Maui.Buttons.StateChangedEventArgs e)
        {
            UpdateSelectedNetworks();

        }

        private void services_StateChanged(object sender, Syncfusion.Maui.Buttons.StateChangedEventArgs e)
        {
            UpdateSelectedServices();

        }


        private void UpdateSelectedNetworks()
        {
            int selectedItemCode = 0;

            if (Airtel.IsChecked.Value)
                selectedItemCode += 10000;
            if (Halotel.IsChecked.Value)
                selectedItemCode += 1000;
            if (Tigo.IsChecked.Value)
                selectedItemCode += 100;
            if (Vodacom.IsChecked.Value)
                selectedItemCode += 10;
            if (Ttcl.IsChecked.Value)
                selectedItemCode += 1;

            SelectedItemsLabel.Text = $"Selected Item Code: {selectedItemCode:D5}";
            networkCode = selectedItemCode.ToString();
        }

        private void UpdateSelectedServices()
        {
            int selectedServiceCode = 0;



            if (SimCardServices.IsChecked.Value)
                selectedServiceCode += 10;
            if (MoneyTransactionServices.IsChecked.Value)
                selectedServiceCode += 1;

            serviceCode = selectedServiceCode.ToString();
            SelectedIServicesCode.Text = $"Selected Item Code: {selectedServiceCode:D2}";
        }

        #endregion


        //#region Send Agent Data To WebApi
        //private async void RegisterButton_Clicked(object sender, EventArgs e)
        //{
        //    var apiService = new ApiServices(BaseAddress);

        //     var agentModel = new AgentModel
        //    {
        //        agentFullName = agentFullName.Text,
        //        nida = nida.Text,
        //        networksOperating = networkCode,
        //        serviceGroupCode = serviceCode,
        //        password = password.Text,
        //        agentPhone = phone.Text,
        //        agentCode = agentCode.Text,
        //        address = address.Text
        //    };

        //    try
        //    {
        //        //Task response = SaveAgentAsync(agentModel);

        //        string json = JsonSerializer.Serialize<AgentModel>(agentModel, _serializerOptions);

        //        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        //        HttpResponseMessage response = await apiService.PostAsync(_baseAddress, content);


        //        //var response = await apiService.PostAsync(agentModel);



        //        var resultMessage = response?.ToString();         
        //        await DisplayAlert("API Response", resultMessage, "OK");
        //    }
        //    catch (Exception ex)
        //    {
        //         await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        //    }
        //}
        //#endregion
        #region Send Agent Data To WebApi
        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            Location currentLocation = await livelocation.GetCurrentLocation();
            if (currentLocation != null)
            {
                AgentDataModel agentModel = new AgentDataModel();
                double latitude = currentLocation.Latitude;
                double longitude = currentLocation.Longitude;
                agentModel.agentFullName = agentFullName.Text;
                agentModel.nida = nida.Text;
                agentModel.networksOperating = networkCode;
                agentModel.serviceGroupCode = serviceCode;
                agentModel.password = password.Text;
                agentModel.agentPhone = phone.Text;
                agentModel.agentCode = agentCode.Text;
                agentModel.address = address.Text;
                agentModel.regstrationStatus = "PE";
                agentModel.latitude = latitude;
                agentModel.longitude = longitude;
                try
                {
                    var apiResponse = await _agentServices.RegisterAgent(agentModel);
                    if (apiResponse)
                    {
                        var data = apiResponse.ToString();
                        DisplayAlert("TAARIFA YA USAJILI WA WAKALA", "Usajili Wako Umekamilika \n, Utapokea Ujumbe baada ya Uhakiki kukamilika", "Funga");
                    }
                    else
                    {
                        DisplayAlert("TAARIFA YA USAJILI WA WAKALA", "Usajili Wako Umeshindikana \n, Tafadhali Wasiliana na Huduma Kwa Wateja Kwa Msaada Zaidi.", "Funga");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"An error occurred: {ex.Message}\n \n StackTrace: {ex.StackTrace}", "OK");
                }
            }
            else
            {
                DisplayAlert("TAARIFA YA USAJILI WA WAKALA", "Usajili Wako Umeshindikana \n, Tafadhali Kamilisha Taarifa Za.", "Funga");
           }
        }
        #endregion
    }


