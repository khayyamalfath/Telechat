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
            // Sends a message
            DateTime sentAt = DateTime.UtcNow;
            string username = await _messageService.GetUsernameByIdAsync(userId);
            await _messageService.SaveMessageAsync(userId, messageText, sentAt);
            await Clients.All.SendAsync("ReceiveMessage", username, messageText, sentAt);
        }

        public async Task LoadPreviousMessages()
        {
            // Loads previous messages
            var messages = await _messageService.LoadPreviousMessagesAsync();

            var clientMessages = messages.Select(msg => new
            {
                msg.Id,
                msg.MessageText,
                msg.SentAt,
                username = msg.Username
            }).ToList();

            await Clients.Caller.SendAsync("LoadMessages", clientMessages);
        }
    }
}