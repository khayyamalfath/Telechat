using MySql.Data.MySqlClient;
using Telechat.Models;

namespace Telechat.DataAccess
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> AddUserAsync(User user)
        {
            // Saves user to DB

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = "INSERT INTO Users (Name, PasswordHash, Email, RegisteredAt) VALUES (@Name, @PasswordHash, @Email, @RegisteredAt)";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Name", user.Name);
            cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            cmd.Parameters.AddWithValue("@Email", (object?)user.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@RegisteredAt", user.RegisteredAt);

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

        public async Task<User?> GetUserByCredentialsAsync(string username, string passwordHash)
        {
            // Checks user against DB

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = "SELECT Id FROM Users WHERE Name = @Name AND PasswordHash = @PasswordHash";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Name", username);
            cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = reader.GetInt32(0),
                    Name = username,
                    PasswordHash = passwordHash
                };
            }

            return null;
        }
    }
}
