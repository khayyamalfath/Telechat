using Microsoft.AspNetCore.Identity.Data;
using Telechat.DataAccess;
using Telechat.Hubs;
using Telechat.Services;

var builder = WebApplication.CreateBuilder(args);

/* Database */
var connectionString = builder.Configuration.GetConnectionString("TelechatDevelopment");

/* Services! */
builder.Services.AddSingleton(new MessageService(connectionString));
builder.Services.AddScoped<IUserRepository>(sp => new UserRepository(connectionString));
builder.Services.AddScoped<UserService>();
builder.Services.AddSignalR();

var app = builder.Build();

// Trying to redirect from index.html to login.html
//app.MapGet("/", context =>
//{
//    context.Response.Redirect("/login.html");
//    return Task.CompletedTask;
//});

app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "login.html" }
});

app.UseStaticFiles();
//app.MapStaticAssets();

/* Endpoints */
app.MapHub<ChatHub>("/chatHub");

app.MapPost("/api/register", async (HttpContext context, UserService userService) =>
{
    var data = await context.Request.ReadFromJsonAsync<Telechat.Models.Auth.RegisterRequest>();
    if (data == null || string.IsNullOrEmpty(data.Username) || string.IsNullOrEmpty(data.Password))
        return Results.BadRequest("Invalid input");

    var success = await userService.RegisterAsync(data.Username, data.Password, data.Email);
    return success ? Results.Ok() : Results.Conflict("User already exists");
});

app.MapPost("/api/login", async (HttpContext context, UserService userService) =>
{
    var data = await context.Request.ReadFromJsonAsync<Telechat.Models.Auth.LoginRequest>();
    if (data == null || string.IsNullOrEmpty(data.Username) || string.IsNullOrEmpty(data.Password))
        return Results.BadRequest("Invalid input");

    var user = await userService.LoginAsync(data.Username, data.Password);

    if (user == null)
        return Results.Unauthorized();

    return Results.Ok(user.Id);
});

app.Run();
