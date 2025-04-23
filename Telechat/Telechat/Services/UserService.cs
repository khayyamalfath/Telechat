using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;
using Telechat.Models;

namespace Telechat.Services
{
    public class UserService
    {
        private readonly string _connectionString;

        public UserService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> RegisterAsync(string username, string password, string? email = null)
        {
            // Saves user to DB if user does not exist else returns false

            DateTime registeredAt = DateTime.UtcNow;

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            string passwordHash = ComputeSha256Hash(password);
            string query = "INSERT INTO Users (Name, PasswordHash, Email, RegisteredAt) VALUES (@Name, @PasswordHash, @Email, @RegisteredAt)";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Name", username);
            cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
            cmd.Parameters.AddWithValue("@Email", (object)email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@RegisteredAt", registeredAt);

            try
            {
                await cmd.ExecuteNonQueryAsync();
                return true;
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                // Duplicate entry

                return false;
            }
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            // Checks user against DB, returns true + Id if user exists else returns null

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            string passwordHash = ComputeSha256Hash(password);
            string query = "SELECT Id FROM Users WHERE Name = @Name AND PasswordHash = @PasswordHash";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Name", username);
            cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return new User
                {
                    Id = reader.GetInt32(0)
                };

            return null;
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
