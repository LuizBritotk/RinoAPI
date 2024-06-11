using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Rino.Domain.Entities;
using Rino.Domain.Repositories;
using Rino.Domain.Services;
using Rino.Infrastructure.Utilities;

namespace Rino.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtHandler _jwtHandler;
        private readonly PasswordHasher _passwordHasher;

        public AuthService(IUserRepository userRepository, IJwtHandler jwtHandler, PasswordHasher passwordHasher)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _jwtHandler = jwtHandler ?? throw new ArgumentNullException(nameof(jwtHandler));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public async Task<string> LoginAsync(LoginCommand loginCommand)
        {
            if (loginCommand == null)
                throw new ArgumentNullException(nameof(loginCommand));

            var user = await _userRepository.FindByUsernameAndPassword(loginCommand);

            if (user == null || !_passwordHasher.VerifyPassword(loginCommand.Password, user.PasswordHash))
                return null;

            return _jwtHandler.GenerateToken(user);
        }
    }
}
