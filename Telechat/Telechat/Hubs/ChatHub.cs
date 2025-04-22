using Microsoft.AspNetCore.SignalR;
using Telechat.Models;
using Telechat.Services;

namespace Telechat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly MessageService _messageService;

        public ChatHub(MessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task SendMessage(int userId, string messageText)
        {
            /* Sends a message */
            DateTime sentAt = DateTime.UtcNow;
            string username = await _messageService.GetUsernameByIdAsync(userId);

            await _messageService.SaveMessageAsync(userId, messageText, sentAt);
            await Clients.All.SendAsync("ReceiveMessage", username, messageText, sentAt);
        }

        public async Task LoadPreviousMessages()
        {
            /* Loads previous messages */

            List<Message> messages = await _messageService.GetMessagesAsync(256);

            // Estrai gli UserId dai messaggi
            var userIds = messages.Select(m => m.UserId).Distinct().ToList();

            // Recupera tutti gli username in una sola query
            var users = await _messageService.GetUsernamesByIdsAsync(userIds);

            // Crea un dizionario UserId -> Username
            var userDict = users.ToDictionary(u => u.Id, u => u.Name);

            // Crea una lista arricchita con gli username
            var enrichedMessages = messages.Select(msg => new
            {
                msg.Id,
                msg.MessageText,
                msg.SentAt,
                Username = userDict.ContainsKey(msg.UserId) ? userDict[msg.UserId] : "Unknown"
            }).ToList();

            // Invia i messaggi arricchiti al client
            await Clients.Caller.SendAsync("LoadMessages", enrichedMessages);
        }
    }
}
