using System;
using System.Collections.Generic;

namespace oventy
{
    public interface IApiEndpointsFactory
    {
        ClientRequest CreateClientRequest(ApiEndpoint endpoint, string[] queryStringIds = null, KeyValuePair<string, string>[] queryParams = null);
    }
}
