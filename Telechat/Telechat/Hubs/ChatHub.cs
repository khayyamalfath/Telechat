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

        public async Task SendMessage(string username, string messageText)
        {
            /* Sends a message */
            DateTime sentAt = DateTime.UtcNow;

            await _messageService.SaveMessageAsync(username, messageText, sentAt);

            await Clients.All.SendAsync("ReceiveMessage", username, messageText, sentAt);
        }

        public async Task LoadPreviousMessages()
        {
            /* Loads previous messages */
            List<Message> messages = await _messageService.GetMessagesAsync(256);

            await Clients.Caller.SendAsync("LoadMessages", messages);
        }
    }
}
