using System;
using System.Threading.Tasks;
using Rino.Domain.Entities;
using Rino.Domain.Interfaces;
using Rino.Domain.Repositories;
using Rino.Domain.Services;

namespace Rino.Domain.Handlers
{
    public class UserHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IPasswordHasher _passwordHasher;

        public UserHandler(IUserRepository userRepository, IAuthService authService, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _authService = authService;
            _passwordHasher = passwordHasher;
        }

        // Método para autenticar um usuário
        public async Task<User> Authenticate(LoginCommand loginCommand)
        {
            try
            {
                return await _authService.LoginAsync(loginCommand);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // Método para registrar um novo usuário
        public async Task<OperationResult> RegisterUser(UserCreateCommand userCommand)
        {
            try
            {
                if (userCommand == null)
                    throw new ArgumentNullException(nameof(userCommand));

                var existingUser = await _userRepository.GetUserByUsername(userCommand.Login);

                if (existingUser != null)
                    return new OperationResult(false, "Usuario já existe.");

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = userCommand.Email,
                    Login = userCommand.Login,
                    FirstName = userCommand.FirstName,
                    LastName = userCommand.LastName,
                    PhoneNumber = userCommand.PhoneNumber,
                    DateOfBirth = userCommand.DateOfBirth,
                    PasswordHash = userCommand.PasswordHash
                };

                await _userRepository.CreateUser(user);

                return new OperationResult(true, "User registered successfully");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
