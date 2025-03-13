using Microsoft.AspNetCore.SignalR;

namespace Telechat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            DateTime sentAt = DateTime.UtcNow;
            await Clients.All.SendAsync("ReceiveMessage", user, message, sentAt);
        }
    }
}
