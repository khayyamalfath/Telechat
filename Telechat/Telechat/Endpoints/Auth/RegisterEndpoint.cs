using Telechat.Services;

namespace Telechat.Endpoints.Auth
{
    public static class RegisterEndpoint
    {
        public static void MapEndpoint(WebApplication app)
        {
            app.MapPost("/api/register", async (HttpContext context, UserService userService) =>
            {
                var data = await context.Request.ReadFromJsonAsync<Telechat.Models.Auth.RegisterRequest>();
                if (data == null || string.IsNullOrEmpty(data.Username) || string.IsNullOrEmpty(data.Password))
                    return Results.BadRequest("Invalid input");

                var success = await userService.AddUserAsync(data.Username, data.Password, data.Email);
                return success ? Results.Ok() : Results.Conflict("User already exists");
            });
        }
    }
}
