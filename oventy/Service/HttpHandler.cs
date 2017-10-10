using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Newtonsoft.Json.Linq;

namespace oventy
{
    public class HttpHandler
    {
        private HttpClient _httpClient;

        public HttpHandler()
        {
            _httpClient = new HttpClient();
        }

        #region LoginAsync
        public async Task<bool> LoginAsync(string email, string password)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Logging in...");
                _httpClient.BaseAddress = new Uri(ClientConstants.ApiUrl);
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var request = new HttpRequestMessage(HttpMethod.Post, ClientConstants.AccessToken);

                var oJsonObject = new JObject();
                oJsonObject.Add("UserName", email);
                oJsonObject.Add("Password", password);
                request.Content = new StringContent(oJsonObject.ToString(),
                                                    Encoding.UTF8,
                                                    "application/json");

                var response = await _httpClient.SendAsync(request);
                //UserDialogs.Instance.HideLoading();

                var result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("login result:" + result);
                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonArray = JToken.Parse(result);
                    var access_token = jsonArray["access_token"].ToString();
                    var refresh_token = jsonArray["refresh_token"].ToString();

                    Settings.AccessToken = access_token;
                    Settings.RefreshToken = refresh_token;

                    return true;
                }
                else
                {
                    var jsonArray = JToken.Parse(result);
                    var message = jsonArray["Message"].ToString();
                    ParseError(message);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                ParseError();
                return false;
            }
        }
        #endregion

        #region InstallDeviceAsync
        public async Task<bool> InstallDeviceAsync()
        {
            try
            {
                //_httpClient.BaseAddress = new Uri(ClientConstants.ApiUrl);
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AccessToken);
                _httpClient.DefaultRequestHeaders.Add("X-AccountOnlyAction", "true");

                var request = new HttpRequestMessage(HttpMethod.Put, ClientConstants.InstallDevice);

                var oJsonObject = new JObject();
                oJsonObject.Add("PushChannel", Settings.DeviceToken);
                oJsonObject.Add("Platform", App.DeviceType);
                oJsonObject.Add("InstallationId", Guid.NewGuid().ToString());
                request.Content = new StringContent(oJsonObject.ToString(),
                                                    Encoding.UTF8,
                                                    "application/json");

                Console.WriteLine("request content:" + oJsonObject);
                var response = await _httpClient.SendAsync(request);
                UserDialogs.Instance.HideLoading();

                var result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("InstallDeviceAsync:" + result);

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    var jsonArray = JToken.Parse(result);
                    var message = jsonArray["Message"].ToString();
                    ParseError(message);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                ParseError();
                return false;
            }
        }
        #endregion

        void ParseError(string error_message = "")
        {
            if (string.IsNullOrEmpty(error_message))
                error_message = "Something went wrong. Please try again";

            UserDialogs.Instance.Alert(error_message, "Oh, sorry!", "OK");
        }
    }
}
