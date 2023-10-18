using System.Reflection;
using System.Text;
using EventModule;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Shared.API;
using UserModule;
using UserModule.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEventModule();
builder.Services.AddUserModule();
// todo: check how well this works when i have validators in EventModule
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>(ServiceLifetime.Transient);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var secrets = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(
        x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidAudience = secrets["JwtSettings:Audience"],
                ValidIssuer = secrets["JwtSettings:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secrets["JwtSettings:Key"] ??
                    throw new InvalidOperationException("Security Key is null!"))),
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });
builder.Services.AddAuthorization();

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
app.UseMiddleware<ExceptionMiddleware>();
app.UseEventModule();
app.UseUserModule();

app.Run();