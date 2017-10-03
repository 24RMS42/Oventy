using System;
namespace oventy
{
    public interface IDataStorageService
    {
        string Token { get; set; }

        string PushToken { get; set; }

        bool LoggedIn { get; }

        //UserDTO User { get; set; }

        string InstallationId { get; set; }

        string CachedEmail { get; set; }
    }
}
