using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ModernHttpClient;
using Newtonsoft.Json;

namespace oventy
{
    public class RestService : IRestService
    {
        private readonly IDataStorageService _accountStorageService;
        private readonly string _apiUrl;

        public RestService(IDataStorageService accountStorageService)
        {
            _accountStorageService = accountStorageService;
            _apiUrl = ClientConstants.ApiUrl;
        }

        public async Task<ClientResponse> SendAsync(ClientRequest request, object data = null)
        {
            var response = await CentralizedSendAsync(request, data);

            return await GetHttpRespose(response);
        }

        public async Task<ClientResponse<TResult>> SendAsync<TResult>(ClientRequest request, object data = null)
        {
            var response = await CentralizedSendAsync(request, data);

            return await GetHttpRespose<TResult>(response);
        }

        private Task<HttpResponseMessage> CentralizedSendAsync(ClientRequest request, object data = null)
        {
            return MakeStandardCall(request, data);
        }

        private async Task<HttpResponseMessage> MakeStandardCall(ClientRequest request, object data = null)
        {
            using(var client = CreateClient())
            {
                using(var httpRequest = new HttpRequestMessage { RequestUri = new Uri(_apiUrl + request.QueryString), Method = request.HttpMethod })
                {
                    if((request.HttpMethod == HttpMethod.Post || request.HttpMethod == HttpMethod.Put)
                        && data != null)
                    {
                        var jsonData = JsonConvert.SerializeObject(data);
                        httpRequest.Content = new StringContent(jsonData,
                                            Encoding.UTF8,
                                            ClientConstants.JsonHttpResponseType);
                    }

                    Debug.WriteLine(request.QueryString);

                    var result = await client.SendAsync(httpRequest)
                                             .WithRequestTimeout()
                                             .ConfigureAwait(false);

                    return result;
                }
            }
        }

        private HttpClient CreateClient()
        {
            var client = new HttpClient(new NativeMessageHandler())
            {
                MaxResponseContentBufferSize = 2560000000
            };
            client.DefaultRequestHeaders.Add("sessionToken",
                                             _accountStorageService.Token);
            return client;
        }

        private async Task<ClientResponse> GetHttpRespose(HttpResponseMessage response)
        {
            if(response.IsSuccessStatusCode)
            {
                return new ClientResponse();
            }
            else
            {
                throw new BusinessException(await response.GetErrorTypeAsync());
            }
        }

        private async Task<ClientResponse<TResult>> GetHttpRespose<TResult>(HttpResponseMessage response)
        {
            if(response.IsSuccessStatusCode)
            {
                return new ClientResponse<TResult>(await response.Content.ReadAsAsync<TResult>().ConfigureAwait(false));
            }
            else
            {
                throw new BusinessException(await response.GetErrorTypeAsync().ConfigureAwait(false));
            }
        }
    }
}
