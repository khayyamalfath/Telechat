using Telechat.Endpoints.Auth;
using Telechat.Endpoints.Chat;
using Telechat.Extensions;

namespace Telechat.Extensions
{
    public static class EndpointExtensions
    {
        public static void MapEndpoints(this WebApplication app)
        {
            RegisterEndpoint.MapEndpoint(app);
            LoginEndpoint.MapEndpoint(app);

            ChatHubEndpoint.MapEndpoint(app);
        }
    }
}
