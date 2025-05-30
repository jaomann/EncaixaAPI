using EncaixaAPI.Core.Contracts;
using EncaixaAPI.Core.Entities;
using EncaixaAPI.Database;
using EncaixaAPI.Repository;
using EncaixaAPI.Services;
using EncaixaAPI.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

var connectionString = builder.Configuration["CONNECTION_STRING"]
                      ?? "Server=sqlserver;Database=EncaixaDb;User Id=sa;Password="
                         + Environment.GetEnvironmentVariable("DB_PASSWORD")
                         + ";TrustServerCertificate=True;";

builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(
        connectionString,
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 15,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null);
            sqlOptions.CommandTimeout(180);
        }
    ));

builder.Services.AddResponseCaching();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["ValidIssuer"],
        ValidAudience = jwtSettings["ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
    };
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EncaixaAPI", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
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

builder.Services.AddScoped<IPackingService, PackingService>();
builder.Services.AddScoped<IBoxRepository, BoxRepository>();
builder.Services.AddScoped<IBoxService, BoxService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddScoped<AuthService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<Context>();

    try
    {
        context.Database.EnsureCreated();

        if (!context.Boxes.Any())
        {
            Console.WriteLine("Inserindo dados iniciais...");

            var boxes = new[]
            {
                new Box {
                    Name = "Caixa Pequena",
                    Width = 30m,
                    Height = 40m,
                    Depth = 80m,
                    AvailableQuantity = 100,
                    MaxWeight = 5m
                },
                new Box {
                    Name = "Caixa Média",
                    Width = 80m,
                    Height = 50m,
                    Depth = 40m,
                    AvailableQuantity = 100,
                    MaxWeight = 10m
                },
                new Box {
                    Name = "Caixa Grande",
                    Width = 50m,
                    Height = 80m,
                    Depth = 60m,
                    AvailableQuantity = 50,
                    MaxWeight = 20m,
                    Type = "Reforçada"
                }
            };

            await context.Boxes.AddRangeAsync(boxes);
            await context.SaveChangesAsync();

            Console.WriteLine("3 caixas padrão criadas com sucesso!");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao inicializar banco: {ex.Message}");
    }
}

app.Run();
