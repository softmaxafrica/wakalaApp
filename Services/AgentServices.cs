using mobilewakala.Models;
using mobilewakala.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
 using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
 
namespace mobilewakala.Services
{
    public class AgentServices : IAgentServices
    {
        private string _baseUrl = Constants.BaseAddress;
        JsonSerializerOptions _serializerOptions;
        private string statusResponse;

        public List<OpenTicketModel> OpenTicketListResponse { get; private set; }
        public List<OpenTicketModel> TicketHistoryResponse { get; private set; }


        #region AgentRegistration
        public async Task<bool> RegisterAgent(AgentDataModel agentData)
        {
            var returnResponse = false;
            try
            {



                using var client = new HttpClient();

                string url = $"{_baseUrl}/api/Agent/registeragent";
                var serializeContent = JsonConvert.SerializeObject(agentData);
                var apiResponse = await client.PostAsync(url, new StringContent(serializeContent, Encoding.UTF8, "application/json"));

                if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var response = await apiResponse.Content.ReadAsStringAsync();

                    var deserilizeResponse = JsonConvert.DeserializeObject<MainResponse>(response);

                    if (deserilizeResponse.IsSuccess)
                    {
                    }
                    else
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
        #region LoginService
        public async Task<bool> LoginService(LoginModel loggedUser)
        {
            var returnResponse = false;
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{_baseUrl}/api/Agent/Login";
                    var serializeContent = JsonConvert.SerializeObject(loggedUser);
                    var apiResponse = await client.PostAsync(url, new StringContent(serializeContent, Encoding.UTF8, "application/json"));
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
                string msg = ex.Message;
            }
            return returnResponse;
        }
        #endregion

        #region CheckStatus Of Agent
        public async Task<string> checkAgentStatus(string agentCode)
        {
            using (var client = new HttpClient())
            {
                string url = $"{_baseUrl}/api/Agent/checkAgentStatus/{agentCode}";

                var apiResponse = await client.GetAsync(url);

                if (!apiResponse.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Failed to fetch agent status. Status code: {apiResponse.StatusCode}");
                }

                var responseContent = await apiResponse.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<JObject>(responseContent);

                // Extract the status from the response object
                string status = responseObject["data"]?.ToString();
                return status;

            }
        }
        #endregion

        #region  Get Open Tickets
        public async Task<List<OpenTicketModel>> GetOPenTicket(string agentCode)
        {
            OpenTicketListResponse = new List<OpenTicketModel>();
            using (var client = new HttpClient())
            {
                string url = $"{_baseUrl}/api/Agent/GetOPenTicket/{agentCode}";

                var apiResponse = await client.GetAsync(url);

                if (!apiResponse.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Failed to fetch open tickets. Status code: {apiResponse.StatusCode}");
                }

                var responseContent = await apiResponse.Content.ReadAsStringAsync();
                var Response = JsonConvert.DeserializeObject<OTApiResponse>(responseContent);

                if (Response.Success)
                {
                    var OpenTicketListResponse = Response.DataList;
                    return OpenTicketListResponse;
                }
                return OpenTicketListResponse;

            }

        }
        #endregion

        #region ChangeStatus
        public async Task<bool> ChangeStatus(AgentStatusModel NewStatus)
        {
            var returnResponse = false;
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{_baseUrl}/api/Agent/ChangeStatus";
                    var serializeContent = JsonConvert.SerializeObject(NewStatus);
                    var apiResponse = await client.PutAsync(url, new StringContent(serializeContent, Encoding.UTF8, "application/json"));
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
                string msg = ex.Message;
            }
            return returnResponse;
        }
        #endregion
        #region OnReceiveNotification
        public async Task<bool> DeliveryReport(NotificationMessages MessageData)
        {
            var returnResponse = false;
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{_baseUrl}/api/Agent/DeliveryReport";
                    var serializeContent = JsonConvert.SerializeObject(MessageData);

                    var apiResponse = await client.PutAsync(url, new StringContent(serializeContent, Encoding.UTF8, "application/json"));
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
        #region SyncAgentLocation
        public async Task<bool> SyncLoaction(AgentLocationSync currentLocation)
        {
            var returnResponse = false;
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{_baseUrl}/api/Agent/SyncLiveAgentLocation";
                    var serializeContent = JsonConvert.SerializeObject(currentLocation);

                    var apiResponse = await client.PutAsync(url, new StringContent(serializeContent, Encoding.UTF8, "application/json"));
                    if (apiResponse.IsSuccessStatusCode)
                    {
                        returnResponse = true;
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
        #region AttendTicket
        public async Task<bool> AttendCustTicket(OpenTicketModel ticketDetails)
        {
            var returnResponse = false;
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{_baseUrl}/api/Agent/AttendCustTicket";
                    var serializeContent = JsonConvert.SerializeObject(ticketDetails);
                    var apiResponse = await client.PutAsync(url, new StringContent(serializeContent, Encoding.UTF8, "application/json"));
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
                string msg = ex.Message;
            }
            return returnResponse;
        }

        public async Task<List<OpenTicketModel>> GetTicketHistory(string agentCode)
        {
            TicketHistoryResponse = new List<OpenTicketModel>();
            using (var client = new HttpClient())
            {
                string url = $"{_baseUrl}/api/Agent/GetTicketHistory/{agentCode}";

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
