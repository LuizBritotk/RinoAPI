using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Rino.Domain.Entities;
using Rino.Domain.Services;

namespace Rino.Infrastructure.Authentication
{
    public class JwtHandler : IJwtHandler
    {
        private readonly IConfiguration _configuration;

        public JwtHandler(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string GenerateToken(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User cannot be null.");

            var secret = _configuration["JwtSettings:Secret"];
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var expirationMinutes = int.Parse(_configuration["JwtSettings:ExpirationMinutes"]);

            if (string.IsNullOrWhiteSpace(secret))
                throw new InvalidOperationException("JWT Secret cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(issuer))
                throw new InvalidOperationException("JWT Issuer cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(audience))
                throw new InvalidOperationException("JWT Audience cannot be null or empty.");

            try
            {
                // Lista de claims padrão
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("password_reset", "true") // Claim de teste para reset de senha
                };

                // Cria a chave de segurança com base no segredo configurado
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // Cria o token JWT com as reivindicações e credenciais de assinatura
                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(expirationMinutes),
                    signingCredentials: creds
                );

                // Escreve o token JWT como uma string
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                // Log da exceção
                Console.Error.WriteLine($"Error generating JWT: {ex.Message}");
                throw new InvalidOperationException("An error occurred while generating the JWT.", ex);
            }
        }
    }
}
