using MySql.Data.MySqlClient;
using Telechat.Models;

namespace Telechat.Services
{
    public class MessageService
    {
        private readonly string _connectionString;

        public MessageService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task SaveMessageAsync(int userId, string messageText, DateTime sentAt)
        {
            /* Saves a message to DB */

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "INSERT INTO Messages (MessageText, SentAt, UserId) VALUES (@MessageText, @SentAt, @UserId)";

                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@MessageText", messageText);
                    cmd.Parameters.AddWithValue("@SentAt", sentAt);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<Message>> GetMessagesAsync(int limit = 256)
        {
            /* Gets previous messages from DB */

            var messages = new List<Message>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                //string query = @"SELECT Messages.Id, Messages.MessageText, Users.Name AS Username, Messages.SentAt 
                //                    FROM Messages 
                //                    JOIN Users ON Messages.UserId = Users.Id 
                //                    ORDER BY Messages.SentAt DESC 
                //                    LIMIT @Limit";

                string query = "SELECT Id, MessageText, SentAt, UserId FROM Messages ORDER BY SentAt DESC LIMIT @Limit";

                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Limit", limit);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            messages.Add(new Message
                            {
                                Id = reader.GetInt64(0),
                                MessageText = reader.GetString(1),
                                SentAt = reader.GetDateTime(2),
                                UserId = reader.GetInt32(3)
                            });
                        }
                    }
                }
            }

            messages.Reverse();
            return messages;
        }

        public async Task<string> GetUsernameByIdAsync(int userId)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = "SELECT Name FROM Users WHERE Id = @Id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", userId);

            return (string)(await cmd.ExecuteScalarAsync()) ?? "Unknown";
        }

        public async Task<List<User>> GetUsernamesByIdsAsync(List<int> userIds)
        {
            var users = new List<User>();

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            string query = "SELECT Id, Name FROM Users WHERE Id IN (@UserIds)";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@UserIds", string.Join(",", userIds));

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                users.Add(new User
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }

            return users;
        }

    }
}
