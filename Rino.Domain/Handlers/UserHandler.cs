using Rino.Domain.Entities;
using Rino.Domain.Services;

namespace Rino.Domain.Handlers
{
    public class UserHandler
    {
        private readonly IAuthService _authService;

        public UserHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<string> Authenticate(LoginCommand loginCommand)
        {
            return await _authService.LoginAsync(loginCommand);
        }
    }
}