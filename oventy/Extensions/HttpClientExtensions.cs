using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace oventy
{
    public static class HttpClientExtensions
    {
        /// <summary>
        ///     Custom implementation of POST with timeout
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="requestUri"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> PostAsJsonAsync<T>(
            this HttpClient client,
            string requestUri,
            T value)
        {
            var data = JsonConvert.SerializeObject(value);
            var content = new StringContent(data,
                                            Encoding.UTF8,
                                            ClientConstants.JsonHttpResponseType);
            Debug.WriteLine(client.BaseAddress + requestUri);
            return await client.PostAsync(requestUri,
                                          content)
                               .WithRequestTimeout()
                               .ConfigureAwait(false);
        }

        /// <summary>
        ///     Custom implementation of PUT with timeout
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="requestUri"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> PutAsJsonAsync<T>(
            this HttpClient client,
            string requestUri,
            T value)
        {
            var data = JsonConvert.SerializeObject(value);
            var content = new StringContent(data,
                                            Encoding.UTF8,
                                            ClientConstants.JsonHttpResponseType);
            Debug.WriteLine(client.BaseAddress + requestUri);
            return await client.PutAsync(requestUri,
                                         content)
                               .WithRequestTimeout()
                               .ConfigureAwait(false);
        }

        /// <summary>
        ///     Read JSON response as String.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task<T> ReadAsAsync<T>(
            this HttpContent message)
        {
            var json = await message.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static async Task<HttpResponseMessage> WithRequestTimeout(
            this Task<HttpResponseMessage> task,
            int timeoutInMilliseconds = 20000)
        {
            var result = await task.WithTimeout(timeoutInMilliseconds);
            return result != default(HttpResponseMessage)
                       ? result : new HttpResponseMessage(HttpStatusCode.RequestTimeout);
        }

        public static async Task<ErrorType> GetErrorTypeAsync(
            this HttpResponseMessage message)
        {
            if(message.StatusCode == HttpStatusCode.RequestTimeout)
                return ErrorType.Timeout;

            if(message.StatusCode == HttpStatusCode.InternalServerError)
                return ErrorType.Failed;

            if(message.Content != null)
            {
                var response = await message.Content.ReadAsAsync<ErrorResponse>().ConfigureAwait(false);
                return response.Error;
            }
            return ErrorType.Failed;
        }
    }
}
