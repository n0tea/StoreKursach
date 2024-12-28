//using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using DataAcessLayer.ContextDB;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Backend.Api.Services;
using Backend.Api.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Confluent.Kafka;

//using Microsoft.AspNetCore.Authentication.JwtBearer

var builder = WebApplication.CreateBuilder(args);

// Настройка Kafka Producer
builder.Services.AddSingleton<IProducer<string, string>>(provider =>
{
    var config = new ProducerConfig
    {
        BootstrapServers = "localhost:9092" // Адрес вашего Kafka брокера
    };
    return new ProducerBuilder<string, string>(config).Build();
});

builder.Services.Configure<JwtConfigurationOptions>(builder.Configuration.GetSection("JWT"));
//builder.Services.Configure<>

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Настройка безопасности через Bearer токен
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
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
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("UsersDb")));

builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ProductsDb")));

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("OrdersDb")));

// JWT Authentication setup
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var config = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JwtConfigurationOptions>>().Value;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
        };
    });

builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<JwtService>();

// Kafka Consumer
builder.Services.AddSingleton<IConsumer<string, string>>(provider =>
{
    var config = new ConsumerConfig
    {
        BootstrapServers = "localhost:9092",
        GroupId = "notification-service",
        AutoOffsetReset = AutoOffsetReset.Earliest
    };
    return new ConsumerBuilder<string, string>(config).Build();
});

// NotificationService
builder.Services.AddSingleton<NotificationService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

// Запуск NotificationService
var notificationService = app.Services.GetService<NotificationService>();
Task.Run(() => notificationService.StartListening());

app.Run();
