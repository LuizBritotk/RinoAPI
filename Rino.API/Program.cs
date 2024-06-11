using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Rino.Domain.Services;
using Rino.Infrastructure.Data;
using Rino.Infrastructure.Services;
using Rino.Domain.Repositories;
using Rino.API.Configurations;
using Rino.Infrastructure.Utilities;
using Rino.Domain.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Configura��es e servi�os

#region Configura��o de Depend�ncias e Inje��o de Depend�ncia

// Add services to the container.
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Registrar PasswordHasher
builder.Services.AddSingleton<PasswordHasher>();

// Registrar Handlers
builder.Services.AddTransient<UserHandler, UserHandler>();

// Registrar todos os servi�os no assembly atual
builder.Services.Scan(scan => scan
    .FromAssembliesOf(typeof(IJwtHandler), typeof(AuthService))
    .AddClasses(classes => classes.AssignableToAny(typeof(IJwtHandler), typeof(IAuthService), typeof(IUserRepository)))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

#endregion

#region Configura��o do Banco de Dados

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

#endregion

#region Configura��o de Autentica��o e Autoriza��o

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]!))
    };
});

#endregion

#region Configura��o dos Controladores e Rotas

builder.Services.AddControllers();

#endregion

#region Configura��o do Swagger

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rino API", Version = "v1" });
});

#endregion

var app = builder.Build();

// Configura��o do Pipeline de Requisi��o HTTP

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Configura��o do Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rino API V1");
});

app.UseEndpoints(endpoints =>
{
    endpoints?.MapControllers();
});

app.Run();
