 using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using mobilewakala.Models;
using mobilewakala.Shared;
using WakalaPlus.Models;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace mobilewakala.Services
{
    public  class CustomerServices:ICustomerServices
    {
        public List<OpenTicketModel> TicketHistoryResponse { get; private set; }

        private string _baseUrl = Constants.BaseAddress;
        JsonSerializerOptions _serializerOptions;
        public List<AgentDataModel> agentListResponse { get; private set; }

        public CustomerServices()
        {
            _serializerOptions = new JsonSerializerOptions();
        }

        public JsonSerializerOptions Get_serializerOptions()
        {
            return _serializerOptions;
        }
        #region LoginRegister

        #region LoginService
        public async Task<bool> LoginService(CustomerModel customer)
        {
            var returnResponse = false;
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{_baseUrl}/api/Customer/Login";
                    var serializeContent = JsonConvert.SerializeObject(customer);
                    var apiResponse = await client.PostAsync(url, new StringContent(serializeContent, Encoding.UTF8, "application/json"));
                    if (apiResponse.IsSuccessStatusCode)
                    {
                        var response = await apiResponse.Content.ReadAsStringAsync();
                        var deserilizeResponse = JsonConvert.DeserializeObject<MainResponse>(response);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return returnResponse;
        }
        #endregion

        #region CustomerRegistration
        public async Task<bool> RegisterCustomer(CustomerModel CustomerData)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{_baseUrl}/api/Customer/RegisterCustomer";
                    var serializeContent = JsonConvert.SerializeObject(CustomerData);
                    var apiResponse = await client.PostAsync(url, new StringContent(serializeContent, Encoding.UTF8, "application/json"));

                    if (apiResponse.IsSuccessStatusCode)
                    {
                        var response = await apiResponse.Content.ReadAsStringAsync();
                        var deserializeResponse = JsonConvert.DeserializeObject<MainResponse>(response);
                        return deserializeResponse.IsSuccess;
                    }
                    else
                    {
                        // Log or handle the failure
                        // For now, returning false
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred: {ex}");
                // For now, returning false
                return false;
            }
        }
        #endregion


        #endregion

        #region UpdateDeviceIdInDatabase
        public async Task<string> ConfigureDeviceIdInDatabase(string deviceId, string userId)
        {
            string device;
            using (var client = new HttpClient())
            {
                string url = $"{Constants.BaseAddress}/api/DeviceId/ConfigureDeviceIdInDatabase/{deviceId}/{userId}";
                var apiResponse = await client.GetAsync(url);
                var responseContent = await apiResponse.Content.ReadAsStringAsync();
              
                var responseObject = JsonConvert.DeserializeObject<JObject>(responseContent);
                device = responseObject["data"]?.ToString();
                return device;
            }
        }


        #endregion


        #region SendNotificationToAgent
        public async Task<string> PushMessage(string message)
        {
            using (var client = new HttpClient())
            {
                string url = $"{Constants.BaseAddress}/api/Customer/PushMessage/{message}";

                var apiResponse = await client.GetAsync(url);

                if (apiResponse.IsSuccessStatusCode)
                {
                    return "Sent";
                }
                else
                {
                    // Optionally handle different failure scenarios
                    return "NotSent";
                }
            }
        }


        public async Task<string> SendMessageToClient(string sender,string Identity, string message)
        {
            using (var client = new HttpClient())
            {
                //string url = $"{Constants.BaseAddress}/api/Customer/SendMessageToClient?sender={Uri.EscapeDataString(sender)}&Identity={Uri.EscapeDataString(Identity)}&message={Uri.EscapeDataString(message)}";
                string url = $"{Constants.BaseAddress}/api/Customer/SendMessageToClient?identifier={Uri.EscapeDataString(Identity)}";

                try
                {
                    var apiResponse = await client.GetAsync(url);
                    if (apiResponse.IsSuccessStatusCode)
                    {
                        var connectionId = await apiResponse.Content.ReadAsStringAsync();
                        return connectionId;
                    }
                    else
                    {
                         return null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occurred: {ex.Message}");
                    return "Error";
                }
            }
        }


        public async Task<string> CallForAgent(string Identity, string message)
        {
            using (var client = new HttpClient())
            {
                 string url = $"{Constants.BaseAddress}/api/Customer/SendMessageToClient?identifier={Uri.EscapeDataString(Identity)}";

                try
                {
                    var apiResponse = await client.GetAsync(url);
                    if (apiResponse.IsSuccessStatusCode)
                    {
                        var connectionId = await apiResponse.Content.ReadAsStringAsync();
                        return connectionId;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occurred: {ex.Message}");
                    return "Error";
                }
            }
        }

        #region SendMessageToAgent
        public async Task<bool> SendRequestMessage(NotificationMessages MessageData)
        {
            var returnResponse = false;
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{_baseUrl}/api/Customer/CallForAgent";
                    var serializeContent = JsonConvert.SerializeObject(MessageData);

                    var apiResponse = await client.PostAsync(url, new StringContent(serializeContent, Encoding.UTF8, "application/json"));
                    if (apiResponse.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return returnResponse;
        }

        #endregion


        #endregion
        #region GetOnlineAgents

        public async Task<List<AgentDataModel>> GetOnlineAgents(string networkName, string serviceRequested)
        {
            agentListResponse = new List<AgentDataModel>();

            using (var client = new HttpClient())
            {
                // string url = $"{_baseUrl}/api/Customer/GetOnlineAgents";
                string url = $"{Constants.BaseAddress}/api/Customer/GetOnlineAgents/{networkName}/{serviceRequested}";

                var apiResponse = await client.GetAsync(url);

                var responseContent = await apiResponse.Content.ReadAsStringAsync();
                var Response = JsonConvert.DeserializeObject<ApiResponse>(responseContent);


                if (Response.Success)
                {
                     var agentListResponse = Response.DataList;
                  
                    return agentListResponse;
                }
                return agentListResponse;

            }

        }
        #endregion

        #region Create New Ticket
        public async Task<bool> CreateCustomerTicket(PreparedCustomerTicket NewTicket)
        {
            var returnResponse = false;
            try
            {

                using (var client = new HttpClient())
                {
                     string url = $"{Constants.BaseAddress}/api/Customer/CreateCustomerTicket";
                    
                    var serializeContent = JsonConvert.SerializeObject(NewTicket);
                  
                    var apiResponse = await client.PostAsync(url, new StringContent(serializeContent, Encoding.UTF8, "application/json")).ConfigureAwait(false);

                    //if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    if (apiResponse.IsSuccessStatusCode)
                    {
                        var response = await apiResponse.Content.ReadAsStringAsync();

                        var deserilizeResponse = JsonConvert.DeserializeObject<MainResponse>(response);                       
                        return true;
                        
                    }
                }

            }
            catch (Exception ex)
            {
                string msg =ex.Message;
                Console.WriteLine(msg);
            }
            return returnResponse;
         }
        #endregion

        #region TransactionHistory
        public async Task<List<OpenTicketModel>> GetTransactionsHistory(string custCode)
        {
            TicketHistoryResponse = new List<OpenTicketModel>();
            using (var client = new HttpClient())
            {
                string url = $"{_baseUrl}/api/Customer/GetTransactionsHistory/{custCode}";

                var apiResponse = await client.GetAsync(url);

                if (!apiResponse.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Failed to fetch  tickets History. Status code: {apiResponse.StatusCode}");
                }

                var responseContent = await apiResponse.Content.ReadAsStringAsync();
                var Response = JsonConvert.DeserializeObject<OTApiResponse>(responseContent);

                if (Response.Success)
                {
                    var TicketHistoryResponse = Response.DataList;
                    return TicketHistoryResponse;
                }
                return TicketHistoryResponse;

            }
        }


        #endregion


    }
}
