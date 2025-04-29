using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;
using Telechat.DataAccess;
using Telechat.Models;

namespace Telechat.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> AddUserAsync(string username, string password, string? email = null)
        {
            // Gets hash, sends credentials to db

            var passwordHash = ComputeSha256Hash(password);

            var user = new User
            {
                Name = username,
                PasswordHash = passwordHash,
                Email = email,
                RegisteredAt = DateTime.UtcNow
            };

            return await _userRepository.AddUserAsync(user);
        }

        public async Task<User?> GetUserByCredentialsAsync(string username, string password)
        {
            // calculates hash, gets user Id

            var passwordHash = ComputeSha256Hash(password);
            return await _userRepository.GetUserByCredentialsAsync(username, passwordHash);
        }

        private static string ComputeSha256Hash(string rawData)
        {
            // Hashes password

            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }
    }
}
