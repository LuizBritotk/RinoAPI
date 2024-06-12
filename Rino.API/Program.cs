using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Rino.API.Configurations;
using Rino.Domain.Handlers;
using Rino.Domain.Repositories;
using Rino.Domain.Services;
using Rino.Infrastructure.Authentication;
using Rino.Infrastructure.Data;
using Rino.Infrastructure.Repositories;
using Rino.Infrastructure.Services;
using Rino.Infrastructure.Utilities;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Register PasswordHasher
builder.Services.AddSingleton<PasswordHasher>();

// Register JwtHandler
builder.Services.AddScoped<IJwtHandler, JwtHandler>();

// Register UserHandler
builder.Services.AddScoped<UserHandler>();

// Register other services
builder.Services.Scan(scan => scan
    .FromAssembliesOf(typeof(IJwtHandler), typeof(AuthService))
    .AddClasses(classes => classes.AssignableToAny(typeof(IJwtHandler), typeof(IAuthService), typeof(IUserRepository)))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]))
    };
});

builder.Services.AddControllers();

// Add Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rino API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Add Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rino API V1");
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
