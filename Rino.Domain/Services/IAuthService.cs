using Rino.Domain.Entities;

namespace Rino.Domain.Services
{
    public interface IAuthService
    {
        Task<User> LoginAsync(LoginCommand loginCommand);
    }
}
