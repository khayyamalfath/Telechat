using Telechat.Services;

namespace Telechat.Endpoints.Auth
{
    public static class LoginEndpoint
    {
        public static void MapEndpoint(WebApplication app)
        {
            app.MapPost("/api/login", async (HttpContext context, UserService userService) =>
            {
                var data = await context.Request.ReadFromJsonAsync<Telechat.Models.Auth.LoginRequest>();
                if (data == null || string.IsNullOrEmpty(data.Username) || string.IsNullOrEmpty(data.Password))
                    return Results.BadRequest("Invalid input");

                var user = await userService.GetUserByCredentialsAsync(data.Username, data.Password);

                if (user == null)
                    return Results.Unauthorized();

                return Results.Ok(user.Id);
            });
        }
    }
}
