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

        public async Task SaveMessageAsync(string username, string message, DateTime sentAt)
        {
            /* Saves a message to DB */

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "INSERT INTO Messages (Message, Username, SentAt) VALUES (@Message, @Username, @SentAt)";

                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Message", message);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@SentAt", sentAt);

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

                string query = "SELECT Id, Message, Username, SentAt FROM Messages ORDER BY SentAt DESC LIMIT @Limit";

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
                                Username = reader.GetString(2),
                                SentAt = reader.GetDateTime(3)
                            });
                        }
                    }
                }
            }

            messages.Reverse();
            return messages;
        }
    }
}
