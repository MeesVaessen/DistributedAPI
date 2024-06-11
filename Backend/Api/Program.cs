using Dal.Interfaces;
using Dal;
using Dal.Repository;
using Logic;
using Logic.Interfaces.Manager;
using Logic.Interfaces.Repository;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Logic.Managers;
using Fleck;
using Api;
using FluentFTP;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IConfiguration configuration = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .Build();

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Distributed Computing API", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
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

        var TestURL = configuration["CORS:Front-End-Prod-Test"];
        var ProdURL = configuration["CORS:Front-End-URL"];
        var ProdURL2 = configuration["CORS:Front-End-URL2"];
        var ProdDashURL = configuration["CORS:Front-End-Dashboard"]; 


        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder.WithOrigins(TestURL.ToString(), ProdURL.ToString(), ProdURL2.ToString(), ProdDashURL.ToString())
                           .AllowAnyHeader()
                           .AllowCredentials()
                           .AllowAnyMethod();
                });
        });

    builder.Services.AddSingleton<IUserRepository, UserRepository>();
    builder.Services.AddSingleton<IFileRepository, FileRepository>();   
    builder.Services.AddScoped<IUserManager, UserManager>();
    builder.Services.AddScoped<IFileManager, FileManager>();
    builder.Services.AddSingleton<IDBContext, ApplicationDBContext>();
    builder.Services.AddSingleton<WebSocketHandler>();

    builder.Services.AddSingleton<IAsyncFtpClient, AsyncFtpClient>();

// Register FluentFTP client as a transient service
/*builder.Services.AddTransient<IAsyncFtpClient>(_ =>
{
    var ftpClient = new FtpClient();
    ftpClient.Host = "192.168.0.2";
    ftpClient.Credentials = new NetworkCredential("Apiuser", "P@ssword");
    return (IAsyncFtpClient)ftpClient;
});*/


builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = "http://localhost:5173",
            ValidIssuer = "https://localhost:7080",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ByYM000OLlMQG6VVVp1OH7Xzyr7gHuw1qvUC5dcGt3SNM"))
        };
    });

var app = builder.Build();

// Retrieve WebSocket server instance
var webSocketServer = app.Services.GetService<WebSocketHandler>();
// Start the WebSocket server
webSocketServer.StartServer();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
