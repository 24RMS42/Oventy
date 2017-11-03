using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace oventy
{
    public class ClientRequest
    {
        public ClientRequest(string endpoint, HttpMethod httpMethod)
        {
            QueryString = endpoint;
            HttpMethod = httpMethod;
        }

        public void Initialize(string[] queryStringIds = null, KeyValuePair<string, string>[] queryParams = null)
        {
            if(queryStringIds != null && queryStringIds.Any())
            {
                QueryString = string.Format(QueryString, queryStringIds);
            }

            if(queryParams != null && queryParams.Any())
            {
                QueryString = queryParams.Aggregate($"{QueryString}?",
                                                   (current,
                                                    parameter) => AddParam(current,
                                                                           parameter.Key,
                                                                           parameter.Value));
            }

        }

        public HttpMethod HttpMethod { get; private set; }

        public string QueryString { get; private set; }

        /// <summary>
        ///     Add params to url.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string AddParam(
            string url,
            string name,
            object value)
        {
            var formatString = url.EndsWith("?",
                                            StringComparison.Ordinal) ? "{0}{1}={2}" : "{0}&{1}={2}";
            return string.Format(formatString,
                                 url,
                                 name,
                                 Uri.EscapeDataString(value.ToString()));
        }
    }
}
