using Telechat.Hubs;
using Telechat.Models;
using Telechat.Services;

var builder = WebApplication.CreateBuilder(args);

/* Database */
var connectionString = builder.Configuration.GetConnectionString("TelechatDevelopment");

/* Services! */
builder.Services.AddSingleton(new MessageService(connectionString));
builder.Services.AddSingleton(new UserService(connectionString));
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
    var data = await context.Request.ReadFromJsonAsync<Dictionary<string, string>>();
    if (data == null || !data.ContainsKey("username") || !data.ContainsKey("password"))
        return Results.BadRequest("Invalid input");

    var success = await userService.RegisterAsync(data["username"], data["password"]);
    return success ? Results.Ok() : Results.Conflict("User already exists");
});

app.MapPost("/api/login", async (HttpContext context, UserService userService) =>
{
    var data = await context.Request.ReadFromJsonAsync<Dictionary<string, string>>();
    if (data == null || !data.ContainsKey("username") || !data.ContainsKey("password"))
        return Results.BadRequest("Invalid input");

    var user = await userService.LoginAsync(data["username"], data["password"]);

    if (user == null)
        return Results.Unauthorized();

    return Results.Ok(user.Id);
});

app.Run();
