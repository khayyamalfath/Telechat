using Telechat.DataAccess;
using Telechat.Extensions;
using Telechat.Hubs;
using Telechat.Services;

var builder = WebApplication.CreateBuilder(args);

/* Database */
var connectionString = builder.Configuration.GetConnectionString("TelechatDevelopmentLade");

/* Services! */
builder.Services.AddSingleton(new MessageService(connectionString));
builder.Services.AddScoped<IUserRepository>(sp => new UserRepository(connectionString));
builder.Services.AddScoped<UserService>();
//builder.Services.AddScoped<IMessageRepository>(sp => new MessageRepository(connectionString));
//builder.Services.AddScoped<MessageService>();
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
app.MapEndpoints();

app.Run();
