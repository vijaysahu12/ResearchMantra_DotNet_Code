using System.Security.Claims;
using System.Text;
using KRCRM.Database.KingResearchContext;
using LMS.API.IService;
using LMS.API.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------
// REGISTER SERVICES
// ---------------------------------------------------------

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ---------------- Swagger + JWT Auth in Swagger UI ----------------
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "LMS API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization using Bearer scheme. Example: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ---------------- Database ----------------
builder.Services.AddDbContext<KingResearchContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GurujiDevCS")));

// ---------------- CORS ----------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
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

// ---------------- Dependency Injection ----------------
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddHttpContextAccessor();

// ---------------- JWT Authentication ----------------
var configuration = builder.Configuration;

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha512 },
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["AppSettings:Token"])
            ),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var mobileUserId = context.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
                var mobileUserKey = context.Principal?.Claims.FirstOrDefault(c => c.Type == "userPublicKey")?.Value;
                var version = context.Principal?.Claims.FirstOrDefault(c => c.Type == "version")?.Value;
                var deviceType = context.Principal?.Claims.FirstOrDefault(c => c.Type == "deviceType")?.Value;

                return Task.CompletedTask;
            }
        };
    });

// ---------------------------------------------------------
// BUILD APP
// ---------------------------------------------------------

var app = builder.Build();

// ---------------- Middleware ----------------

app.UseCors("AllowAngular");

// Swagger MUST come before HTTPS redirection
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LMS API V1");
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
