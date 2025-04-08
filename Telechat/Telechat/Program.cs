using Telechat.Hubs;
using Telechat.Services;

var builder = WebApplication.CreateBuilder(args);

/* Database */
var connectionString = builder.Configuration.GetConnectionString("TelechatDevelopment");

/* Services! */
builder.Services.AddSingleton(new MessageService(connectionString));
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

app.Run();
