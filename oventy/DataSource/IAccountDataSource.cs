using System;
using System.Threading.Tasks;

namespace oventy
{
    public interface IAccountDataSource
    {
        Task<UserLogin> LogInAsync(
            string email,
            string password);

        Task LogOutAsync();
    }
}
