// Importa��es de namespaces necess�rias
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Rino.API.Configurations;
using Rino.Domain.Handlers;
using Rino.Domain.Interfaces; // Importa��o da interface IPasswordHasher
using Rino.Domain.Repositories;
using Rino.Domain.Services;
using Rino.Infrastructure.Authentication;
using Rino.Infrastructure.Data;
using Rino.Infrastructure.Repositories;
//using Rino.Infrastructure.Services;
using Rino.Infrastructure.Utilities;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configura��es para o JWT
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Registro do PasswordHasher como servi�o Singleton para uso global
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

// Registro do JwtHandler como servi�o Scoped para ser instanciado uma vez por requisi��o
builder.Services.AddScoped<IJwtHandler, JwtHandler>();

// Registro do UserHandler como servi�o Transient para ser instanciado uma vez por requisi��o
builder.Services.AddTransient<UserHandler>();

// Registro de outros servi�os usando varredura autom�tica e configura��o de ciclo de vida como Scoped
builder.Services.Scan(scan => scan
    .FromAssembliesOf(typeof(IJwtHandler), typeof(AuthService))
    .AddClasses(classes => classes.AssignableToAny(typeof(IJwtHandler), typeof(IAuthService), typeof(IUserRepository)))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

// Configura��o do DbContext usando o Entity Framework Core com conex�o SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configura��o de autentica��o usando JWT Bearer
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Par�metros de valida��o do token JWT
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

// Configura��o dos controllers da API
builder.Services.AddControllers();

// Configura��o do Swagger para documenta��o da API
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rino API", Version = "v1" });
});

var app = builder.Build();

// Configura��o do pipeline de requisi��o HTTP

// P�gina de exce��o de desenvolvedor ativada apenas em ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Middlewares para roteamento, autentica��o e autoriza��o
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Configura��o do Swagger UI para visualiza��o da documenta��o da API
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rino API V1");
});

// Configura��o dos endpoints da API
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Execu��o da aplica��o
app.Run();
