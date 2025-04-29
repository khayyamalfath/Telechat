using MySql.Data.MySqlClient;
using Telechat.Models;

namespace Telechat.DataAccess
{
    public class MessageRepository : IMessageRepository
    {
        private readonly string _connectionString;

        public MessageRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
    }
}
