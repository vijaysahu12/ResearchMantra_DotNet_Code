using Microsoft.EntityFrameworkCore;
using TradingServiceLayer.DbContextFolder;
using TradingServiceLayer.Hubs;
using TradingServiceLayer.IServices;
using TradingServiceLayer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer(); // Required for Swagger
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Title = "Trading Service API",
//        Version = "v1",
//        Description = "API for Trading Platform"
//    });
//});

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


// SignalR
builder.Services.AddSignalR();
builder.Services.AddSingleton<ISignalRNotifier, SignalRNotifier>();

// Services
builder.Services.AddSingleton<ITradeService, TradeService>();
builder.Services.AddHttpClient<INotificationApi, NotificationApi>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7198/");
});


// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalAngular", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Swagger middleware
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Trading Service API V1");
//        c.RoutePrefix = string.Empty; // Swagger UI at root: https://localhost:7198/
//    });
//}

app.UseHttpsRedirection();
app.UseCors("AllowLocalAngular");
app.UseAuthorization();

// Map SignalR hubs
app.MapHub<LiveTradeHub>("/hubs/live-trade");

// Map controllers
app.MapControllers();

app.Run();
