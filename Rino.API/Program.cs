// Importações de namespaces necessárias
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Rino.API.Configurations;
using Rino.Domain.Handlers;
using Rino.Domain.Interfaces; // Importação da interface IPasswordHasher
using Rino.Domain.Repositories;
using Rino.Domain.Services;
using Rino.Infrastructure.Authentication;
using Rino.Infrastructure.Data;
using Rino.Infrastructure.Repositories;
//using Rino.Infrastructure.Services;
using Rino.Infrastructure.Utilities;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configurações para o JWT
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Registro do PasswordHasher como serviço Singleton para uso global
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

// Registro do JwtHandler como serviço Scoped para ser instanciado uma vez por requisição
builder.Services.AddScoped<IJwtHandler, JwtHandler>();

// Registro do UserHandler como serviço Transient para ser instanciado uma vez por requisição
builder.Services.AddTransient<UserHandler>();

// Registro de outros serviços usando varredura automática e configuração de ciclo de vida como Scoped
builder.Services.Scan(scan => scan
    .FromAssembliesOf(typeof(IJwtHandler), typeof(AuthService))
    .AddClasses(classes => classes.AssignableToAny(typeof(IJwtHandler), typeof(IAuthService), typeof(IUserRepository)))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

// Configuração do DbContext usando o Entity Framework Core com conexão SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuração de autenticação usando JWT Bearer
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Parâmetros de validação do token JWT
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

// Configuração dos controllers da API
builder.Services.AddControllers();

// Configuração do Swagger para documentação da API
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rino API", Version = "v1" });
});

var app = builder.Build();

// Configuração do pipeline de requisição HTTP

// Página de exceção de desenvolvedor ativada apenas em ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Middlewares para roteamento, autenticação e autorização
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Configuração do Swagger UI para visualização da documentação da API
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rino API V1");
});

// Configuração dos endpoints da API
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Execução da aplicação
app.Run();
