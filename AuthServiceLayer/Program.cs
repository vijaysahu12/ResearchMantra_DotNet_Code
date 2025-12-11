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

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddDbContext<KingResearchContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GurujiDevCS")));

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
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

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "lms API", Version = "v1" });

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer {token}')",
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
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha512 }, // Force HS512
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value!)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true, // Ensure token expiration is validated
        ClockSkew = TimeSpan.Zero,
        RequireSignedTokens = false,

    };
    options.Events = new JwtBearerEvents
    {
        // Generate random numbers

        OnTokenValidated = async context =>
        {
            //var _mongoDb = context.HttpContext.RequestServices.GetRequiredService<MongoDbService>();
            var mobileUserId = context.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            var mobileUserKey = context.Principal?.Claims.FirstOrDefault(c => c.Type == "userPublicKey")?.Value;
            var version = context.Principal?.Claims.FirstOrDefault(c => c.Type == "version")?.Value;
            var deviceType = context.Principal?.Claims.FirstOrDefault(c => c.Type == "deviceType")?.Value;

            //await _mongoDb.ManageUserVersionReport("OnTokenValidation", deviceType, version, mobileUserKey, Convert.ToInt64(mobileUserId));
        },
        //OnAuthenticationFailed = async context =>
        //        {
        //            var _mongoDb = context.HttpContext.RequestServices.GetRequiredService<IMongoRepository<ExceptionLog>>();
        //            var accessToken = context.Request.Headers["Authorization"].ToString();

        //            var mobileUserId = context.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid)?.Value;
        //            var mobileUserKey = context.Principal?.Claims.FirstOrDefault(c => c.Type == "userPublicKey")?.Value;

        //            try
        //            {
        //                var payloadJson = DecodeJwtPayload(accessToken);

        //                if (payloadJson == null)
        //                {
        //                    payloadJson = accessToken;
        //                }

        //                await _mongoDb.AddAsync(new ExceptionLog
        //                {
        //                    CreatedOn = DateTime.Now,
        //                    InnerException = context.Exception?.InnerException?.ToString(),
        //                    Message = context.Exception?.Message,
        //                    StackTrace = context.Exception?.StackTrace,
        //                    RequestBody = context.Response.StatusCode + " : " + mobileUserId + " : " + mobileUserKey + " : " + payloadJson,
        //                    Source = "JWT"
        //                });
        //            }
        //            catch
        //            {
        //                Console.Write("Catch while parsing jwt token");
        //            }


        //            //// Ensure response modification is only done if the response has not started
        //            //if (!context.Response.HasStarted)
        //            //{
        //            //    // Return 401 Unauthorized
        //            //    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        //            //    context.Response.ContentType = "application/json";

        //            //    // Customize the response body if needed
        //            //    var result = System.Text.Json.JsonSerializer.Serialize(new
        //            //    {
        //            //        error = "Authentication failed",
        //            //        details = context.Exception.Message
        //            //    });

        //            //    await context.Response.WriteAsync(result);
        //            //}
        //        }
    };
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseCors("AllowAngular");
app.UseSwagger();
_ = app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LMS API V1");
});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
