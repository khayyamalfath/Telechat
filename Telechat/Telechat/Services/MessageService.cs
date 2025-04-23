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

        public async Task<List<MessageWithUsername>> LoadPreviousMessagesAsync(int limit = 256)
        {
            // Joins Messages with Users and gets list of all messages with the corresponding usernames

            var messages = new List<MessageWithUsername>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                    SELECT Messages.Id, Messages.MessageText, Messages.SentAt, Messages.UserId, Users.Name as Username
                    FROM Messages
                    LEFT JOIN Users ON Messages.UserId = Users.Id
                    ORDER BY Messages.SentAt ASC
                    LIMIT @Limit";

                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Limit", limit);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            messages.Add(new MessageWithUsername
                            {
                                Id = reader.GetInt64(0),
                                MessageText = reader.GetString(1),
                                SentAt = reader.GetDateTime(2),
                                UserId = reader.GetInt32(3),
                                Username = reader.IsDBNull(4) ? "Unknown" : reader.GetString(4)
                            });
                        }
                    }
                }
            }

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
    }
}
