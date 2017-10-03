using System;
using System.Threading.Tasks;

namespace oventy
{
    public class AccountDataSource : IAccountDataSource
    {
        private readonly IApiEndpointsFactory _apiEndpointsFactory;
        private readonly IDataStorageService _dataStorage;
        private readonly IRestService _restService;

        public AccountDataSource(IApiEndpointsFactory apiEndpointsFactory,
                                 IDataStorageService accountStorage,
                                 IRestService restService)
        {
            _apiEndpointsFactory = apiEndpointsFactory;
            _dataStorage = accountStorage;
            _restService = restService;
        }

        public async Task<UserLogin> LogInAsync(string email, string password)
        {
            var request = _apiEndpointsFactory.CreateClientRequest(ApiEndpoint.Login,
                                                                   queryParams: new[]
                                                                   {
                                                                        ClientConstants.Email.ToKeyValuePair(email),
                                                                        ClientConstants.Password.ToKeyValuePair(password)
                                                                   });

            var response = await _restService.SendAsync<UserLogin>(request).ConfigureAwait(false);

            _dataStorage.Token = response.Value.AccessToken;

            return response.Value;
        }

        public Task LogOutAsync()
        {
            throw new NotImplementedException();
        }
    }
}
