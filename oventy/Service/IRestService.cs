using System;
using System.Threading.Tasks;

namespace oventy
{
    public interface IRestService
    {
        Task<ClientResponse<TResponse>> SendAsync<TResponse>(ClientRequest request, object data = null);

        Task<ClientResponse> SendAsync(ClientRequest request, object data = null);
    }
}
