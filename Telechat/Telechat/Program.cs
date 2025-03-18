using Telechat.Hubs;
using Telechat.Services;

var builder = WebApplication.CreateBuilder(args);

/* Database */
var connectionString = builder.Configuration.GetConnectionString("TelechatDevelopment");

/* Services! */
builder.Services.AddSingleton(new MessageService(connectionString));
builder.Services.AddSignalR();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
//app.MapStaticAssets();

/* Endpoints */
app.MapHub<ChatHub>("/chatHub");

app.Run();
