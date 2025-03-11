using Telechat.Hubs;

var builder = WebApplication.CreateBuilder(args);

/* Services! */
builder.Services.AddSignalR();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
//app.MapStaticAssets();

/* Endpoints */
app.MapHub<ChatHub>("/chatHub");

app.Run();
