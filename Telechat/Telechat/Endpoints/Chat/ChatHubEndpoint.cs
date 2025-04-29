using Telechat.Hubs;

namespace Telechat.Endpoints.Chat
{
    public static class ChatHubEndpoint
    {
        public static void MapEndpoint(WebApplication app)
        {
            app.MapHub<ChatHub>("/chatHub");
        }
    }
}
