using Telechat.Models;

namespace Telechat.DataAccess
{
    public interface IMessageRepository
    {
        Task<bool> AddUserAsync(User user);
        Task<User?> GetUserByCredentialsAsync(string username, string passwordHash);
    }
}
