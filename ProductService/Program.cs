using KRCRM.Database.KingResearchContext;
using Microsoft.EntityFrameworkCore;
using ProductService.IServices;
using ProductService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<KingResearchContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GurujiDevCS")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins(
                "https://localhost:7289",
                "http://localhost:4200",
                "http://lms.kingresearch.co.in",
                "https://lms.kingresearch.co.in",
                "http://auth.kingresearch.co.in",
                "https://auth.kingresearch.co.in"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});
builder.Services.AddScoped<IResearchService, ResearchService>();


var app = builder.Build();

app.UseCors("AllowAngular");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
