using System;
namespace oventy
{
    public static class ClientConstants
    {
        public const string JsonHttpResponseType = "application/json";

#if DEBUG
        public const string ApiUrl = "https://api.oventy.com/";
#else
        public const string ApiUrl = "https://api.oventy.com/";
#endif

        public const string AccessToken = "Ovt.AccessTokenGen/Create";

        public const string Email = "email";
        public const string Password = "password";

        public const string ConnectionString = "Endpoint=sb://xtestnotihubns.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=WVbUvXvo9lMIyzIxNjyUoMlwcZHtkosdRU2RcoHgaG8=";
        public const string NotificationHubPath = "xtestnotihub";
        public const string GoogleSenderId ="902361147448";
    }
}
